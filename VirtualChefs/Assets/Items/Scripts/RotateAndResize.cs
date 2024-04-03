using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateAndResize : MonoBehaviour
{
    // Start is called before the first frame update
    public float turn = 2;
    public float A = 20;
    // Update is called once per frame
    void FixedUpdate()
    {
        transform.Rotate(new Vector3(0, 0, turn));
        transform.localScale -= (new Vector3((Mathf.Sin(Time.time / 3) / A),(Mathf.Sin(Time.time / 3) / A), 0));
    }
}
