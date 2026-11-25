using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Volume : MonoBehaviour
{
    public Slider audioSlider;
    public List<AudioSource> audioSources = new List<AudioSource>();
    void Awake()
    {
        audioSlider.onValueChanged.AddListener(VolumeChange);
        foreach(AudioSource audioSource in audioSources)
        {
            audioSource.volume = 0.1f;
        }
    }

    public void VolumeChange(float amount)
    {
        foreach(AudioSource audioSource in audioSources)
        {
            audioSource.volume = amount;
        }
    }
}
