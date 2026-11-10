using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueNodeData
{
    [Tooltip("Unique identifier for this node.")]
    public string nodeId;

    [TextArea(3, 6)]
    public string dialogueText;

    public List<DialogueChoiceData> choices =
        new List<DialogueChoiceData>();
}
