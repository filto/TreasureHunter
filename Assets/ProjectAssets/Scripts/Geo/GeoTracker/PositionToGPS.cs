using UnityEngine;

public class PositionToGPS : MonoBehaviour
{
    private Vector3 lastPosition;

    void Start()
    {
        lastPosition = transform.position;
    }

    void LateUpdate()
    {
        if (transform.position != lastPosition)
        {
            Vector2 newLatLon = GeoUtils.PositionToGPS(transform.position);

            if (GPSManager.Instance.gpsMarker != null)
            {
                GPSManager.Instance.gpsMarker.latitude = newLatLon.x;
                GPSManager.Instance.gpsMarker.longitude = newLatLon.y;
            }

            // Flytta tillbaka objektet till origo
            transform.position = Vector3.zero;

            lastPosition = Vector3.zero; // viktigt, så vi inte detekterar rörelse nästa frame igen
        }
    }
}
