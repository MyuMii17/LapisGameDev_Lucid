using System.Collections;
using UnityEngine;

public class Holder : InteractObject
{
    private PushObject pushObject;
    public GameObject diamond;
    public GameObject ruby;
    protected override void Awake()
    {
        base.Awake();
        GameObject parent = transform.parent.gameObject;
        pushObject = parent.GetComponentInChildren<PushObject>();
    }
    protected override IEnumerator OnInteract()
    {
        if (playerInteract.diamond)
        {
            diamond.SetActive(true);
        }
        if (playerInteract.ruby)
        {
            ruby.SetActive(true);
        }

        if(playerInteract.ruby && playerInteract.diamond)
        {
            canInteract = false;
            animator.SetTrigger(animationTrigger);
            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(animationName));

            EndInteract();

            yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
            pushObject.canPush = true;
            
            if(isOneTimeAnimaion)
            {
                animator.enabled = false;
                yield break;
            }
            
            interactCorutine = null;
        }
    }
}
