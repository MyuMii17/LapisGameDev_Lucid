using UnityEngine;

public class NPC : InteractionObject
{
    [Header("Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DialogueAsset dialogue;

    [Header("Settings")]
    [SerializeField] private bool canTalk = true;
    protected PlayerInteract playerInteract;

    public override void GetInteract(GameObject interactor)
    {

        player = interactor;
        playerController = interactor.GetComponentInChildren<PlayerController>();
        playerInteract = interactor.GetComponentInChildren<PlayerInteract>();
        getInteract = playerController.interact.IsHold;

        playerInteract.isInteract = getInteract;

        if (canTalk)
        {
            if (dialogueManager == null || dialogue == null)
            {
                Debug.LogWarning(
                    $"{name} is missing a dialogue reference.",
                    this
                );
            }
            else
            {
                dialogueManager.StartDialogue(dialogue);
            }
        }
    }

    public void SetCanTalk(bool value)
    {
        canTalk = value;
    }
}
