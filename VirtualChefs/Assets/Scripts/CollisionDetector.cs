using System.Collections.Generic;
using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private AudioSource audioSource;
    private List<string> foodItems = new() { "BottomBun", "BottomBunToasted", "CheeseSlice", "LettuceSlice", "TomatoSlice", "TopBun", "TopBunToasted", "RareMeat", "MediumMeat", "WellDoneMeat" };
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        // Check if the collided gameObject's tag is in the foodItems list
        if (this.gameObject.tag == "Plate" && foodItems.Contains(collision.gameObject.tag))
        {
            // If the collision is with a food item, and you are a plate, return without playing a sound
            return;
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}