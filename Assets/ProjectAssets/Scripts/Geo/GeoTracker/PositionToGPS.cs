using UnityEngine;

public class PositionToGPS : MonoBehaviour
{
    private Vector3 lastPosition;
    public float updateInterval = 2f;
    private float timeSinceLastUpdate = 0f;
    public float moveDistance = 0.001f;

    void Start()
    {
    }

    void LateUpdate()
    {
            timeSinceLastUpdate += Time.deltaTime;
            if (timeSinceLastUpdate >= updateInterval)
            {

                if (GPSManager.Instance.gpsMarker != null)
                {
                    GPSManager.Instance.gpsMarker.latitude += moveDistance;
                    //GPSManager.Instance.gpsMarker.longitude = newLatLon.y;
                }
                timeSinceLastUpdate = 0f;
            }
    }
}
