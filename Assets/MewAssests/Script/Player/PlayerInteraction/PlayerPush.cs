using UnityEngine;

public class PlayerPush : PlayerInteraction
{
    public PushObject target;
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(targetTag) && currentInteractable == null)
        {
            IInteractable interactable = other.GetComponentInParent<IInteractable>();
            target = other.GetComponentInParent<PushObject>();
            if (interactable == null) return;

            currentInteractable = interactable;
        }
    }
    protected override void OnTriggerStay(Collider other)
    {
        if (other.CompareTag(targetTag) && currentInteractable != null)
        {
            IInteractable interactable = other.GetComponentInParent<IInteractable>();
            target = other.GetComponentInParent<PushObject>();

            if (interactable == null) return;

            currentInteractable = interactable;
        } 
    }
    protected override void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(targetTag) && currentInteractable != null)
        {
            if(playerController.push.IsHold)
            {
                playerController.push.IsHold = false;
                OnInteract();
            }
            currentInteractable = null;
            target = null;
        } 
    }

    public override void OnInteract()
    {
        playerController.ResetVelocity();
        currentInteractable.GetInteract(player);
    }
}
