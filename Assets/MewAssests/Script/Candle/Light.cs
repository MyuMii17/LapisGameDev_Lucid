using UnityEngine;

public class Light : MonoBehaviour
{
    private Animator animator;
    public float speedMultipiler;
    void Awake()
    {
        animator = GetComponent<Animator>();
        speedMultipiler = Random.Range(1.0f, 1.75f);
        animator.SetFloat("Speed", speedMultipiler);
    }
}
