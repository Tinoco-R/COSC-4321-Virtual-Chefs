using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeGet : MonoBehaviour
{
    public float volume;
    public SliceObject slice;
    // Start is called before the first frame update
    void Start()
    {
        volume = slice.CalculateVolume(this.gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        volume = slice.CalculateVolume(this.gameObject);
    }
}
