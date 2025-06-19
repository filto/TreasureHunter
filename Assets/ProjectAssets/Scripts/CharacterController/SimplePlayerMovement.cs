using UnityEngine;

public class SimplePlayerMovement : MonoBehaviour
{
    public Camera cam;
    public float speed = 5f;
    public float rotationSpeed = 8f;
    public Joystick joystick;
    public GPSMarker gpsMarker; // ← Dra in GPSMarker här (eller sätt den globalt från t.ex. GeoManager)

    void Awake()
    {
        if (cam == null)
            cam = Camera.main;
    }

    void Update()
    {
        Vector2 input = joystick.Direction;
        if (input.sqrMagnitude < 0.01f) return;

        Vector3 camForward = cam.transform.forward;
        Vector3 camRight = cam.transform.right;
        camForward.y = 0f;
        camRight.y = 0f;

        Vector3 moveDirection = (camForward * input.y + camRight * input.x).normalized;

        Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);

        // Räkna ut ny lat/lon från rörelsen
        float refLat = GPSManager.Instance.gpsMarker.latitude;
        float refLon = GPSManager.Instance.gpsMarker.longitude;
        float latRad = refLat * Mathf.Deg2Rad;

        float deltaX = moveDirection.x * speed * Time.deltaTime;
        float deltaZ = moveDirection.z * speed * Time.deltaTime;

        // Räkna baklänges från world movement till lat/lon
        float worldScale = GPSManager.Instance.worldScale;
        float xScale = worldScale * Mathf.Cos(latRad);
        float zScale = worldScale;

        GPSManager.Instance.gpsMarker.longitude += deltaX / xScale;
        GPSManager.Instance.gpsMarker.latitude  += deltaZ / zScale;
    }
}