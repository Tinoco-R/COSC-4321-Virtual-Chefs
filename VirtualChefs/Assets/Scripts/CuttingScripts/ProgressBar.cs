using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
#if UNITY_EDITOR
using UnityEditor;
#endif

[ExecuteInEditMode()]
public class ProgressBar : MonoBehaviour
{
#if UNITY_EDITOR
    [MenuItem("GameObject/UI/LinearProgressBar")]
    public static void AddLinearProgressBar()
    {
        GameObject obj = Instantiate(Resources.Load<GameObject>("UI/LinearProgressBar"));
        obj.transform.SetParent(Selection.activeGameObject.transform, false);
    }
#endif

    public int minimum;
    public int maximum;
    public int current;
    public Image mask;
    public Image fill;
    public Color color;
    public Image progressBarImage;
    public bool ticket;

    // Method to change the fill color
    public void SetFillColor(Color color)
    {
        this.color = color;
        fill.color = color;
    }

    // Set Minimum value
    public void SetMinimum(int min)
    {
        minimum = min;
    }

    // Set Minimum value
    public void SetMaximum(int max)
    {
        maximum = max;
    }

    // Start is called before the first frame update
    void Start()
    {
        mask.enabled = false;
        fill.enabled = false;
        HideProgressBar();
        if (current > 0)
        {
            ticket = true;
        }
    }

    // Method to show the progress bar
    public void ShowProgressBar()
    {
        progressBarImage.enabled = true;
    }

    // Method to hide the progress bar
    public void HideProgressBar()
    {
        progressBarImage.enabled = false;
    }


    // Update is called once per frame
    void Update()
    {
        if (ticket)
        {
            GetCurrentFillTicket();
        }
        else
        {
            GetCurrentFill();
        }
    }

    void GetCurrentFillTicket()
    {
        // Check if current is greater than 0
        if (current != maximum)
        {
            ShowProgressBar();
            // Calculate fill amount
            float currentOffset = (float)current - (float)minimum;
            float maximumOffset = (float)maximum - (float)minimum;
            float fillAmount = currentOffset / maximumOffset;

            // Update fill amount and enable the mask and fill
            mask.fillAmount = fillAmount;
            mask.enabled = true;
            fill.enabled = true;
            fill.color = color;
        }
        else
        {
            // If current is not greater than 0, disable the mask and fill
            mask.enabled = false;
            fill.enabled = false;
            HideProgressBar();
        }
    }
    void GetCurrentFill()
    {
        // Check if current is greater than 0
        if (current > 0)
        {
            ShowProgressBar();
            // Calculate fill amount
            float currentOffset = (float)current - (float)minimum;
            float maximumOffset = (float)maximum - (float)minimum;
            float fillAmount = currentOffset / maximumOffset;

            // Update fill amount and enable the mask and fill
            mask.fillAmount = fillAmount;
            mask.enabled = true;
            fill.enabled = true;
            fill.color = color;
        }
        else
        {
            // If current is not greater than 0, disable the mask and fill
            mask.enabled = false;
            fill.enabled = false;
            HideProgressBar();
        }
    }
}
