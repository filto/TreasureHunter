using UnityEngine;
using System; // För Action (delegates)

public class Interaction : MonoBehaviour
{
    public event Action<Vector3, GameObject, Vector3, GameObject> OnDragEnd; // 🔥 Bara för detta objekt
    public event Action<GameObject> OnClick;
    public Vector3 midPointOffset;
    
    private Vector3 startPosition; // Store start position
    private float clickThreshold = 0.1f;
    public GameObject interactionObject;
    private Vector3 touchStartPosition;
    public bool isDraggable = true;
    public bool isUIAsset = false;
    private Vector3 worldPosition;
    private Camera uiCamera;
    private Camera dragCamera;
    

    public void OnTouchEvent(TouchData touchData)
    {
        //Debug.Log($"📌 {gameObject.name} fick touch-event: {touchData.phase} vid {touchData.position}");
        
        switch (touchData.phase)
        {
            case TouchPhase.Began:
               // Debug.Log("Nu börjar jag");
                touchStartPosition = new Vector3 (touchData.position.x, touchData.position.y, interactionObject.transform.position.z);
                startPosition = interactionObject.transform.position;
                break;

            case TouchPhase.Moved:
                //Debug.Log("Nu drar jag");
                if (isDraggable)
                {
                interactionObject.transform.position =new Vector3 (touchData.position.x, touchData.position.y, interactionObject.transform.position.z) + midPointOffset;
                }
                
                break;

            case TouchPhase.Ended:
                //Debug.Log("Nu slutar jag");
                Vector3 touchEndPosition = new Vector3 (touchData.position.x, touchData.position.y, interactionObject.transform.position.z);
                float dragDistance = Vector3.Distance(touchStartPosition, touchEndPosition);
                
                if (dragDistance <= clickThreshold)
                {
                    OnClick?.Invoke(gameObject);
                    interactionObject.transform.position = startPosition;
                    return;
                }

                if (isDraggable)
                {
                    OnDragEnd?.Invoke(touchEndPosition, touchData.droppedObject, startPosition, interactionObject);
                }

                break;
        }
    }
}