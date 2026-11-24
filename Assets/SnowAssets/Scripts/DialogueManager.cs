using System;
using UnityEngine;

public class DialogueManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private DialogueUI dialogueUI;

    private DialogueAsset currentDialogue;
    private DialogueNodeData currentNode;

    public bool IsDialogueActive { get; private set; }

    public event Action<string> RepeatNodeChangeRequested;
    public event Action DialogueEnded;

    private void Awake()
    {
        dialogueUI.CloseRequested += EndDialogue;
    }

    private void OnDestroy()
    {
        if (dialogueUI != null)
            dialogueUI.CloseRequested -= EndDialogue;
    }

    public bool StartDialogue(
        DialogueAsset dialogue,
        string startNodeId = null)
    {
        if (IsDialogueActive || dialogue == null)
            return false;

        dialogue.Initialize();

        DialogueNodeData rootNode =
            string.IsNullOrEmpty(startNodeId)
                ? dialogue.GetRootNode()
                : dialogue.GetNode(startNodeId);

        if (rootNode == null)
        {
            Debug.LogError(
                $"Starting dialogue node '{startNodeId}' could not be found in '{dialogue.name}'.",
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
            SelectChoice,
            ContinueDialogue
        );
    }

    private void SelectChoice(DialogueChoiceData choice)
    {
        if (choice == null)
            return;

        // Update the NPC's repeat node only when requested.
        if (choice.updateRepeatNode)
        {
            if (string.IsNullOrWhiteSpace(choice.repeatNodeId))
            {
                Debug.LogWarning(
                    $"Choice '{choice.choiceText}' has repeat node updating enabled, but no repeat node ID was assigned.",
                    currentDialogue
                );
            }
            else
            {
                RepeatNodeChangeRequested?.Invoke(
                    choice.repeatNodeId
                );
            }
        }

        NavigateToNode(choice.nextNodeId);
    }

    private void ContinueDialogue(string nextNodeId)
    {
        NavigateToNode(nextNodeId);
    }

    private void NavigateToNode(string nextNodeId)
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

        DialogueEnded?.Invoke();
    }
}
