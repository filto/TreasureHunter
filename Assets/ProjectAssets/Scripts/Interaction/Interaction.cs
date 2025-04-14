using UnityEngine;
using System; // För Action (delegates)

public class Interaction : MonoBehaviour
{
    public event Action<Vector3, TouchData, Vector3, GameObject> OnDragEnd; // 🔥 Bara för detta objekt
    public event Action<GameObject> OnClick;
    public event Action<GameObject> OnLongPress;
    public Vector3 midPointOffset;
    private Vector3 startPosition; // Store start position
    private float clickThreshold = 0.1f;
    public GameObject interactionObject;
    private Vector3 touchStartPosition;
    private Vector3 touchEndPosition;
    public bool isDraggable = true;
    private bool wasClickedInBeganPhase=false;
    public bool is2DMovement= false;
    private Vector3 worldPosition;
    private Camera uiCamera;
    private Camera dragCamera;
    private Collider objectCollider;
    public float longPressDuration = 0.5f;
    private float pressTime = 0f;
    private bool longPressTriggered = false;
    private Vector2 touchStartScreenPosition;
    private bool isTouchActive = false;
    public bool longPressActive = false;
    

    public void OnTouchEvent(TouchData touchData)
    {
        //Debug.Log($"📌 {gameObject.name} fick touch-event: {touchData.phase} vid {touchData.position}");
        
        switch (touchData.phase)
        {
            case TouchPhase.Began:
               //Debug.Log("Nu börjar jag");
               if (UIWrappers.IsPointerOverUI(touchData.screenPosition))
               {
                   Debug.Log("Japp på UI)");
                   return;
               }
                touchStartPosition = touchData.worldPosition;
                touchStartScreenPosition = touchData.screenPosition;
                startPosition = interactionObject.transform.position;
               
                wasClickedInBeganPhase = true;
                objectCollider = touchData.hitObject.GetComponent<Collider>();
                objectCollider.enabled = false;
               
                isTouchActive = true;
                pressTime = 0f;
                longPressTriggered = false;
               
                break;

            case TouchPhase.Moved:
                //Debug.Log("Nu drar jag");
                if (!wasClickedInBeganPhase) return;
                
                if (isDraggable)
                {
                    if (is2DMovement)
                    {
                        interactionObject.transform.position = new Vector3(touchData.worldPosition.x, startPosition.y, touchData.worldPosition.z) + midPointOffset;  
                    }
                    else
                    interactionObject.transform.position = touchData.worldPosition + midPointOffset;
                }
                
                break;

            case TouchPhase.Ended: 
            case TouchPhase.Canceled:
                //Debug.Log("Nu slutar jag");

                if (wasClickedInBeganPhase)
                {
                    if (is2DMovement)
                        touchEndPosition = new Vector3(touchData.worldPosition.x, startPosition.y, touchData.worldPosition.z);
                    else
                        touchEndPosition = touchData.worldPosition;

                    Vector2 touchEndScreenPosition = touchData.screenPosition;
                    float dragDistance = Vector3.Distance(touchStartScreenPosition, touchEndScreenPosition);

                    Debug.Log(" " + dragDistance + " " + touchStartScreenPosition + "End:" + touchEndScreenPosition);

                    if (dragDistance <= clickThreshold && !longPressTriggered)
                    {
                        //Debug.Log("Nu klickar jag");

                        OnClick?.Invoke(gameObject);
                        interactionObject.transform.position = startPosition;
                        Debug.Log("📌 Click triggered");
                    }

                    else if (isDraggable)
                    {

                        OnDragEnd?.Invoke(touchEndPosition, touchData, startPosition, interactionObject);
                    }
                }
                
                wasClickedInBeganPhase = false;
                objectCollider.enabled = true;
                isTouchActive = false;
                pressTime = 0f;
                longPressTriggered = false;
                break;
        }
    }

    public void Initialize()
    {
        var col = interactionObject?.GetComponent<Collider>();
        if (col != null)
            col.enabled = true;
    }
    
    private void Update()
    {
        if (longPressActive && isTouchActive && !longPressTriggered)
        {
            pressTime += Time.deltaTime;

            if (pressTime >= longPressDuration)
            {
                longPressTriggered = true;
                OnLongPress?.Invoke(gameObject);
                Debug.Log("📌 Long press triggered (via Update)");
            }
        }
    }
}