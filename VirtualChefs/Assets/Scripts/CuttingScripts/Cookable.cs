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
    public bool cooking;
    public AudioSource cookSound;

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
        string nextCookLevel;

        // Meats
        if (currentCookLevel == "UncookedMeat")
        {
            nextCookLevel = "RareMeat";
            cookedPrefabDirectory = "Prefabs/Cook/RareMeat";
        }
        else if (currentCookLevel == "RareMeat")
        {
            nextCookLevel = "MediumMeat";
            cookedPrefabDirectory = "Prefabs/Cook/MediumMeat";
        }
        else if (currentCookLevel == "MediumMeat")
        {
            nextCookLevel = "WellDoneMeat";
            cookedPrefabDirectory = "Prefabs/Cook/WellDoneMeat";
        }
        else if (currentCookLevel == "WellDoneMeat")
        {
            nextCookLevel = "BurntMeat";
            cookedPrefabDirectory = "Prefabs/Cook/BurntMeat";
        }

        // Breads
        else if (currentCookLevel == "BottomBun")
        {
            nextCookLevel = "BottomBunToasted";
            cookedPrefabDirectory = "Prefabs/Combine/BottomBunToasted";
        }
        else if (currentCookLevel == "BottomBunToasted")
        {
            nextCookLevel = "BottomBunBurnt";
            cookedPrefabDirectory = "Prefabs/Combine/BottomBunBurnt";
        }
        else if (currentCookLevel == "TopBun")
        {
            nextCookLevel = "TopBunToasted";
            cookedPrefabDirectory = "Prefabs/Combine/TopBunToasted";
        }
        else if (currentCookLevel == "TopBunToasted")
        {
            nextCookLevel = "TopBunBurnt";
            cookedPrefabDirectory = "Prefabs/Combine/TopBunBurnt";
        }

        // Non-cookable / Fully cooked version of food
        else
        {
            return currentCookLevel;
        }
        return nextCookLevel;
    }

    // Food objects are: NameOfFoodBlock, so remove "block" to be able to use when dynamically loading cut game object later
    string GetFoodTypeFromTag()
    {
        string tag = this.tag;
        int meatIndex = tag.IndexOf("Meat");
        int breadIndex = tag.IndexOf("Bun");

        if (meatIndex != -1 ^ breadIndex != -1)
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

    void InitializeCookable(float initialPlaybackTime = 0.0f)
    {
        string foodType = GetFoodTypeFromTag();

        cookProgress = 0.0f;
        cookGoal = 5.0f;

        if (this.tag == "BurntMeat" ^ this.tag == "BottomBunBurnt" ^ this.tag == "TopBunBurnt")
        {
            cooked = true;
            return;
        }


        cooked = false;
        cooking = false;
        cookSound = GetComponent<AudioSource>();

        if (cookSound == null)
        {
            print("Null audio source in " + this.tag);
        }
        else
        {
            cookSound.time = initialPlaybackTime;
            if (cooking)
            {
                cookSound.Play();
            }
        }

        UpdateProgressBar();

        // Load the prefab of cooked version of food from the Resources folder
        cookedPrefab = Resources.Load<GameObject>(cookedPrefabDirectory);
    }

    // Used to set progress bar UI element
    void UpdateProgressBar()
    {
        if (this.tag == "BurntMeat" ^ this.tag == "BottomBunBurnt" ^ this.tag == "TopBunBurnt")
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
        if (cookedPrefabDirectory == "Prefabs/Cook/BurntMeat" ^ cookedPrefabDirectory == "Prefabs/Combine/BottomBunBurnt" ^ cookedPrefabDirectory == "Prefabs/Combine/TopBunBurnt")
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

    void fullyCooked(float initialPlaybackTime = 0.0f)
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
        cookedCookable.InitializeCookable(initialPlaybackTime);
        cookedCookable.CopyFromProgressBar(tempProgressBar);
    }

    private void OnTriggerStay(Collider other)
    {

        int burntIndex = this.tag.IndexOf("Burnt");
        // Check if the collider is the stove
        if (other.gameObject.CompareTag("Stove") && burntIndex == -1/*!cooked && this.tag != "BurntMeat" && this.tag != "BottomBunBurnt" && this.tag != "TopBunBurnt"*/)
        {
            cooking = true;
            // Play cook sound effect
            if (!cookSound.isPlaying)
            {
                cookSound.Play();
            }
            // Increment cookProgress every second the meat stays on the stove
            cookProgress += Time.deltaTime;
            // Update progress bar
            UpdateProgressBar();

            // Check if the meat has been fully cooked
            if (cookProgress >= cookGoal)
            {
                fullyCooked(cookSound.time);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        // Check if the collider is the stove
        if (other.gameObject.CompareTag("Stove"))
        {
            cooking = false;
            // Stop the cooking sound if it's playing
            if (cookSound.isPlaying)
            {
                cookSound.Stop();
            }
        }
    }
}
