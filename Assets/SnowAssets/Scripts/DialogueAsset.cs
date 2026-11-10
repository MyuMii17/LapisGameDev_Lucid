using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "New Dialogue",
    menuName = "Dialogue/Dialogue Asset"
)]
public class DialogueAsset : ScriptableObject
{
    [Header("Dialogue Settings")]
    public string rootNodeId;

    [Header("Dialogue Nodes")]
    public List<DialogueNodeData> nodes =
        new List<DialogueNodeData>();

    private Dictionary<string, DialogueNodeData> nodeLookup;

    public void Initialize()
    {
        nodeLookup = new Dictionary<string, DialogueNodeData>();

        foreach (DialogueNodeData node in nodes)
        {
            if (node == null || string.IsNullOrWhiteSpace(node.nodeId))
            {
                Debug.LogWarning(
                    $"Dialogue '{name}' contains a node with an empty ID.",
                    this
                );

                continue;
            }

            if (nodeLookup.ContainsKey(node.nodeId))
            {
                Debug.LogError(
                    $"Duplicate dialogue node ID: {node.nodeId}",
                    this
                );

                continue;
            }

            nodeLookup.Add(node.nodeId, node);
        }
    }

    public DialogueNodeData GetNode(string nodeId)
    {
        if (nodeLookup == null)
            Initialize();

        if (string.IsNullOrEmpty(nodeId))
            return null;

        nodeLookup.TryGetValue(nodeId, out DialogueNodeData node);

        return node;
    }

    public DialogueNodeData GetRootNode()
    {
        return GetNode(rootNodeId);
    }
}
