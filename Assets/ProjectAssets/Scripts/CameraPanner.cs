using UnityEngine;

public class CameraPan : MonoBehaviour
{

    public Camera cam;
    private bool isPanning = false;
    private Vector3 lastTouchPosition;
    private Vector3 touchStartPosition;
    //private float panThreshold = 0.1f; // Minsta rörelse innan vi räknar det som en panorering
    
    
    public void OnTouchEvent(TouchData touchData)
    {
        //Debug.Log($"📌 {gameObject.name} fick touch-event: {touchData.phase} vid {touchData.position}");
        
        switch (touchData.phase)
        {
            case TouchPhase.Began:
                // Debug.Log("Nu börjar jag");
                touchStartPosition = new Vector3 (touchData.position.x, touchData.position.y, cam.transform.position.z);
                break;

            case TouchPhase.Moved:
                //Debug.Log("Nu drar jag");
                Vector3 delta = touchStartPosition -
                                new Vector3(touchData.position.x, touchData.position.y, cam.transform.position.z);  
                cam.transform.position += new Vector3(delta.x, delta.y, 0);
                break;

            case TouchPhase.Ended:
                //Debug.Log("Nu slutar jag");
                break;
        }
    }
    
   
    /*
    /*void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);
            Vector3 worldPosition = GetWorldPosition(touch.position);

            switch (touch.phase)
            {
                case TouchPhase.Began:
                    StartPan(worldPosition);
                    break;

                case TouchPhase.Moved:
                    if (isPanning) MoveCamera(worldPosition);
                    break;

                case TouchPhase.Ended:
                    if (isPanning) EndPan();
                    break;
            }
        }
    }

    private void StartPan(Vector3 touchPosition)
    {
            isPanning = true;
            touchStartPosition= touchPosition;

            //Debug.Log($"📌 Pan startar! Träffade: {hit.gameObject.name}");
    }

    private void MoveCamera(Vector3 touchPosition)
    {
        Vector3 delta = touchStartPosition - touchPosition;  // Beräkna skillnaden från senaste positionen
        cam.transform.position += new Vector3(delta.x, delta.y, 0); // Lägg till offset
        
    }

    private void EndPan()
    {
        isPanning = false;
        //Debug.Log("✅ Pan avslutad!");

       
    }

    private Vector3 GetWorldPosition(Vector2 screenPosition)
    {
        Vector3 position = screenPosition;
        position.z = -cam.transform.position.z;
        return cam.ScreenToWorldPoint(position);
    }*/
}

