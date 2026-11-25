using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{
    public List<AudioClip> clips = new List<AudioClip>();
    public List<AudioSource> sources = new List<AudioSource>();
    public bool isLoopOnAwake;
    public float rangeloop;
    void Awake()
    {
        if (isLoopOnAwake)
        {
            if(sources == null) return;
            for(int i = 0; i <= rangeloop; i++)
            {
                sources[i].Play();
            }
        }   
    }
    public void OnPlayClip(int sourceIndex)
    {
        sources[sourceIndex].PlayOneShot(clips[sourceIndex]);
    }
    public void OnPlay(int sourceIndex)
    {
        sources[sourceIndex].Play();
    }
    public void OnStop(int sourceIndex)
    {
        sources[sourceIndex].Stop();
    }
}
