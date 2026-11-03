using UnityEngine;

public class PlayerClimb : PlayerInteraction
{
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && currentInteractable == null)
        {
            IInteractable interactable = other.GetComponentInParent<IInteractable>();

            if (interactable == null) return;

            currentInteractable = interactable;
        }
    }
    protected override void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(targetTag) && currentInteractable != null)
        {
            IInteractable interactable = other.GetComponentInParent<IInteractable>();

            if (interactable == null) return;

            currentInteractable = interactable;
        } 
    }
    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag) && currentInteractable != null)
        {
            if (playerController.climb.IsHold)
            {
                playerController.climb.IsHold = false;
                playerController.ResetVelocity();
                OnInteract();
            }

            currentInteractable = null;
        } 
    }

    public override void OnInteract()
    {
        if(playerController.move.IsJumping) playerController.move.IsJumping = false;
        currentInteractable.GetInteract(player);
    }
}
