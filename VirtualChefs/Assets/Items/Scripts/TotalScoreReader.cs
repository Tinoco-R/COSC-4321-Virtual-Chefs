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

    private void OnEnable()
    {
        ReadFood.orderGiven += CalcScore;
    }

    private void OnDisable()
    {
        ReadFood.orderGiven -= CalcScore;
    }

    public void CalcScore(int n, double s)
    {

        totalScore += s;
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
