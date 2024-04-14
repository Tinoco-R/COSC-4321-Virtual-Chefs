using UnityEngine;
using TMPro;

public class NewTutorial : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    [SerializeField] private GameObject knife;
    [SerializeField] private GameObject lettuce;
    [SerializeField] private GameObject meat;
    [SerializeField] private GameObject plate;
    [SerializeField] private GameObject ticket;

    private bool knifeGrabbed = false;
    private bool lettuceGrabbed = false;
    private bool meatGrabbed = false;
    private bool plateGrabbed = false;
    private bool ticketGrabbed = false;

    private void Update()
    {
        UpdateTutorialText();
    }

    private void UpdateTutorialText()
    {
        // Check if the ticket has been grabbed
        if (!ticketGrabbed)
        {
            text.text = "Grab the ticket to start the tutorial.";
            return;
        }

        // Check if the knife has been grabbed
        if (!knifeGrabbed)
        {
            text.text = "Grab the knife.";
            return;
        }

        // Check if the lettuce has been grabbed
        if (!lettuceGrabbed)
        {
            text.text = "Grab the lettuce.";
            return;
        }

        // Check if the meat has been grabbed
        if (!meatGrabbed)
        {
            text.text = "Grab the meat.";
            return;
        }

        // Check if the plate has been grabbed
        if (!plateGrabbed)
        {
            text.text = "Grab the plate.";
            return;
        }

        // All items grabbed, display a message indicating completion
        text.text = "Tutorial completed!";
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ticket"))
        {
            ticketGrabbed = true;
            text.text = "Good job! You've grabbed the ticket. Now grab the knife.";
        }
        else if (other.gameObject.CompareTag("Knife"))
        {
            knifeGrabbed = true;
            text.text = "Good job! You've grabbed the knife. Now grab the lettuce.";
        }
        else if (other.gameObject.CompareTag("LettuceBlock"))
        {
            lettuceGrabbed = true;
            text.text = "Good job! You've grabbed the lettuce. Now grab the meat.";
        }
        else if (other.gameObject.CompareTag("UncookedMeat"))
        {
            meatGrabbed = true;
            text.text = "Good job! You've grabbed the meat. Now grab the plate.";
        }
        else if (other.gameObject.CompareTag("Plate"))
        {
            plateGrabbed = true;
            text.text = "Good job! You've grabbed the plate. Tutorial completed!";
        }
    }
}