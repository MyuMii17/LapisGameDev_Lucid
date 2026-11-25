using UnityEngine;
using UnityEngine.Playables;

public class FogTrigger : MonoBehaviour
{
    public GameObject fog;
    public PlayableDirector fogScene;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fog.SetActive(true);
            fogScene.Play();
        }
    }

}
