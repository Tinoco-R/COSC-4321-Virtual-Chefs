using UnityEngine;

public class CollisionDetector : MonoBehaviour
{
    private AudioSource audioSource;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (audioSource != null)
        {
            audioSource.Play();
        }
    }
}