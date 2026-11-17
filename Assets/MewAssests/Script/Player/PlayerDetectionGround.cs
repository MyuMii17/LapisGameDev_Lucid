using UnityEngine;

public class PlayerDetectionGround : MonoBehaviour
{
    private PlayerController player;
    private Animator animator;
    void Awake()
    {
        player = gameObject.GetComponentInParent<PlayerController>();
        animator = gameObject.GetComponentInParent<Animator>();
    }

    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            player.move.IsGrounded = true;
            player.move.IsJumping = false;
            player.move.IsWalljump = false;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            player.move.IsGrounded = false;
        }
    }
}
