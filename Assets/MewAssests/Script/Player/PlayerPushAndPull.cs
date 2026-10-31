using UnityEngine;

public class PlayerPushAndPull : MonoBehaviour
{
    [SerializeField]private PlayerController playerController; 
    public bool isInteract;
    public IInteractable currentInteractable;   
    void Awake()
    {
        
    }
    void Update()
    {

    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Box") && currentInteractable == null)
        {
            IInteractable interactable = other.GetComponentInParent<IInteractable>();

            if (interactable == null) return;

            currentInteractable = interactable;
        } 
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Box") && currentInteractable != null)
        {
            IInteractable interactable = other.GetComponentInParent<IInteractable>();

            if (interactable == null) return;

            currentInteractable = interactable;
        } 
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box") && currentInteractable != null && !isInteract)
        {
            currentInteractable = null;
        } 
    }
    public void OnInteract()
    {
        currentInteractable.OnInteract(gameObject);
    }

    // void OnDrawGizmosSelected()
    // {
    //     Gizmos.DrawWireSphere(interactPosition, sphereSize);
    // }
}
