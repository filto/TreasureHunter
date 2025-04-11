using UnityEngine;

public class ComponentCreator : MonoBehaviour
{
    public GameObject componentPrefab;
    public InteractionUI dragScript;

    private void OnEnable()
   { 
       if (dragScript != null)
       { 
           dragScript.OnDragEnd += HandleEndDrag; // ✅ Börja lyssna på OnDragEnd
       } 
   }
    
    private void OnDisable()
    {
        if (dragScript != null)
        {
            dragScript.OnDragEnd -= HandleEndDrag; // ❌ Avregistrera
        }
    }
    
    void HandleEndDrag(TouchData touchData, Vector3 startPosition, GameObject dragObject)
    {
        
        if (UIWrappers.IsPointerOverUI(touchData.screenPosition, dragObject))
        {
            return;
        }
        
        Debug.Log(touchData.hitObject.name);
        Instantiate(componentPrefab, touchData.worldPosition, componentPrefab.transform.rotation);

    }
}
