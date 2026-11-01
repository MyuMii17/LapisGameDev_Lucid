using UnityEngine;

public class PlayerPushAndPull : MonoBehaviour
{
    private PlayerController playerController; 
    public GameObject target;
    public bool isInteract;
    public IInteractable currentInteractable;   
    void Awake()
    {
        playerController = GetComponentInParent<PlayerController>();
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
            target = other.gameObject;
            currentInteractable = interactable;
        } 
    }
    void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Box") && currentInteractable != null)
        {
            IInteractable interactable = other.GetComponentInParent<IInteractable>();

            if (interactable == null) return;
            target = other.gameObject;
            currentInteractable = interactable;
        } 
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Box") && currentInteractable != null && !isInteract)
        {
            target = null;
            currentInteractable = null;
        } 
    }
    public void OnInteract()
    {
        currentInteractable.OnInteract(gameObject);
    }
}
