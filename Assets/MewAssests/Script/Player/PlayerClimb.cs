using UnityEngine;

public class PlayerClimb : MonoBehaviour
{
    private PlayerController playerController; 
    public GameObject target;
    public bool isInteract;
    public IInteractable currentInteractable;
    void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
    }
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Edge") && currentInteractable == null)
        {
            IInteractable interactable = other.GetComponentInParent<IInteractable>();

            if (interactable == null) return;

            target = other.gameObject;
            currentInteractable = interactable;
        } 
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Edge") && currentInteractable != null)
        {
            IInteractable interactable = other.GetComponentInParent<IInteractable>();

            if (interactable == null) return;

            target = other.gameObject;
            currentInteractable = interactable;
        } 
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Edge") && currentInteractable != null)
        {
            if (playerController.climb.IsHold)
            {
                playerController.climb.IsHold = false;
                playerController.ResetVelocity();
                OnInteract();
            }
            target = null;
            currentInteractable = null;
        } 
    }

    public void OnInteract()
    {
        if(playerController.move.IsJumping) playerController.move.IsJumping = false;
        currentInteractable.OnInteract(gameObject);
    }
}
