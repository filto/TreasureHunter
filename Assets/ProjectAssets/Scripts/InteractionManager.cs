using UnityEngine;
using System.Collections.Generic;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance; // 🔹 Singleton, så vi har bara en
    private Camera uiCamera;
    private bool isUIObject = false;
    private GameObject activeObject = null;
    private GameObject droppedObject = null;
    private Collider activeObjectCollider;
    
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    
    private void Start()
    {
        uiCamera = GameManager.Instance.uiCamera; // 🔄 Använd en befintlig kamera istället för att instansiera
    }

    void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Ray uiRay = uiCamera.ScreenPointToRay(touch.position);
            Ray worldRay = Camera.main.ScreenPointToRay(touch.position);

            RaycastHit hitUI;
            RaycastHit hitBase;

            GameObject UIHitObject = null;
            GameObject baseHitObject = null;

            Collider UIHitCollider = null;
            Collider baseHitCollider = null;

            Vector3 UITouchPosition = Vector3.zero;
            Vector3 baseTouchPosition = Vector3.zero;

            if (Physics.Raycast(uiRay, out hitUI,100f))
            {
                UIHitObject = hitUI.collider.gameObject;
                UIHitCollider = hitUI.collider;
                UITouchPosition = hitUI.point;
            }

            if (Physics.Raycast(worldRay, out hitBase, 100f))
            {
                baseHitObject  = hitBase.collider.gameObject;
                baseHitCollider = hitBase.collider;
                baseTouchPosition = hitBase.point;
            }

            Vector3 touchPosition = baseTouchPosition;
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    
                    activeObject = baseHitObject;
                    activeObjectCollider = baseHitCollider;
                    
                    if (UIHitObject != null && UIHitObject.CompareTag("UI"))
                    {
                        isUIObject = true;
                        activeObject = UIHitObject;
                        touchPosition = UITouchPosition;
                        activeObjectCollider = UIHitCollider;
                    }
                    
                    activeObjectCollider.enabled = false;
                    
                    if (activeObject != null)
                    {
                        activeObject.SendMessage("OnTouchEvent", new TouchData(touch.phase, touchPosition, null), SendMessageOptions.DontRequireReceiver);
                    }
                    
                    Debug.Log("Began: " + activeObject.name + " at " + touchPosition);
                    
                    break;

                case TouchPhase.Moved:
                    
                    if (isUIObject)
                    {
                        touchPosition = UITouchPosition; 
                    }
                    
                    if (activeObject != null)
                    {
                        activeObject.SendMessage("OnTouchEvent", new TouchData(touch.phase, touchPosition, null), SendMessageOptions.DontRequireReceiver);
                    }
                    
                    Debug.Log("Moved hit: " + activeObject.name + " at " + touchPosition);
                    
                    break;

                case TouchPhase.Ended:
                    
                    droppedObject = baseHitObject;
                    
                    if (UIHitObject != null && UIHitObject.CompareTag("UI"))
                    {
                        isUIObject = true;
                        droppedObject = UIHitObject;
                    }
                    
                    activeObjectCollider.enabled = true;
                    
                    if (activeObject != null)
                    {
                        activeObject.SendMessage("OnTouchEvent", new TouchData(touch.phase, touchPosition, droppedObject), SendMessageOptions.DontRequireReceiver);
                    }
                    
                    isUIObject = false;
                    
                    break;
            }
        }
    }
}