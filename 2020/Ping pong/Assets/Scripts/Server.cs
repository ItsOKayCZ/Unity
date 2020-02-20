using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

public class Server : MonoBehaviour
{
    private TcpListener server;

    private int port = 8080;

    private int playerCount = 0;
    public float playerSpeed = 0.005f;

    public GameObject[] players = new GameObject[2];
    private String[] movement = new String[2];

    public GameObject ballObject;
    private Ball ball;

    // Start is called before the first frame update
    void Start()
    {
        ball = ballObject.GetComponent<Ball>();

        try{
            server = new TcpListener(IPAddress.Any, port);
            server.Start();
            Debug.Log("Web server is running");

            Thread th = new Thread(new ThreadStart(StartListen));
            th.Start();   
        } catch(Exception){
            Debug.Log("Could not make server");
        }
    }

    // Update is called once per frame
    void Update()
    {
        for(int i = 0; i < movement.Length; i++){
            if(movement[i] == "up"){
                Vector3 position = players[i].transform.position;
                position.y += playerSpeed;
                players[i].transform.position = position;
            } else if(movement[i] == "down"){
                Vector3 position = players[i].transform.position;
                position.y -= playerSpeed;
                players[i].transform.position = position;
            }
        }
    }

    void StartListen(){
        
        while(true){

            TcpClient client = server.AcceptTcpClient();
            
            NetworkStream stream = client.GetStream();

            Byte[] bytes = new Byte[1024];
            String data = null;
            int i;
            while((i = stream.Read(bytes, 0, bytes.Length)) != 0){
                data = System.Text.Encoding.ASCII.GetString(bytes, 0, i);
                
                if(data.Substring(0, 3) != "GET"){
                    Debug.Log("Didnt get GET method");
                    client.Close();
                    return;
                }
                
                String[] path = data.Split('\n')[0].Split(' ')[1].Split('/');
                String fileName = path[path.Length - 1];

                String httpVersion = data.Split(new String[]{"HTTP/"}, StringSplitOptions.None)[1];

                if(checkParam(data)){
                    sendHeader(httpVersion, "", "200", ref stream);
                    client.Close();
                    break;
                }

                if(fileName == ""){
                    fileName = "index.html";
                }
                if(fileName == "index.html"){
                    Debug.Log($"Player number {++playerCount} connected");
                }


                String content = getFileContent(fileName);
                if(content == ""){
                    sendHeader(httpVersion, "", "404", ref stream);
                    sendToBrowser("<h1>File not found</h1>", ref stream);
                    client.Close();
                    break;
                }

                String[] temp = fileName.Split('.');
                sendHeader(httpVersion, temp[temp.Length - 1], "200", ref stream);
                sendToBrowser(content, ref stream);
                client.Close();

                if(playerCount == 2){
                    ball.startGame();
                }
                break;
            }
        }
    }

    void sendHeader(String httpVersion, String fileExt, String statusCode, ref NetworkStream stream){
        String buffer = "";

        String MIME = "text/html";
        if(fileExt == "css"){
            MIME = "text/css";
        } else if(fileExt == "js"){
            MIME = "text/javascript";
        }

        buffer += "HTTP/1.1 " + statusCode + "\r\n";
        buffer += "Server: Controller\r\n";
        if(fileExt == "html"){
            buffer += $"Set-Cookie: player={playerCount}\r\n";
        }
        buffer += "Content-Type: " + MIME + "\r\n\r\n";
        sendToBrowser(buffer, ref stream);
    }

    void sendToBrowser(String buffer, ref NetworkStream stream){
        try{
            Byte[] res = Encoding.UTF8.GetBytes(buffer);
            stream.Write(res, 0, res.Length);   
        } catch(Exception){
            Debug.Log("Cannot send data to client");
        }
    }

    String getFileContent(String fileName){
        StreamReader sr;

        String content = "";
        try{
            sr = new StreamReader($"Assets/Web files/{fileName}");
            content = sr.ReadToEnd();

        } catch(Exception){
            Debug.Log($"Cannot read file {fileName}");
        }

        return content;
    }

    bool checkParam(String data){
        String[] temp = data.Split('\n')[0].Split('?');

        if(temp.Length == 1){
            return false;
        }

        int playerNumber = int.Parse(data.Split(new string[]{"Cookie: player="}, StringSplitOptions.None)[1][0].ToString()) - 1;
        String paramName = temp[1].Split(' ')[0].Split('&')[0].Split('=')[0];
        String paramValue = temp[1].Split(' ')[0].Split('&')[0].Split('=')[1];
        String bName = temp[1].Split(' ')[0].Split('&')[1].Split('=')[0];
        String bValue = temp[1].Split(' ')[0].Split('&')[1].Split('=')[1];
        if((paramName == "direction" && (paramValue == "up" || paramValue == "down")) 
        && (bName == "b" && (bValue == "true" || bValue == "false"))
        && (playerNumber <= players.Length - 1)){
            movePlayer(playerNumber, paramValue, bValue);
            return true;
        }

        return false;
    }

    void movePlayer(int playerNumber, String direction, String bString){
        bool b = Boolean.Parse(bString);

        if(!b){
            movement[playerNumber] = "null";
        } else {
            movement[playerNumber] = direction;
        }
    }
}
