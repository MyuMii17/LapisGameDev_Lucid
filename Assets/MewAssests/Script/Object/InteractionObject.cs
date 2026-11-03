using UnityEngine;

public abstract class InteractionObject : MonoBehaviour, IInteractable
{
    protected GameObject player;
    protected PlayerController playerController;
    public bool getInteract;
    public abstract void GetInteract(GameObject interactor);
}
