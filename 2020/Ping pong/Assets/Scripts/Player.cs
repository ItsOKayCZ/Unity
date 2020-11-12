using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject topBorder;
    public GameObject bottomBorder;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        float height = transform.localScale.y / 2;

        Vector3 pos = transform.position;
        if(transform.position.y + height > topBorder.transform.position.y){
            pos.y = topBorder.transform.position.y - height;
        }

        if(transform.position.y - height < bottomBorder.transform.position.y){
            pos.y = bottomBorder.transform.position.y + height;
        }

        transform.position = pos;
    }
}
