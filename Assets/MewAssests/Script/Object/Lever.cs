using System.Collections;
using UnityEngine;

public class Lever : InteractObject
{
    public override void Condition()
    {
        base.Condition();
    }

    protected override IEnumerator OnInteract()
    {
        animator.SetTrigger(animationTrigger);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(animationName));

        EndInteract();

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        
        if(isOneTimeAnimaion) yield break;

        interactCorutine = null;

    }

}
