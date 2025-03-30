using UnityEngine;

public class NodePlacer : MonoBehaviour
{
     [Header("Prefab")]
    public GameObject nodePrefab;  // Prefab som ska placeras

     [Header("Tag för noder")]
    public string nodeTag = "Node";  // Tag för att identifiera noder
    public UIDraggableReset dragScript;

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
    
    
    void HandleEndDrag(Vector3 touchPosition, GameObject hitObject, Vector3 startPosition, GameObject dragObject)
    {
        
        if (hitObject ==  GameManager.Instance.trashCan)
        {
            return;  // ⛔ AVSLUTA HÄR! Vi behöver inte kolla något mer.
        }
        
        PlaceNode(touchPosition); // Placera noden på gridpositionen 
        

    }
    
}