using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "NodeDatabase", menuName = "Nodes/Node Database")]
public class NodeDatabase : ScriptableObject
{
    public List<NodeData> nodes = new List<NodeData>();
}
