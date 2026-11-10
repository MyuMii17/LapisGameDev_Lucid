using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialogueUI dialogueUI;

    private DialogueAsset currentDialogue;
    private DialogueNodeData currentNode;

    public bool IsDialogueActive { get; private set; }

    private void Awake()
    {
        dialogueUI.CloseRequested += EndDialogue;
    }

    private void OnDestroy()
    {
        if (dialogueUI != null)
            dialogueUI.CloseRequested -= EndDialogue;
    }

    public bool StartDialogue(DialogueAsset dialogue)
    {
        if (IsDialogueActive || dialogue == null)
            return false;

        dialogue.Initialize();

        DialogueNodeData rootNode = dialogue.GetRootNode();

        if (rootNode == null)
        {
            Debug.LogError(
                $"Dialogue '{dialogue.name}' has no valid root node.",
                dialogue
            );

            return false;
        }

        currentDialogue = dialogue;
        currentNode = rootNode;

        IsDialogueActive = true;

        ShowCurrentNode();

        return true;
    }

    private void ShowCurrentNode()
    {
        if (currentNode == null)
        {
            Debug.LogError("Current dialogue node is missing.");
            EndDialogue();
            return;
        }

        dialogueUI.ShowNode(
            currentNode,
            SelectChoice
        );
    }

    private void SelectChoice(string nextNodeId)
    {
        if (!IsDialogueActive || currentDialogue == null)
            return;

        DialogueNodeData nextNode =
            currentDialogue.GetNode(nextNodeId);

        if (nextNode == null)
        {
            Debug.LogError(
                $"Dialogue node '{nextNodeId}' could not be found.",
                currentDialogue
            );

            EndDialogue();
            return;
        }

        currentNode = nextNode;

        ShowCurrentNode();
    }

    public void EndDialogue()
    {
        if (!IsDialogueActive)
            return;

        IsDialogueActive = false;

        currentDialogue = null;
        currentNode = null;

        dialogueUI.HideDialogue();
    }
}
