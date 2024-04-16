using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CustomerHead : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        float random1 = Random.Range(2, 11);
        float random2 = Random.Range(2, 11);
        float random3 = Random.Range(2, 11);
        Color CustomerColor = new Color((float)(random1 / 10), (float)(random2 / 10), (float)(random3 / 10), 1);
        this.GetComponent<Renderer>().material.color = CustomerColor;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void makeNewColor()
    {
        float random1 = Random.Range(2, 11);
        float random2 = Random.Range(2, 11);
        float random3 = Random.Range(2, 11);
        Color CustomerColor = new Color((float)(random1 / 10), (float)(random2 / 10), (float)(random3 / 10), 1);
        this.GetComponent<Renderer>().material.color = CustomerColor;
    }
}
