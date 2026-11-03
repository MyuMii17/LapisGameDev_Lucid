using UnityEngine;

public class BoxDetectionGround : MonoBehaviour
{
    private PushObject pushObject;
    void Awake()
    {
        pushObject = gameObject.GetComponentInParent<PushObject>();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            pushObject.isGrounded = true;
            pushObject.isReset = false;
        }
    }
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Ground"))
        {
            pushObject.isGrounded = false;
            pushObject.getInteract = false;
        }
    }
}
