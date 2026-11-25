using UnityEngine;

public class FogTrigger : MonoBehaviour
{
    public GameObject fog;
    public Animator animator;
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            fog.SetActive(true);
            animator.SetBool("Start", true);
        }
    }
}
