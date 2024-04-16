using Oculus.Interaction;
using Oculus.Interaction.HandGrab;
using UnityEngine;
using TMPro;
using System.Collections;
using System.Linq; // Add this line to use LINQ

public class Tutorial : MonoBehaviour
{
    [SerializeField] private TMP_Text text;
    [SerializeField] private AudioClip CompleteSound;

    private int currentStage = 0;
    private bool tutorialCompleted = false;
    private AudioSource audioSource;

    private void Start()
    {
        
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
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
                PlaySound(CompleteSound);
               
                break;
            case 2:
                text.text = "Good job! You've grabbed the knife. Now grab the Cheese.";
                PlaySound(CompleteSound);
                break;
            case 3:
                text.text = "Good job! You've grabbed the lettuce. Now grab the meat.";
                PlaySound(CompleteSound);
                break;
            case 4:
                text.text = "Good job! You've grabbed the meat. Now grab the plate.";
                PlaySound(CompleteSound);
                break;
            case 5:
                text.text = "Good job! You've grabbed the plate. Now cut the lettuce and place on the plate";
                PlaySound(CompleteSound);
                
                break;
            case 6:
                text.text = "Good job! You've placed the lettuce on the plate. Now cook the meat and place on the plate";
                PlaySound(CompleteSound);
                break;
            case 7:
                text.text = "Good job! You've placed the meat on the plate. Now turn it in";
                PlaySound(CompleteSound);
                break;
            case 8:
                text.text = "Good job! You've turned in the order! Tutorial Completed!";
                PlaySound(CompleteSound);
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
                else if (CheckGrabbable(grabbedObject, "CheeseBlock", 2))
                {
                    currentStage = 3;
                    UpdateTutorialText();
                }
                else if (CheckGrabbable(grabbedObject, "UncookedMeat", 3))
                {
                    currentStage = 4;
                    UpdateTutorialText();
                }
                
                
                else if (CheckGrabbable(grabbedObject,"Plate" , 4))
                {
                    currentStage = 5;
                    UpdateTutorialText();
                }
                
                else if (CheckGrabbable(grabbedObject, "CheeseSlice", 5))
                {
                    currentStage = 6;
                    UpdateTutorialText();
                }
                else if (CheckGrabbable(grabbedObject, "CookedMeat", 6))
                {
                    currentStage = 7;
                    UpdateTutorialText();
                }
                else if (CheckTurnInZoneScore())
                {
                    currentStage = 8;
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
    private bool CheckTurnInZoneScore()
    {
        // Find the TurnInZone prefab instance by its tag
        GameObject turnInZoneInstance = GameObject.FindGameObjectWithTag("TurnInZone");

        if (turnInZoneInstance != null)
        {
            // Get the ReadFood component from the TurnInZone instance
            ReadFood readFood = turnInZoneInstance.GetComponent<ReadFood>();

            if (readFood.score != 0)
            {
                // The score is not 0, so return true
                return true;
            }
        }

        // The TurnInZone instance or its ReadFood component was not found, or the score is 0
        return false;
    }

    
    private void PlaySound(AudioClip clip)
    {
        if (clip != null && audioSource != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}