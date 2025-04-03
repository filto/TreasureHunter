// 🔹 Assets/Scripts/TouchData.cs
using UnityEngine;

public class TouchData
{
    public TouchPhase phase;
    public Vector3 worldPosition;
    public Vector3 screenPosition;
    public GameObject hitObject;
    public int touchCount;

    public TouchData(TouchPhase phase, Vector3 worldPosition, Vector3 screenPosition, GameObject hitObject, int touchCount)
    {
        this.phase = phase;
        this.worldPosition = worldPosition;
        this.screenPosition = screenPosition;
        this.hitObject = hitObject;
        this.touchCount = touchCount;
        
    }
}
