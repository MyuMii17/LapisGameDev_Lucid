using UnityEngine;

public class NPC : InteractionObject
{
    [Header("Dialogue")]
    [SerializeField] private DialogueManager dialogueManager;
    [SerializeField] private DialogueAsset dialogue;

    [Header("Conversation Progress")]
    [Tooltip("Default starting node after the first conversation.")]
    [SerializeField] private string repeatStartNodeId = "ask_help";

    [Header("Settings")]
    [SerializeField] private bool canTalk = true;

    protected PlayerInteract playerInteract;

    // Runtime conversation progress.
    private bool hasTalkedBefore;
    private string currentRepeatNodeId;

    public override void GetInteract(GameObject interactor)
    {
        player = interactor;
        playerController = interactor.GetComponentInChildren<PlayerController>();
        playerInteract = interactor.GetComponentInChildren<PlayerInteract>();
        getInteract = playerController.interact.IsHold;

        playerInteract.isInteract = getInteract;

        if (!canTalk)
            return;

        if (dialogueManager == null || dialogue == null)
        {
            Debug.LogWarning(
                $"{name} is missing a dialogue reference.",
                this
            );

            return;
        }

        // Choose the starting node.
        string startNodeId = hasTalkedBefore
            ? currentRepeatNodeId
            : null;

        // Listen for repeat-node changes.
        dialogueManager.RepeatNodeChangeRequested -= SaveRepeatNode;
        dialogueManager.RepeatNodeChangeRequested += SaveRepeatNode;

        dialogueManager.DialogueEnded -= OnDialogueEnded;
        dialogueManager.DialogueEnded += OnDialogueEnded;

        bool started = dialogueManager.StartDialogue(
            dialogue,
            startNodeId
        );

        if (started)
        {
            hasTalkedBefore = true;

            if (string.IsNullOrEmpty(currentRepeatNodeId))
                currentRepeatNodeId = repeatStartNodeId;
        }
        else
        {
            UnsubscribeFromDialogueEvents();
        }
    }

    private void SaveRepeatNode(string nodeId)
    {
        if (string.IsNullOrWhiteSpace(nodeId))
            return;

        currentRepeatNodeId = nodeId;

        Debug.Log(
            $"{name}: Repeat dialogue updated to {nodeId}"
        );
    }

    private void OnDialogueEnded()
    {
        UnsubscribeFromDialogueEvents();
    }

    private void UnsubscribeFromDialogueEvents()
    {
        if (dialogueManager == null)
            return;

        dialogueManager.RepeatNodeChangeRequested -= SaveRepeatNode;
        dialogueManager.DialogueEnded -= OnDialogueEnded;
    }

    public void SetCanTalk(bool value)
    {
        canTalk = value;
    }

    public void ResetConversationProgress()
    {
        hasTalkedBefore = false;
        currentRepeatNodeId = null;

        Debug.Log($"{name}: Conversation progress reset.");
    }

    private void OnDestroy()
    {
        UnsubscribeFromDialogueEvents();
    }
}
