using UnityEngine;
using System.Collections.Generic;

public class InteractionManager : MonoBehaviour
{
    public static InteractionManager Instance; // 🔹 Singleton, så vi har bara en
    private Camera uiCamera;
    private bool isUIObject = false;
    private GameObject activeObject = null;
    private GameObject droppedObject = null;
    private Collider2D activeObjectCollider;
    
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
            Vector3 UITouchPosition = uiCamera.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
            Vector3 baseTouchPosition = Camera.main.ScreenToWorldPoint(new Vector3(touch.position.x, touch.position.y, 0));
            Collider2D UIHit = Physics2D.OverlapPoint(UITouchPosition);
            Collider2D baseHit = Physics2D.OverlapPoint(baseTouchPosition);
            GameObject UIHitObject = UIHit ? UIHit.gameObject : null;
            GameObject baseHitObject = baseHit ? baseHit.gameObject : null;
            Vector3 touchPosition=baseTouchPosition;
            
            switch (touch.phase)
            {
                case TouchPhase.Began:
                    
                    activeObject = baseHitObject;
                    activeObjectCollider = baseHit;
                    
                    if (UIHitObject != null && UIHitObject.CompareTag("UI"))
                    {
                        isUIObject = true;
                        activeObject = UIHitObject;
                        touchPosition = UITouchPosition;
                        activeObjectCollider = UIHit;
                    }
                    
                    activeObjectCollider.enabled = false;
                    
                    if (activeObject != null)
                    {
                        activeObject.SendMessage("OnTouchEvent", new TouchData(touch.phase, touchPosition, null), SendMessageOptions.DontRequireReceiver);
                    }
                    
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