using UnityEngine;

public class Fog : MonoBehaviour
{
    public ParticleSystem particleSystem;
    public void OnPlay()
    {
        particleSystem.Play();
    }
}
