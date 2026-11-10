using System;
using UnityEngine;

[Serializable]
public class DialogueChoiceData
{
    [TextArea(1, 3)]
    public string choiceText;

    [Tooltip("ID of the node this choice leads to.")]
    public string nextNodeId;
}
