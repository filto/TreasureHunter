using UnityEngine;

public class Connector : MonoBehaviour
{
    public Interaction interaction;
    public GameObject Arrow;
    public GameObject GhostConnector;
    private bool isGhost;
    
    private void OnEnable()
    {
        if (interaction != null)
        {
            interaction.OnDragEnd += HandleEndDrag; // ✅ Börja lyssna på OnDragEnd
            interaction.OnClick += HandleClick; // ✅ Börja lyssna på OnDragEnd
        }
        
    }

    public void setConnectorType(bool isGhost)
    {
        if (isGhost == false)
        {
            Arrow.SetActive(true);
            GhostConnector.SetActive(false);
        }
        else
        {
            Arrow.SetActive(false);
            GhostConnector.SetActive(true);
        }
    }
    
    void HandleEndDrag(Vector3 touchPosition, TouchData touchData, Vector3 startPosition, GameObject dragObject)
    {
        dragObject.transform.position = startPosition; //Återställ originalPosition;
        
        if (UIWrappers.DroppedOnTrashCan(touchData.screenPosition))
        {
            Debug.Log("🗑️ Connector släppt på Trashcan, tar bort den.");
            var nodes = NodeManager.Instance.GetNodesFromConnection(gameObject);
            (Node fromNode, Node toNode) = nodes.Value;
            NodeManager.Instance.RemoveConnection(fromNode,toNode);
            NodeManager.Instance.AddConnection(toNode, fromNode, true);
            return;
        }
    }

    void HandleClick(GameObject dragObject)
    {
        var nodes = NodeManager.Instance.GetNodesFromConnection(gameObject);
        (Node fromNode, Node toNode) = nodes.Value;
        NodeManager.Instance.RemoveConnection(fromNode,toNode);
        NodeManager.Instance.AddConnection(toNode, fromNode);
        
    }
}
