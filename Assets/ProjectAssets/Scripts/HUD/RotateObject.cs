using UnityEngine;

public class RotateObject : MonoBehaviour
{
    public Vector3 rotationAxis = Vector3.up; // Y-led som standard
    public float rotationSpeed = 90f; // grader per sekund
    public bool rotate = true;

    void Update()
    {
        if (rotate)
        {
            transform.Rotate(rotationAxis.normalized * rotationSpeed * Time.deltaTime);
        }
    }
}