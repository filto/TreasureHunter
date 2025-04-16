using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public Transform target;             // Spelaren att följa
    public Vector3 offset = new Vector3(0f, 4f, -6f); // Position relativt spelaren (lokalt)
    public float followSpeed = 5f;       // Hur snabbt kameran följer
    public float lookHeightOffset = 1.5f; // Titta lite över spelarens mittpunkt

    private Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;

        // Beräkna önskad position (bakom spelaren i lokal riktning)
        Vector3 desiredPosition = target.position + target.rotation * offset;

        // Mjuk följning med dampning
        transform.position = Vector3.SmoothDamp(transform.position, desiredPosition, ref velocity, 1f / followSpeed);

        // Titta på spelaren
        Vector3 lookTarget = target.position + Vector3.up * lookHeightOffset;
        transform.LookAt(lookTarget);
    }
}