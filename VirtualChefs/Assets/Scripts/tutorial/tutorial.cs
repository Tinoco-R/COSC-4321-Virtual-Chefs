using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Linq; // Add this line to use LINQ

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private int currentStage = 0;
    private bool tutorialCompleted = false;

    private void Start()
    {
        UpdateTutorialText();
        StartCoroutine(CheckGrabbedObjects());
    }

    private void UpdateTutorialText()
    {
        switch (currentStage)
        {
            case 0:
                text.text = "Grab the ticket to start the tutorial.";
                break;
            case 1:
                text.text = "Good job! You've grabbed the ticket. Now grab the knife.";
                break;
            case 2:
                text.text = "Good job! You've grabbed the knife. Now grab the lettuce.";
                break;
            case 3:
                text.text = "Good job! You've grabbed the lettuce. Now grab the meat.";
                break;
            case 4:
                text.text = "Good job! You've grabbed the meat. Now grab the plate.";
                break;
            case 5:
                text.text = "Good job! You've grabbed the plate. Tutorial completed!";
                tutorialCompleted = true;
                break;
        }
    }

    private IEnumerator CheckGrabbedObjects()
    {
        while (!tutorialCompleted)
        {
            Grabbable grabbedObject = FindGrabbedObject();

            if (grabbedObject != null)
            {
                if (CheckGrabbable(grabbedObject, "Ticket", 0))
                {
                    currentStage = 1;
                    UpdateTutorialText();
                }
                else if (CheckGrabbable(grabbedObject, "Knife", 1))
                {
                    currentStage = 2;
                    UpdateTutorialText();
                }
                else if (CheckGrabbable(grabbedObject, "LettuceBlock", 2))
                {
                    currentStage = 3;
                    UpdateTutorialText();
                }
                else if (CheckGrabbable(grabbedObject, "UncookedMeat", 3))
                {
                    currentStage = 4;
                    UpdateTutorialText();
                }
                else if (CheckGrabbable(grabbedObject, "Plate", 4))
                {
                    currentStage = 5;
                    UpdateTutorialText();
                }
            }

            yield return null;
        }
    }

    private Grabbable FindGrabbedObject()
    {
        // Find all Grabbable components in the scene
        Grabbable[] grabbables = FindObjectsOfType<Grabbable>();

        // Return the first grabbed object, or null if none are grabbed
        return grabbables.FirstOrDefault(g => g.isGrabbed);
    }

    private bool CheckGrabbable(Grabbable grabbable, string tag, int stage)
    {
        if (currentStage == stage && grabbable.gameObject.CompareTag(tag))
        {
            return true;
        }
        return false;
    }
}