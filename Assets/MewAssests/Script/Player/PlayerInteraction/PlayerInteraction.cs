using UnityEngine;

public abstract class PlayerInteraction : MonoBehaviour
{
    protected GameObject player;
    protected PlayerController playerController;
    public IInteractable currentInteractable;
    public string targetTag;
    public bool isInteract;
    protected virtual void Awake()
    {
        player = transform.parent.gameObject;
        playerController = GetComponentInParent<PlayerController>();
    }
    protected abstract void OnTriggerEnter(Collider other);
    protected abstract void OnTriggerStay(Collider other);
    protected abstract void OnTriggerExit(Collider other);
    public abstract void OnInteract();
}
