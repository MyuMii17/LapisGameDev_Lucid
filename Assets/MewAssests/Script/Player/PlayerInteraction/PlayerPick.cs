using UnityEngine;

public class PlayerPick : PlayerInteraction
{
    public Transform handPosition;
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
            currentInteractable = null;
        } 
    }

    public override void OnInteract()
    {
        currentInteractable.GetInteract(player);
    }
}
