// 🔹 Assets/Scripts/TouchData.cs
using UnityEngine;

public class TouchData
{
    public TouchPhase phase;
    public Vector3 position;
    public GameObject droppedObject;

    public TouchData(TouchPhase phase, Vector3 position, GameObject droppedObject)
    {
        this.phase = phase;
        this.position = position;
        this.droppedObject = droppedObject;
    }
}
