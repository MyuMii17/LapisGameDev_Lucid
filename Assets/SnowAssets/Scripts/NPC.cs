using UnityEngine;

public class NPC : InteractionObject
{
    [Header("Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DialogueAsset dialogue;

    [Header("Settings")]
    [SerializeField] private bool canTalk = true;
    protected PlayerPick playerPick;

    public override void GetInteract(GameObject interactor)
    {

        player = interactor;
        playerController = interactor.GetComponentInChildren<PlayerController>();
        playerPick = interactor.GetComponentInChildren<PlayerPick>();
        getInteract = playerController.pick.IsHold;

        playerPick.isInteract = getInteract;

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
