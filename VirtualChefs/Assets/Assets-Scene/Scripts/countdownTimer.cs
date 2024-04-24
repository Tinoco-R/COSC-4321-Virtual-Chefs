using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class countdownTimer : MonoBehaviour
{
    [SerializeField] public float gameTime;
    [SerializeField] TMP_Text timeTextBox;
    private float initialGameTime;


    // Start is called before the first frame update
    void Start()
    {
        // Set initial color to green
        timeTextBox.color = Color.green;
        initialGameTime = gameTime;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateGameTimer();
    }

    private void UpdateGameTimer()
    {
        
        GameObject ScoreInstance = GameObject.FindGameObjectWithTag("ScoreBox");
        TotalScoreReader totalScore = ScoreInstance.GetComponent<TotalScoreReader>();
        
        
        gameTime -= Time.deltaTime;

        if (gameTime <= 0 )
        {
            timeTextBox.text = "Gameover";
        }
        else if(gameTime >0)
        {
            var minutes = Mathf.FloorToInt(gameTime / 60);
            var seconds = Mathf.FloorToInt(gameTime - minutes * 60);

            string gameTimeClockDisplay = string.Format("{0:0}:{1:00}", minutes, seconds);

            timeTextBox.text = gameTimeClockDisplay;

            // Change text color based on remaining time
            float normalizedTime = gameTime / initialGameTime;
            timeTextBox.color = Color.Lerp(Color.green, Color.red, 1 - normalizedTime);
            
        }



    }
}