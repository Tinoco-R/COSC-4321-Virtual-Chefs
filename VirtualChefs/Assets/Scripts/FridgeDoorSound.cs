using UnityEngine;

public class FridgeDoorSound : MonoBehaviour
{
    [SerializeField] private AudioSource openSound;
    [SerializeField] private AudioSource closeSound;
    private bool isDoorOpen = false;

    private void Update()
    {
        float currentAngle = transform.localRotation.eulerAngles.y;

        if (currentAngle > 10f && !isDoorOpen)
        {
            PlayOpenSound();
            isDoorOpen = true;
        }
        else if (currentAngle <= 10f && isDoorOpen)
        {
            PlayCloseSound();
            isDoorOpen = false;
        }
    }

    private void PlayOpenSound()
    {
        if (openSound != null)
        {
            openSound.Play();
        }
    }

    private void PlayCloseSound()
    {
        if (closeSound != null)
        {
            closeSound.Play();
        }
    }
}