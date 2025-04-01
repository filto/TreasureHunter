using UnityEngine;

public class NodePlacer : MonoBehaviour
{
     [Header("Prefab")]
    public GameObject nodePrefab;  // Prefab som ska placeras

     [Header("Tag för noder")]
    public string nodeTag = "Node";  // Tag för att identifiera noder
    public InteractionUI dragScript;

    private void OnEnable()
    {
        if (dragScript != null)
        {
            dragScript.OnDragEnd += HandleEndDrag; // ✅ Börja lyssna på OnDragEnd
        }
    }
     
    // Placerar en nod-prefab på angiven gridposition
    void PlaceNode(Vector3 touchPosition)
    {
        NodeManager.Instance.PlaceNode(touchPosition); // Skicka direkt till NodeManager
    }
    
    
    void HandleEndDrag(TouchData touchData, Vector3 startPosition, GameObject dragObject)
    {
        
        if (UIWrappers.IsPointerOverUI(touchData.screenPosition, dragObject))
        {
            Debug.Log("Japp på UI)");
            dragObject.transform.position = startPosition;
            return;
        }
        
        PlaceNode(touchData.worldPosition); // Placera noden på gridpositionen 
    }
    
}