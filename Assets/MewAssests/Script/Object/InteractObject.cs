using System.Collections;
using UnityEngine;

public class InteractObject : InteractionObject
{
    protected PlayerInteract playerInteract;
    protected Animator animator;
    protected Coroutine interactCorutine;
    public string animationName;
    public string animationTrigger;
    public bool isOneTimeAnimaion;
    public bool canInteract;
    protected virtual void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }
    protected virtual IEnumerator OnInteract()
    {
        yield return null;
    }
    protected void EndInteract()
    {
        playerInteract.isInteract = false;

        playerInteract = null;
        playerController = null;
    } 
    public override void GetInteract(GameObject interactor)
    {
        if(interactCorutine != null) return;
        player = interactor;
        playerController = interactor.GetComponentInChildren<PlayerController>();
        playerInteract = interactor.GetComponentInChildren<PlayerInteract>();

        getInteract = !getInteract;
        playerInteract.isInteract = getInteract;

        if(getInteract == false) return;
        interactCorutine = StartCoroutine(OnInteract());
    }
}
