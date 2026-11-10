using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Transform choiceContainer;
    [SerializeField] private Button choiceButtonPrefab;
    [SerializeField] private Button closeButton;

    private readonly List<Button> activeButtons = new();

    public event Action CloseRequested;

    private void Awake()
    {
        closeButton.onClick.AddListener(
            () => CloseRequested?.Invoke()
        );

        HideDialogue();
    }

    public void ShowNode(
        DialogueNodeData node,
        Action<string> onChoiceSelected)
    {
        if (node == null)
            return;

        dialoguePanel.SetActive(true);

        dialogueText.text = node.dialogueText;

        ClearChoices();

        bool hasChoices = node.choices != null &&
                          node.choices.Count > 0;

        closeButton.gameObject.SetActive(!hasChoices);

        if (!hasChoices)
            return;

        foreach (DialogueChoiceData choice in node.choices)
        {
            CreateChoiceButton(choice, onChoiceSelected);
        }
    }

    private void CreateChoiceButton(
        DialogueChoiceData choice,
        Action<string> onChoiceSelected)
    {
        Button button = Instantiate(
            choiceButtonPrefab,
            choiceContainer
        );

        TMP_Text buttonText =
            button.GetComponentInChildren<TMP_Text>();

        if (buttonText != null)
            buttonText.text = choice.choiceText;

        string targetNodeId = choice.nextNodeId;

        button.onClick.AddListener(() =>
        {
            onChoiceSelected?.Invoke(targetNodeId);
        });

        activeButtons.Add(button);
    }

    private void ClearChoices()
    {
        foreach (Button button in activeButtons)
        {
            if (button != null)
                Destroy(button.gameObject);
        }

        activeButtons.Clear();
    }

    public void HideDialogue()
    {
        ClearChoices();

        closeButton.gameObject.SetActive(false);
        dialoguePanel.SetActive(false);
    }
}
