using UnityEngine;
using System.Collections.Generic;
using UnityEngine.EventSystems;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance; // 🔹 Singleton, så vi har bara en
    GameObject activeObject = null;
    
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
            Vector3 worldPosition = Vector3.zero;
            
            if (Physics.Raycast(worldRay, out hitRay, 100f))
            {
                hitObject  = hitRay.collider.gameObject;
                worldPosition = hitRay.point;
            }

            if (hitObject == null) return;

            var touchData = new TouchData(
                phase: touch.phase,
                worldPosition: worldPosition,
                screenPosition: touch.position,
                hitObject: hitObject
            );
            
            if (touchData.phase == TouchPhase.Began)
                activeObject = hitObject;
            
            if (activeObject == null) return;
            
            activeObject.SendMessage("OnTouchEvent", touchData, SendMessageOptions.DontRequireReceiver);
            
        }
    }
}