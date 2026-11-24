using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TextMeshProUGUI speakerNameText;
    [SerializeField] private TextMeshProUGUI dialogueText;
    [SerializeField] private Transform choiceContainer;
    [SerializeField] private Button choiceButtonPrefab;
    [SerializeField] private Button nextButton;
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
        Action<DialogueChoiceData> onChoiceSelected,
        Action<string> onContinue)
    {
        if (node == null)
            return;

        dialoguePanel.SetActive(true);

        speakerNameText.text = node.speakerName;
        dialogueText.text = node.dialogueText;

        ClearChoices();

        nextButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);

        if (node.choices != null && node.choices.Count > 0)
        {
            foreach (DialogueChoiceData choice in node.choices)
            {
                CreateChoiceButton(choice, onChoiceSelected);
            }
        }
        else if (!string.IsNullOrEmpty(node.nextNodeId))
        {
            nextButton.gameObject.SetActive(true);

            nextButton.onClick.RemoveAllListeners();

            string targetNodeId = node.nextNodeId;

            nextButton.onClick.AddListener(() =>
            {
                onContinue?.Invoke(targetNodeId);
            });
        }
        else
        {
            closeButton.gameObject.SetActive(true);
        }
    }

    private void CreateChoiceButton(
        DialogueChoiceData choice,
        Action<DialogueChoiceData> onChoiceSelected)
    {
        Button button = Instantiate(
            choiceButtonPrefab,
            choiceContainer
        );

        TMP_Text buttonText =
            button.GetComponentInChildren<TMP_Text>();

        if (buttonText != null)
            buttonText.text = choice.choiceText;

        button.onClick.AddListener(() =>
        {
            onChoiceSelected?.Invoke(choice);
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

        nextButton.gameObject.SetActive(false);
        closeButton.gameObject.SetActive(false);

        dialoguePanel.SetActive(false);
    }
}
