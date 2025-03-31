using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance; // 🔹 Singleton, så vi har bara en
    private GameObject activeObject = null;
    private GameObject droppedObject = null;
    private Collider activeObjectCollider;
    private bool touchStartedOverUI = false;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            Ray worldRay = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hitRay;
            GameObject hitObject = null;
            Collider hitCollider = null;
            Vector3 touchPosition = Vector3.zero;
            
            if (Physics.Raycast(worldRay, out hitRay, 100f))
            {
                hitObject  = hitRay.collider.gameObject;
                hitCollider = hitRay.collider;
                touchPosition = hitRay.point;
            }
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    
                    touchStartedOverUI = UIWrappers.IsPointerOverUI(touch.position);
                    if (touchStartedOverUI)
                    {
                        // Den här touchen tillhör UI – InteractionManager ska inte göra nåt alls
                        return;
                    }
                    
                    activeObject = hitObject;
                    activeObjectCollider = hitCollider;
                    
                    activeObjectCollider.enabled = false;
                    
                    if (activeObject != null)
                    {
                        activeObject.SendMessage("OnTouchEvent", new TouchData(touch.phase, touchPosition, null), SendMessageOptions.DontRequireReceiver);
                    }
                    
                    //Debug.Log("Began: " + activeObject.name + " at " + touchPosition);
                    
                    break;

                case TouchPhase.Moved:

                    if (touchStartedOverUI) return;
                    
                    if (activeObject != null)
                    {
                        activeObject.SendMessage("OnTouchEvent", new TouchData(touch.phase, touchPosition, null), SendMessageOptions.DontRequireReceiver);
                    }
                    
                    //Debug.Log("Moved hit: " + activeObject.name + " at " + touchPosition);
                    
                    break;

                case TouchPhase.Ended:
                    
                    if (touchStartedOverUI) return;
                    
                    droppedObject = hitObject;
                    activeObjectCollider.enabled = true;
                    
                    if (activeObject != null)
                    {
                        activeObject.SendMessage("OnTouchEvent", new TouchData(touch.phase, touchPosition, droppedObject), SendMessageOptions.DontRequireReceiver);
                    }
                    
                    break;
            }
        }
    }
}