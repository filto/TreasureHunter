using UnityEngine;

public class CameraPanner : MonoBehaviour
{
    public Camera cam;
    public Collider mapCollider; // Dra in 3D-collider i inspector

    private Vector3 startWorldPosition;
    private float zoomSpeed = 0.01f;
    private float scrollZoomSpeed = 2f;
    public float speedX = 1;
    public float speedZ = 1;
    private Vector2 lastTouchPos;
    private Vector3 inertia = Vector3.zero;
    public float inertiaDamp = 0.9f;  // 0.9 → långsam broms, 0.5 → snabbare
    public float inertiaThreshold = 0.01f; // under detta → stopp
    public float minZoom = 2f;
    public float maxZoom = 20f;
    
    private float doubleClickTime = 0.25f; // Max tid mellan klick för att räknas som dubbelklick
    private float lastClickTime = -1f;
    private bool zoomMode = false;
    private Vector2 lastMousePos;
    private bool isActive = false;
        

    void Update()
    {
        if (Input.touchCount == 1 && !zoomMode)
        {
            Touch touch = Input.GetTouch(0);
                
            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPos = touch.position;
                
                bool touchedUI = UIWrappers.IsPointerOverUI(touch.position);
                bool touchedMap = IsTouchOnMap(touch.position);
                isActive = !touchedUI && touchedMap;
                Debug.Log(isActive);
            }
            
            if (touch.phase == TouchPhase.Ended)
            {
                isActive = false;
            }
            
            if (!isActive) return;
            
            if (touch.phase == TouchPhase.Moved && isActive)
            {
                Vector3 worldA = RaycastToWorld(lastTouchPos);
                Vector3 worldB = RaycastToWorld(touch.position);

                Vector3 delta = worldA - worldB;
                transform.position += new Vector3(delta.x * speedX, 0f, delta.z * speedZ);

                //Debug.Log($"Delta: {delta}, Pos: {transform.position}");
                
                inertia = delta / Time.deltaTime;

                lastTouchPos = touch.position; // bara screen-coord!
                
            }
            
        }
        
        if (Input.touchCount == 2)
        {
            Touch t1 = Input.GetTouch(0);
            Touch t2 = Input.GetTouch(1);

            if (IsTouchOnMap(t1.position) || IsTouchOnMap(t2.position))
            {
                float prevDistance = ((t1.position - t1.deltaPosition) - (t2.position - t2.deltaPosition)).magnitude;
                float currDistance = (t1.position - t2.position).magnitude;
                float zoomDelta = (currDistance - prevDistance) * zoomSpeed;

                cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - zoomDelta, 2f, 20f);
            }
        }
        
        if (Input.touchCount == 0 && inertia.magnitude > inertiaThreshold)
        {
            transform.position += new Vector3(
                inertia.x * speedX * Time.deltaTime,
                0f,
                inertia.z * speedZ * Time.deltaTime
            );

            inertia *= inertiaDamp; // bromsa in

            // Debug
            //Debug.Log($"Inertia: {inertia}");
        }
        
        
        #region EditorZoom       
#if UNITY_EDITOR
        if (Input.GetMouseButtonDown(0))
        {
            float timeSinceLastClick = Time.time - lastClickTime;

            if (timeSinceLastClick <= doubleClickTime)
            {
                zoomMode = true;
                Debug.Log("🔍 Zoom mode activated");
            }

            lastClickTime = Time.time;
            lastMousePos = Input.mousePosition;
        }

        if (zoomMode && Input.GetMouseButton(0))
        {
            Vector2 currentMousePos = Input.mousePosition;
            float deltaY = currentMousePos.y - lastMousePos.y;

            cam.orthographicSize = Mathf.Clamp(
                cam.orthographicSize - deltaY * scrollZoomSpeed * 0.01f,
                minZoom,
                maxZoom
            );

            lastMousePos = currentMousePos;
        }

        if (zoomMode && Input.GetMouseButtonUp(0))
        {
            zoomMode = false;
            Debug.Log("❌ Zoom mode deactivated");
        }
#endif
        #endregion
    }
    
    private bool IsTouchOnMap(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.collider == mapCollider;
        }
        return false;
    }
    
    private Vector3 RaycastToWorld(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.point;
        }
        
        return Vector3.zero;
    }

}



