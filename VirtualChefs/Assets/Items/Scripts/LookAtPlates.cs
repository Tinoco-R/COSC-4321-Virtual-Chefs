using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlates : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] gameObjects;
        gameObjects = GameObject.FindGameObjectsWithTag("Plate");
        GameObject lookAt = null;
        if (gameObjects.Length != 0)
        {
            float minDistance = 999;
            for (int i = 0; i < gameObjects.Length; i++)
            {
                if (minDistance == 999) 
                {
                    minDistance = Vector3.Distance(this.transform.position, gameObjects[i].transform.position);
                    lookAt = gameObjects[i];
                }
                else if (Vector3.Distance(this.transform.position, gameObjects[i].transform.position) < minDistance)
                {
                    minDistance = Vector3.Distance(this.transform.position, gameObjects[i].transform.position);
                    lookAt = gameObjects[i];
                }
            }
        }
        if (lookAt != null)
        {
            transform.LookAt(lookAt.transform.position);
            transform.Rotate(new Vector3(-90, 180, 0));
        }
    }
}
