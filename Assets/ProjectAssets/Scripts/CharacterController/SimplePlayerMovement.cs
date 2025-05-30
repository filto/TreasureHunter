using UnityEngine;
using UnityEngine.InputSystem;

public class SimplePlayerMovement : MonoBehaviour
{
    public InputActionReference moveAction;
    public Camera cam; // Dra in kameran här
    public float speed = 5f;
    public float rotationSpeed = 8f;

    void Awake()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void OnEnable()
    {
        moveAction.action.Enable();
    }

    void OnDisable()
    {
        moveAction.action.Disable();
    }

    void Update()
    {
        Vector2 input = moveAction.action.ReadValue<Vector2>();
        if (input.sqrMagnitude < 0.01f) return;

        // 1. Beräkna rörelseriktning i kamerans space
        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;

        Vector3 moveDirection = (camForward * input.y + camRight * input.x).normalized;

        // 2. Rotera spelaren mot rörelseriktningen
        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // 3. Flytta spelaren framåt
        transform.position += moveDirection * speed * input.magnitude * Time.deltaTime;
    }
}