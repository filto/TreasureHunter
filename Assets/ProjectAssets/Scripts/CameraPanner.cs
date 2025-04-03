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
        

    void Update()
    {
        
        if (Input.touchCount == 1)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPos = touch.position;
            }
            else if (touch.phase == TouchPhase.Moved)
            {
                Vector3 worldA = RaycastToWorld(lastTouchPos);
                Vector3 worldB = RaycastToWorld(touch.position);

                Vector3 delta = worldA - worldB;
                transform.position += new Vector3(delta.x * speedX, 0f, delta.z * speedZ);

                Debug.Log($"Delta: {delta}, Pos: {transform.position}");
                
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

        float scroll = Input.mouseScrollDelta.y;
        if (scroll != 0f)
        {
            cam.orthographicSize = Mathf.Clamp(cam.orthographicSize - scroll * scrollZoomSpeed, 2f, 20f);
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
            Debug.Log($"Inertia: {inertia}");
        }
    }
    
    private bool IsTouchOnMap(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;
        return mapCollider.Raycast(ray, out hit, Mathf.Infinity);
    }
    
    private Vector3 RaycastToWorld(Vector2 screenPos)
    {
        Ray ray = cam.ScreenPointToRay(screenPos);
        RaycastHit hit;

        if (mapCollider.Raycast(ray, out hit, Mathf.Infinity))
        {
            return hit.point;
        }

        return Vector3.zero; // eller behåll senaste pos om du vill undvika hopp
    }

}



