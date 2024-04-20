using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class Cookable : MonoBehaviour
{
    [SerializeField] ProgressBar progressBar;

    GameObject cookedPrefab;

    public float cookProgress;
    public float cookGoal;
    public bool cooked;

    private string cookedPrefabDirectory;

    void CopyFromProgressBar(ProgressBar original)
    {
        this.progressBar.minimum = original.minimum;
        this.progressBar.maximum = original.maximum;
        this.progressBar.mask = original.mask;
        this.progressBar.fill = original.fill;
        this.progressBar.color = original.color;
    }

    string NextCookLevel(string currentCookLevel)
    {
        string cookLevel;

        if (currentCookLevel == "UncookedMeat")
        {
            cookLevel = "CookedMeat";
            cookedPrefabDirectory = "Prefabs/Cook/CookedMeat";
        }
        else if (currentCookLevel == "CookedMeat")
        {
            cookLevel = "BurntMeat";
            cookedPrefabDirectory = "Prefabs/Cook/BurntMeat";
        }
        else
        {
            return currentCookLevel;
        }
        return cookLevel;
    }

    // Food objects are: NameOfFoodBlock, so remove "block" to be able to use when dynamically loading cut game object later
    string GetFoodTypeFromTag()
    {
        string tag = this.tag;
        int meatIndex = tag.IndexOf("Meat");

        if (meatIndex != -1)
        {
            tag = NextCookLevel(tag);
        }
        return tag;
    }

    // Start is called before the first frame update
    void Start()
    {
        InitializeCookable();
    }

    void InitializeCookable()
    {
        string foodType = GetFoodTypeFromTag();

        cookProgress = 0.0f;
        cookGoal = 5.0f;

        if (this.tag == "BurntMeat")
        {
            cooked = true;
            return;
        }

        cooked = false;
        UpdateProgressBar();

        // Load the prefab of cooked version of food from the Resources folder
        cookedPrefab = Resources.Load<GameObject>(cookedPrefabDirectory);
    }

    // Used to set progress bar UI element
    void UpdateProgressBar()
    {
        if (this.tag == "BurntMeat")
        {
            return;
        }
        
        progressBar.minimum = 0;
        progressBar.maximum = (int)cookGoal;
        progressBar.current = (int)cookProgress;

        if (progressBar.current == 0) {
            progressBar.mask.enabled = false;
            progressBar.fill.enabled = false;
        }
        // Color progress bar to warn user of potential burnt meat
        if (cookedPrefabDirectory == "Prefabs/Cook/BurntMeat")
        {
            if (cookProgress < 3.0f)
            {
                // Make bar yellow!!!
                progressBar.SetFillColor(Color.yellow);
            }
            else if (cookProgress >= 3.0f)
            {
                // Make bar orange!!!
                progressBar.SetFillColor(Color.red);
            }
        }
    }

    void fullyCooked()
    {
        cooked = true;
        // Removes progress bar with some kind of UI to show task completed (smoke effect, stars, object shimmer, etc.)
        // TO DO

        // Gets object position
        Vector3  meatPosition = transform.position;
        Quaternion meatRotation = transform.rotation;

        // Gets CountDisplay and ProgressBar
        CountDisplay tempDisplay = GetComponent<CountDisplay>();
        ProgressBar tempProgressBar = GetComponent<ProgressBar>();

        // Deactivates the original object block
        gameObject.SetActive(false);

        // Places cut version of object on original position
        GameObject cookedObject = Instantiate(cookedPrefab, meatPosition, meatRotation);

        // Access the Cookable component attached to the cookedObject and set initial properties
        Cookable cookedCookable = cookedObject.GetComponent<Cookable>();
        cookedCookable.InitializeCookable();
        cookedCookable.CopyFromProgressBar(tempProgressBar);
    }

    private void OnTriggerStay(Collider other)
    {
        // Check if the collider is the stove
        UpdateProgressBar();
        if (other.gameObject.CompareTag("Stove") && !cooked && this.tag != "BurntMeat")
        {
            // Increment cookProgress every second the meat stays on the stove
            cookProgress += Time.deltaTime;

            // Update progress bar
            UpdateProgressBar();

            // Check if the meat has been fully cooked
            if (cookProgress >= cookGoal)
            {
                fullyCooked();
            }
        }
    }
}
