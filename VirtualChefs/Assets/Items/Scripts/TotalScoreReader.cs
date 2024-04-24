using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TotalScoreReader : MonoBehaviour
{
    public double totalScore;
    [SerializeField]
    private TMP_Text text;
    [SerializeField] TextMeshProUGUI highScoreText;

    private void Start()
    {
        totalScore = 0;
      
    }



     void Update ()
    {

        
        TMP_Text textComponent = text.GetComponent<TMP_Text>();
        textComponent.SetText("Total Score: " + (((int)(totalScore * 100)) / 100));
    }

    void CheckHighScore()
    {
        if(totalScore > PlayerPrefs.GetInt("HighScore", 0))
        {
            PlayerPrefs.SetInt("HighScore", (int)totalScore);
           
        }
    }



}
