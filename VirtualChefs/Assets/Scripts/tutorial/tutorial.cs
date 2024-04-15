using UnityEngine;
using TMPro;

public class tutorial : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private ObjectGrabCheck ticketGrabCheck;
    private ObjectGrabCheck knifeGrabCheck;
    private ObjectGrabCheck lettuceGrabCheck;
    private ObjectGrabCheck meatGrabCheck;
    private ObjectGrabCheck plateGrabCheck;

    [SerializeField] private GameObject ticket;
    [SerializeField] private GameObject knife;
    [SerializeField] private GameObject lettuce;
    [SerializeField] private GameObject meat;
    [SerializeField] private GameObject plate;

    private void Start()
    {
        ticketGrabCheck = ticket.AddComponent<ObjectGrabCheck>();
        knifeGrabCheck = knife.AddComponent<ObjectGrabCheck>();
        lettuceGrabCheck = lettuce.AddComponent<ObjectGrabCheck>();
        meatGrabCheck = meat.AddComponent<ObjectGrabCheck>();
        plateGrabCheck = plate.AddComponent<ObjectGrabCheck>();

        UpdateTutorialText();
    }

    private void UpdateTutorialText()
    {
        
        // Check if the ticket has been grabbed
        if (!ticketGrabCheck.IsBeingHeld())
        {
            text.text = "Grab the ticket to start the tutorial.";
        }
        // Check if the knife has been grabbed
        else if (!knifeGrabCheck.IsBeingHeld())
        {
            text.text = "Good job! You've grabbed the ticket. Now grab the knife.";
        }
        // Check if the lettuce has been grabbed
        else if (!lettuceGrabCheck.IsBeingHeld())
        {
            text.text = "Good job! You've grabbed the knife. Now grab the lettuce.";
        }
        // Check if the meat has been grabbed
        else if (!meatGrabCheck.IsBeingHeld())
        {
            text.text = "Good job! You've grabbed the lettuce. Now grab the meat.";
        }
        // Check if the plate has been grabbed
        else if (!plateGrabCheck.IsBeingHeld())
        {
            text.text = "Good job! You've grabbed the meat. Now grab the plate.";
        }
        // All items grabbed, display a message indicating completion
        else
        {
            text.text = "Good job! You've grabbed the plate. Tutorial completed!";
        }
    }
}