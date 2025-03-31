using UnityEngine;

[CreateAssetMenu(fileName = "NewNodeData", menuName = "Nodes/Node Data")]
public class NodeData : ScriptableObject
{
    public string nodeType;
    public Sprite icon;
    public Color color;
}
