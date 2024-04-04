using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Placement : MonoBehaviour
{
    public GameObject Wheel; // Reference to the parent GameObject of spawn positions

    private bool isGrabbed = false; // Flag to check if the item is grabbed

    // Update is called once per frame
    void Update()
    {
        if (!isGrabbed && IsInsidePlacementZone())
        {
            // Place the item at the nearest spawn position
            Transform nearestSpawn = GetNearestSpawnPosition();
            if (nearestSpawn != null)
            {
                transform.position = nearestSpawn.position;
            }
        }
    }

    private bool IsInsidePlacementZone()
    {
        // Check if the item is inside the PlacementZone by comparing distances
        float distance = Vector3.Distance(transform.position, transform.parent.position);
        return distance <= transform.parent.localScale.x / 2; // Assuming PlacementZone is a sphere, comparing with its radius
    }

    private Transform GetNearestSpawnPosition()
    {
        Transform nearestSpawn = null;
        float minDistance = Mathf.Infinity;

        foreach (Transform spawnPos in Wheel.transform)
        {
            float distance = Vector3.Distance(transform.position, spawnPos.position);
            if (distance < minDistance)
            {
                minDistance = distance;
                nearestSpawn = spawnPos;
            }
        }

        return nearestSpawn;
    }

    public void GrabItem()
    {
        // Called when the item is grabbed
        isGrabbed = true;
    }

    public void ReleaseItem()
    {
        // Called when the item is released
        isGrabbed = false;
    }
}
