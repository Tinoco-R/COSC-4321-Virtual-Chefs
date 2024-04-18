using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class TimeTicket : MonoBehaviour
{
    [SerializeField] ProgressBar progressBar;
    public RecipeWriting recipeWriting;

    public float initialTime;
    public float timeRemaining;
    public bool turnedIn;
    string recipeText;

    // Start is called before the first frame update
    void Start()
    {
        initialTime = 60f;
        timeRemaining = 60f;
        turnedIn = false;

        // Check if recipeWriting and its text component are not null before accessing text.text
        if (recipeWriting != null && recipeWriting.text != null)
        {
            recipeText = recipeWriting.text.text;
        }

        UpdateProgressBar();
    }

    private void Update()
    {
        // Check if text is not null before accessing its text property
        if (recipeWriting != null && recipeWriting.text != null)
        {
            recipeText = recipeWriting.text.text;
            if (recipeText == "No Order" ^ recipeText == "") {
                return;
            }

            timeRemaining -= Time.deltaTime;
            UpdateProgressBar();
        }
    }

    // Used to set progress bar UI element
    void UpdateProgressBar()
    {
        progressBar.minimum = 0;
        progressBar.maximum = (int)initialTime;
        progressBar.current = (int)timeRemaining;

        if (timeRemaining <= 15f)
        {
            // Make bar red!!!
            progressBar.SetFillColor(Color.red);
        }
        else if (timeRemaining <= 30f)
        {
            // Make bar orange!!!
            progressBar.SetFillColor(Color.yellow);
        }
    }

}
