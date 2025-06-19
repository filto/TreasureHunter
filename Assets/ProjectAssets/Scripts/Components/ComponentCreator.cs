using UnityEngine;

public class ComponentCreator : MonoBehaviour
{
    public GameObject componentPrefab;
    public InteractionUI dragScript;
    public float objectScale = 1.0f;

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
        GameObject newObj = Instantiate(componentPrefab, touchData.worldPosition, componentPrefab.transform.rotation);
        Vector3 instanceScale = new Vector3(objectScale, objectScale, objectScale);
        newObj.transform.localScale = instanceScale;
        newObj.transform.SetParent(GameManager.Instance.nodeContainer.transform, worldPositionStays: true);

        var modeSwitcher = newObj.GetComponent<ModeSwitcher>();
        if (modeSwitcher != null)
        {
            modeSwitcher.SetGameMode(SwitchEditorGame.CurrentModeIsGame); 
        }
        

    }
}
