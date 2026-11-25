using UnityEngine;

public class CeilingLight : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip lightCrack;
    void Awake()
    {
        audioSource = GetComponentInChildren<AudioSource>();
    }
    public void OnPlaySound()
    {
        audioSource.PlayOneShot(lightCrack);
    }
}
