using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    public Camera cam;
    public float speed = 5f;
    public float rotationSpeed = 8f;

    public Joystick joystick; // ← Dra in din joystick här i Inspector

    void Awake()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        // Använd joystickens input istället för tangentbord
        Vector2 input = joystick.Direction;

        if (input.sqrMagnitude < 0.01f) return;

        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;

        Vector3 moveDirection = (camForward * input.y + camRight * input.x).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        transform.position += moveDirection * speed * input.magnitude * Time.deltaTime;
    }
}