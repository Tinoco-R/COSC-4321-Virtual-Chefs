using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Tutorial : MonoBehaviour
{
    public TMP_Text text;

    public GameObject cuttingCounter;
    public GameObject stove;
    public GameObject plate;
    public GameObject lettuce;
    public GameObject meat;
    public GameObject knife;
    public GameObject ticket;

    private ObjectGrabCheck knifeGrabCheck;
    private ObjectGrabCheck lettuceGrabCheck;
    private ObjectGrabCheck meatGrabCheck;
    private ObjectGrabCheck plateGrabCheck;

    private bool hasGrabbedTicket = false;
    private bool hasGrabbedBunTop = false;
    private bool hasGrabbedBunBottom = false;
    private bool hasGrabbedLettuce = false;
    private bool hasGrabbedMeat = false;
    private bool hasGrabbedKnife = false;
    private bool hasCutLettuce = false;
    private bool hasCookedMeat = false;
    private bool hasPlacedIngredientsOnPlate = false;

    void Start()
    {
        knifeGrabCheck = knife.GetComponent<ObjectGrabCheck>();
        lettuceGrabCheck = lettuce.GetComponent<ObjectGrabCheck>();
        meatGrabCheck = meat.GetComponent<ObjectGrabCheck>();
        plateGrabCheck = plate.GetComponent<ObjectGrabCheck>();
    }

    void Update()
    {
        UpdateTutorialText();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == ticket)
        {
            hasGrabbedTicket = true;
        }
    }

    void UpdateTutorialText()
    {
        string tutorialText = "";

        // Check if the ticket has been grabbed
        if (!hasGrabbedTicket)
        {
            tutorialText += "Grab the ticket to start the tutorial.\n";
            text.text = tutorialText;
            return;
        }

        // Check if the buns, lettuce, and meat have been grabbed from the fridge
        if (hasGrabbedBunTop && hasGrabbedBunBottom && hasGrabbedLettuce && hasGrabbedMeat)
        {
            tutorialText += "Buns, lettuce, and meat grabbed.\n";
        }
        else
        {
            tutorialText += "Go to the fridge and grab top and bottom bun, lettuce, and meat.\n";
        }

        // Check if the knife has been grabbed
        if (hasGrabbedKnife || knifeGrabCheck.IsBeingHeld())
        {
            hasGrabbedKnife = true;
            tutorialText += "Knife grabbed.\n";
        }
        else
        {
            tutorialText += "Grab the knife.\n";
        }

        // Check if the lettuce has been grabbed
        if (hasGrabbedLettuce || lettuceGrabCheck.IsBeingHeld())
        {
            hasGrabbedLettuce = true;
            tutorialText += "Lettuce grabbed.\n";
        }
        else
        {
            tutorialText += "Grab the lettuce.\n";
        }

        // Check if the meat has been grabbed
        if (hasGrabbedMeat || meatGrabCheck.IsBeingHeld())
        {
            hasGrabbedMeat = true;
            tutorialText += "Meat grabbed.\n";
        }
        else
        {
            tutorialText += "Grab the meat.\n";
        }

        // Check if the plate has been grabbed
        if (hasGrabbedBunTop || plateGrabCheck.IsBeingHeld())
        {
            hasGrabbedBunTop = true;
            tutorialText += "Plate grabbed.\n";
        }
        else
        {
            tutorialText += "Grab the plate.\n";
        }

        // Check if the lettuce has been cut on the cutting counter
        if (hasGrabbedKnife && cuttingCounter.transform.childCount > 0)
        {
            hasCutLettuce = true;
            tutorialText += "Lettuce cut.\n";
        }
        else
        {
            tutorialText += "Go to the cutting counter and cut lettuce.\n";
        }

        // Check if the meat has been cooked on the stove
        if (hasGrabbedMeat && stove.transform.childCount > 0)
        {
            hasCookedMeat = true;
            tutorialText += "Meat cooked.\n";
        }
        else
        {
            tutorialText += "Go to the stove and cook the meat.\n";
        }

        // Check if all ingredients have been placed on the plate
        if (hasGrabbedBunTop && hasGrabbedBunBottom && hasCutLettuce && hasCookedMeat)
        {
            hasPlacedIngredientsOnPlate = true;
            tutorialText += "Ingredients placed on plate.\n";
        }
        else
        {
            tutorialText += "Grab a plate and put all the ingredients.\n";
        }

        // Display final instruction to turn in the plate
        if (hasPlacedIngredientsOnPlate)
        {
            tutorialText += "Turn in the plate.";
        }

        text.text = tutorialText;
    }


}
