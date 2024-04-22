using UnityEngine;

public class FoodCatcher : MonoBehaviour
{
    // Reference to the Combine script on the plate GameObject
    public Combine combineScript;

    private void OnTriggerEnter(Collider other)
    {
        // Calls a method in the Combine script to add the food object to the plate
        combineScript.AddFoodToPlate(other);
    }
}
