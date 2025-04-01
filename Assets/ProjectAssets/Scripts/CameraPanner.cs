using UnityEngine;

public class CameraPan : MonoBehaviour
{

    public Camera cam;
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
                touchStartPosition = new Vector3 (touchData.worldPosition.x, touchData.worldPosition.y, cam.transform.position.z);
                break;

            case TouchPhase.Moved:
                //Debug.Log("Nu drar jag");
                Vector3 delta = touchStartPosition -
                                new Vector3(touchData.worldPosition.x, touchData.worldPosition.y, cam.transform.position.z);  
                cam.transform.position += new Vector3(delta.x, delta.y, 0);
                break;

            case TouchPhase.Ended:
                //Debug.Log("Nu slutar jag");
                break;
        }
    }
    
   
    
}

