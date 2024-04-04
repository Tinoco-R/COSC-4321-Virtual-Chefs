 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cuttable : MonoBehaviour
{
   
    [SerializeField] ProgressBar progressBar;

    GameObject slicedPrefab;

    public int cutProgress;
    public int cutGoal;
    public bool cut;

    // Food objects are: NameOfFoodBlock, so remove "block" to be able to use when dynamically loading cut game object later
    string GetFoodTypeFromTag()
    {
        string tag = this.tag;
        int blockIndex = tag.IndexOf("Block");

        if (blockIndex != -1)
        {
            return tag.Substring(0, blockIndex) + "Slice";
        }
        else
        {
            return tag;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        string foodType = GetFoodTypeFromTag();

        cutProgress = 0;
        cutGoal = 5;
        cut = false;

        UpdateProgressBar();

        // Load the prefab of cut version of food from the Resources folder
        slicedPrefab = Resources.Load<GameObject>("Prefabs/Combine/" + foodType);
    }

    // Used to set progress bar UI element
    void UpdateProgressBar()
    {
        progressBar.minimum = 0;
        progressBar.maximum = cutGoal;
        progressBar.current = cutProgress;
    }

    void fullyCut()
    {
        cut = true;
        // Removes progress bar with some kind of UI to show task completed (smoke effect, stars, object shimmer, etc.)
        // TO DO

        // Gets object position
        Vector3 blockPosition = transform.position;
        Quaternion blockRotation = transform.rotation;

        // Deactivates the original object block
        gameObject.SetActive(false);

        // Places cut version of object on original position
        GameObject slicedObject = Instantiate(slicedPrefab, blockPosition, blockRotation);
    }

    private void OnTriggerEnter(Collider other)
    {
        // If mesh box collides with knife call cut
        if (other.gameObject.CompareTag("Knife") && !cut)
        {
            cutProgress++;
            if (cutProgress >= cutGoal)
            {
                fullyCut();
            }
            UpdateProgressBar();
        }
    }
}
