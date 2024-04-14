using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TotalScoreReader : MonoBehaviour
{
    public double totalScore;
    public TMP_Text text;

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
        text.text = "Total Score: " + (((int)(totalScore * 100)) / 100);
    }

}
