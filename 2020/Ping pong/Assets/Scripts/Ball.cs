using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject[] players = new GameObject[2];

    public GameObject topBorder;
    public GameObject bottomBorder;
    public GameObject leftBorder;
    public GameObject rightBorder;

    public GameObject scoreObject;
    private ScoreManager scoreManager;

    private struct Directions{
        public bool left;
        public bool right;
        public bool down;
        public bool up;

        public Directions(bool _left, bool _right, bool _down, bool _up){
            left = _left;
            right = _right;
            down = _down;
            up = _up;
        }
    }
    Directions directions = new Directions(false, false, false, false);

    public float speed = 0.05f;
    private float startSpeed;

    // Start is called before the first frame update
    void Start()
    {
        startSpeed = speed;

        scoreManager = scoreObject.GetComponent<ScoreManager>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 position = transform.position;
        if(directions.left){
            position.x -= speed;
        } else if(directions.right){
            position.x += speed;
        }

        if(directions.up){
            position.y += speed;
        } else if(directions.down){
            position.y -= speed;
        }
        transform.position = position;

        checkIfWin();
        checkOutOfWindow();
        checkCollision();
    }

    void checkIfWin(){

        if(transform.position.x < leftBorder.transform.position.x){
            scoreManager.addPoint(1);
            resetBall();
        }

        if(transform.position.x > rightBorder.transform.position.x){
            scoreManager.addPoint(0);
            resetBall();
        }

    }

    void resetBall(){
        speed = startSpeed;

        transform.position = new Vector3(0, 0, 0);
        directions.up = directions.down = false;
    }

    void checkOutOfWindow(){

        float height = transform.localScale.y / 2;

        if(directions.up
        && (transform.position.y + height) > topBorder.transform.position.y){
            directions.up = false;
            directions.down = true;
        }

        if(directions.down
        && (transform.position.y - height) < bottomBorder.transform.position.y){
            directions.down = false;
            directions.up = true;
        }

    }

    void checkCollision(){

        Vector2 size = new Vector2(
            transform.localScale.x / 2,
            transform.localScale.y / 2
        );

        Vector2 playerSize = new Vector2(
            players[0].transform.localScale.x / 2,
            players[0].transform.localScale.y / 2
        );

        int index;
        if(directions.left){
            index = 0;
        } else {
            index = 1;
        }

        if((directions.left && (transform.position.x - size.x < players[0].transform.position.x + playerSize.x))
        || (directions.right && (transform.position.x + size.x > players[1].transform.position.x - playerSize.x))){
            
            if((transform.position.y > players[index].transform.position.y)
            && (transform.position.y < players[index].transform.position.y + playerSize.y)){
                changeDir(true, false);
                return;
            }

            if((transform.position.y < players[index].transform.position.y)
            && (transform.position.y > players[index].transform.position.y - playerSize.y)){
                changeDir(false, true);
                return;
            }

            if(transform.position.y == players[index].transform.position.y){
                changeDir(true, false);
                return;
            }
        }

    }

    void changeDir(bool up, bool down){
        directions.up = up;
        directions.down = down;

        directions.left = !directions.left;
        directions.right = !directions.right;

        speed *= 1.05f;
    }

    public void startGame(){
        directions.left = true;
    }
}
