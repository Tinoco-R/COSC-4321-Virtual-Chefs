using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class countdownTimer : MonoBehaviour
{
    [SerializeField] private float gameTime;
    [SerializeField] TextMeshProUGUI timeTextBox;
    private bool isGameOver = false;

    // Start is called before the first frame update
    void Start()
    {
        // Set initial color to green
        timeTextBox.color = Color.red;

    }

    // Update is called once per frame
    void Update()
    {
        if (!isGameOver)
        {
            UpdateGameTimer();
        }
    }

    private void UpdateGameTimer() 
    {
        gameTime -= Time.deltaTime;

        // Stop timer when it reaches 0, and display game over
        if (gameTime <= 0)
        {
            gameTime = 0;
            isGameOver = true;
            timeTextBox.text = "Game Over";

            // Change timer text color to red when it reaches 0
            timeTextBox.color = Color.red;
        }
        else
        {
            var minutes = Mathf.FloorToInt(gameTime / 60);
            var seconds = Mathf.FloorToInt(gameTime - minutes * 60);

            string gameTimeClockDisplay = string.Format("{0:0}:{1:00}", minutes, seconds);

            timeTextBox.text = gameTimeClockDisplay;

            // Change text color based on remaining time
            float normalizedTime = gameTime / 60.0f; // Timer at 60 seconds
            timeTextBox.color = Color.Lerp(Color.green, Color.red, 1 - normalizedTime);
        }
    }
}
