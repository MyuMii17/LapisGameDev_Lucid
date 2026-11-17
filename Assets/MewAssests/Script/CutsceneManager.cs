using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneManager : MonoBehaviour
{
    public List<PlayableDirector> directors; 
    private static CutsceneManager Instance = null;
    public static CutsceneManager GetInstance()
    {
        return Instance;
    }
    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    public void PlayerCutScene(int index)
    {
        directors[index].Play();
    }
}
