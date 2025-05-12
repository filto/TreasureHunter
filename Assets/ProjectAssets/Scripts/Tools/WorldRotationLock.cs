using UnityEngine;

public class WorldRotationLock : MonoBehaviour
{
    [Tooltip("Rotation i världsrummet som objektet ska följa")]
    public Vector3 targetWorldEulerAngles = Vector3.zero;

    [Tooltip("Hur snabbt objektet följer målet (högre = snabbare)")]
    [Range(0.01f, 20f)]
    public float rotationSpeed = 5f;

    void LateUpdate()
    {
        Quaternion targetRotation = Quaternion.Euler(targetWorldEulerAngles);

        // Interpolera mot målet i världsrummet
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
    }
}