using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance; // 🔹 Singleton, så vi har bara en
    public GameObject activeObject = null;
    public Camera raycastCamera;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    void OnEnable()
    {
        activeObject = null;
    }
    
    public void ResetInteraction()
    {
        activeObject = null;
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            
            Ray worldRay = raycastCamera.ScreenPointToRay(touch.position);
            RaycastHit hitRay;
            GameObject hitObject = null;
            Vector3 worldPosition = Vector3.zero;
            
            if (Physics.Raycast(worldRay, out hitRay, 2000f))
            {
                hitObject  = hitRay.collider.gameObject;
                worldPosition = hitRay.point;
            }

            if (hitObject == null) return;

            var touchData = new TouchData(
                phase: touch.phase,
                worldPosition: worldPosition,
                screenPosition: touch.position,
                hitObject: hitObject,
                touchCount:Input.touchCount
            );

            if (touchData.phase == TouchPhase.Began)
            {
                if (activeObject != null)
                {
                    var interaction = activeObject.GetComponent<Interaction>();
                    interaction?.Initialize(); 
                }

                activeObject = hitObject;
            }

            if (activeObject == null) return;
            
            activeObject.SendMessage("OnTouchEvent", touchData, SendMessageOptions.DontRequireReceiver);
            
        }
    }
}