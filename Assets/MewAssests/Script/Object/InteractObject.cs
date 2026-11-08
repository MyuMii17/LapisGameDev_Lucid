using System.Collections;
using UnityEngine;

public class InteractObject : InteractionObject
{
    private PlayerInteract playerInteract;
    void Awake()
    {
        
    }
    public virtual IEnumerator OnInteract()
    {
        yield return null;
    }
    public override void GetInteract(GameObject interactor)
    {
        player = interactor;
        playerController = interactor.GetComponentInChildren<PlayerController>();
        playerInteract = interactor.GetComponentInChildren<PlayerInteract>();

        getInteract = playerController.interact.IsHold;
        playerInteract.isInteract = getInteract;
    }
}
