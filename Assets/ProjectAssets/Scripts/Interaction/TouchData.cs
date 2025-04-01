// 🔹 Assets/Scripts/TouchData.cs
using UnityEngine;

public class TouchData
{
    public TouchPhase phase;
    public Vector3 worldPosition;
    public Vector3 screenPosition;
    public GameObject hitObject;

    public TouchData(TouchPhase phase, Vector3 worldPosition, Vector3 screenPosition, GameObject hitObject)
    {
        this.phase = phase;
        this.worldPosition = worldPosition;
        this.screenPosition = screenPosition;
        this.hitObject = hitObject;
    }
}
