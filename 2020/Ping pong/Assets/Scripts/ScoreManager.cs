using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using UnityEngine.UI;

public class ScoreManager : MonoBehaviour
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void addPoint(int playerIndex){
        GameObject score = transform.GetChild(playerIndex).gameObject;

        int currentScore = int.Parse(score.GetComponent<Text>().text);
        currentScore += 1;
        score.GetComponent<Text>().text = currentScore.ToString();
    }
}
