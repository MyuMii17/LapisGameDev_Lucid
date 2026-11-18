using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class FallDamage : MonoBehaviour
{
    [Header("Fall Damage")]
    [SerializeField, Min(0f)] private float safeFallHeight = 4f;
    [SerializeField] private string groundTag = "Ground";

    [Header("Ground Check")]
    [SerializeField, Min(0.01f)] private float groundCheckRadius = 0.2f;
    [SerializeField] private Transform groundCheck;

    private Rigidbody rb;

    private bool wasGrounded;
    private bool isFalling;
    private float fallStartHeight;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        bool grounded = CheckGrounded();

        if (wasGrounded && !grounded && rb.linearVelocity.y <= 0.1f)
        {
            isFalling = true;
            fallStartHeight = transform.position.y;
        }

        // Record the height when a fall actually begins.
        if (!grounded && !isFalling && rb.linearVelocity.y < -0.1f)
        {
            isFalling = true;
            fallStartHeight = transform.position.y;
        }

        wasGrounded = grounded;
    }

    private bool CheckGrounded()
    {
        if (groundCheck == null)
            return false;

        Collider[] colliders = Physics.OverlapSphere(
            groundCheck.position,
            groundCheckRadius,
            ~0,
            QueryTriggerInteraction.Ignore
        );

        foreach (Collider col in colliders)
        {
            if (col.CompareTag(groundTag))
                return true;
        }

        return false;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!isFalling)
            return;

        // Only apply fall damage when landing on an object
        // with the Ground tag.
        if (!collision.gameObject.CompareTag(groundTag))
            return;

        // Only count collisions with upward-facing surfaces.
        foreach (ContactPoint contact in collision.contacts)
        {
            if (contact.normal.y < 0.5f)
                continue;

            float fallHeight = fallStartHeight - transform.position.y;

            if (fallHeight > safeFallHeight)
            {
                Debug.Log("Player died!");
            }

            isFalling = false;
            return;
        }
    }

    private void OnDrawGizmosSelected()
    {
        if (groundCheck == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
