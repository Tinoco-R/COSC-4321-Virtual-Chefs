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

        if (gameTime <= 0)
        {
            gameTime = 0;
            isGameOver = true;
            timeTextBox.text = "Game Over";
            // You can add more game over logic here like stopping the game, showing a game over screen, etc.
        }
        else
        {
            var minutes = Mathf.FloorToInt(gameTime / 60);
            var seconds = Mathf.FloorToInt(gameTime - minutes * 60);

            string gameTimeClockDisplay = string.Format("{0:0}:{1:00}", minutes, seconds);

            timeTextBox.text = gameTimeClockDisplay;
        }
    }
}
