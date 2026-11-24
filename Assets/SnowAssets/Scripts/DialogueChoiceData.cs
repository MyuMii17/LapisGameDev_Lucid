using System;
using UnityEngine;

[Serializable]
public class DialogueChoiceData
{
    [TextArea(1, 3)]
    public string choiceText;

    [Tooltip("ID of the node this choice leads to.")]
    public string nextNodeId;

    [Header("Conversation Progress")]
    [Tooltip("Enable this to change the NPC's starting node for the next conversation.")]
    public bool updateRepeatNode;

    [Tooltip("Node ID to start from next time the player talks to this NPC.")]
    public string repeatNodeId;
}
