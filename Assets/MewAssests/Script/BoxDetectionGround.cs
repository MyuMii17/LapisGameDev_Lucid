using UnityEngine;

public class BoxDetectionGround : MonoBehaviour
{
    private Box box;
    void Awake()
    {
        box = gameObject.GetComponentInParent<Box>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            box.isGrounded = true;
            box.isReset = false;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            box.isGrounded = false;
            box.getInteract = false;
        }
    }
}
