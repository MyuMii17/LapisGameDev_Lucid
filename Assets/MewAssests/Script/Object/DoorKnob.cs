using System.Collections;
using UnityEngine;

public class DoorKnob : InteractObject
{
    public GameObject parent;
    protected override void Awake()
    {
        base.Awake();
        animator = parent.GetComponent<Animator>();
    }
    protected override IEnumerator OnInteract()
    {
        animator.SetTrigger(animationTrigger);
        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).IsName(animationName));

        EndInteract();

        yield return new WaitUntil(() => animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1f);
        
        if(cutscene.HasCutscene)
        {
            cutsceneManager.PlayerCutScene(cutscene.CutsceneNumber);
        }

        if(isOneTimeAnimaion) yield break;

        interactCorutine = null;
    }
}
