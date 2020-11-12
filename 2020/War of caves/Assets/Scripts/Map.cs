using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Map : MonoBehaviour
{
    public float width;
    public float height;

    public float gridWidth;

    private List<List<GameObject>> grid = new List<List<GameObject>>();

    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < width; i++){

            grid.Add(new List<GameObject>());
            for(int j = 0; j < height; j++){
                GameObject square = createSquare(i + 1, j + 1);

                Instantiate(square, this.transform);
                grid[i].Add(square);
            }
        }
    }

    /**
     * Creates 1 square of the grid
     */
    GameObject createSquare(float x, float y){
        GameObject square = new GameObject("Grid X: " + x.ToString() + " Y: " + y.ToString());
        square.hideFlags = HideFlags.HideInHierarchy;
        square.transform.localScale = new Vector3(gridWidth, 1, gridWidth);

        square.transform.position = new Vector3(x * (gridWidth * 10), 0, y * (gridWidth * 10));

        return square;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
