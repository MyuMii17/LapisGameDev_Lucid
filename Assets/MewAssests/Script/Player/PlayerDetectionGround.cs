using UnityEngine;

public class PlayerDetectionGround : MonoBehaviour
{
    private PlayerController player;
    void Awake()
    {
        player = gameObject.GetComponentInParent<PlayerController>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            player.move.IsGrounded = true;
            player.move.IsJumping = false;
            player.move.CanJump = true;
        }
    }
}
