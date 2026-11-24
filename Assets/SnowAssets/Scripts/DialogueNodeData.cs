using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueNodeData
{
    [Tooltip("Unique identifier for this node.")]
    public string nodeId;

    [Header("Speaker")]
    public string speakerName;

    [Header("Dialogue")]
    [TextArea(3, 6)]
    public string dialogueText;

    [Header("Choices")]
    public List<DialogueChoiceData> choices =
        new List<DialogueChoiceData>();

    [Header("Continue")]
    [Tooltip("Next node ID when no choices are required.")]
    public string nextNodeId;
}
