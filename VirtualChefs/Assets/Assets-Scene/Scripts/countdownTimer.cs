using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class countdownTimer : MonoBehaviour
{
    [SerializeField] private float gameTime;
    [SerializeField] TMP_Text timeTextBox;
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

        UpdateGameTimer();
        
    }

    private void UpdateGameTimer() 
    {
        gameTime -= Time.deltaTime;


            var minutes = Mathf.FloorToInt(gameTime / 60);
            var seconds = Mathf.FloorToInt(gameTime - minutes * 60);

            string gameTimeClockDisplay = string.Format("{0:0}:{1:00}", minutes, seconds);

            timeTextBox.text = gameTimeClockDisplay;

            // Change text color based on remaining time
            float normalizedTime = gameTime / 60.0f; // Timer at 60 seconds
            timeTextBox.color = Color.Lerp(Color.green, Color.red, 1 - normalizedTime);
        
    }
}
