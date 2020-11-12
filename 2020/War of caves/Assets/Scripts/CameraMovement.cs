using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    //Movement
    public float camSpeed = 10f;
    public float camBoarderT = 10f;
    public Vector2 camLimitor;
    
    //Zoom
    public float zoomSpeed = 100f;
    public float maxY = 20f;
    public float minY = 20f; 

    void Update()
    {
        Vector3 pos = transform.position;

        if (Input.GetKey("up") || Input.mousePosition.y  >= Screen.height -camBoarderT)
        {
            pos.z += camSpeed * Time.deltaTime;
        }
        if (Input.GetKey("down") || Input.mousePosition.y <= camBoarderT)
        {
            pos.z -= camSpeed * Time.deltaTime;
        }
        if (Input.GetKey("left") || Input.mousePosition.x <= camBoarderT)
        {
            pos.x -= camSpeed * Time.deltaTime;
        }
        if (Input.GetKey("right") || Input.mousePosition.x >= Screen.width - camBoarderT)
        {
            pos.x += camSpeed * Time.deltaTime;
        }

        float zoom = Input.GetAxis("Mouse ScrollWheel");
        pos.y -= zoom * zoomSpeed *1000f *  Time.deltaTime;

        //Clamp movement
        pos.x = Mathf.Clamp(pos.x, -camLimitor.x, camLimitor.x);
        pos.z = Mathf.Clamp(pos.z, -camLimitor.y, camLimitor.y);

        //Clamp zoom
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        transform.position = pos;
    }
}
