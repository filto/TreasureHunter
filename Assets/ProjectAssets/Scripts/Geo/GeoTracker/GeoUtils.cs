using UnityEngine;

public static class GeoUtils
{
    public static Vector3 LatLonToWorld(float lat, float lon)
    {
        if (GPSManager.Instance == null)
        {
            Debug.LogError("GPSManager.Instance is null! GeoUtils cannot convert coordinates.");
            return Vector3.zero;
        }

        float refLat = GPSManager.Instance.referenceLatitude;
        float refLon = GPSManager.Instance.referenceLongitude;

        float metersPerDegreeLat = 111_132f;
        float metersPerDegreeLon = 111_320f * Mathf.Cos(refLat * Mathf.Deg2Rad);

        float deltaLat = lat - refLat;
        float deltaLon = lon - refLon;

        float x = deltaLon * metersPerDegreeLon;
        float z = deltaLat * metersPerDegreeLat;

        return new Vector3(x, 0, z);
    }
    
    public static Vector2 WorldToLatLon(Vector3 worldPos)
    {
        if (GPSManager.Instance == null)
        {
            Debug.LogError("GPSManager.Instance is null! GeoUtils cannot convert coordinates.");
            return Vector2.zero;
        }

        float refLat = GPSManager.Instance.referenceLatitude;
        float refLon = GPSManager.Instance.referenceLongitude;

        float metersPerDegreeLat = 111_132f;
        float metersPerDegreeLon = 111_320f * Mathf.Cos(refLat * Mathf.Deg2Rad);

        float deltaLon = worldPos.x / metersPerDegreeLon;
        float deltaLat = worldPos.z / metersPerDegreeLat;

        float lat = refLat + deltaLat;
        float lon = refLon + deltaLon;

        return new Vector2(lat, lon);
    }
    
    public static Vector2 PositionToGPS(Vector3 worldPos)
    {
        float refLat = GPSManager.Instance.referenceLatitude;
        float refLon = GPSManager.Instance.referenceLongitude;
        float baseScale = GPSManager.Instance.worldScale;

        float latRad = refLat * Mathf.Deg2Rad;
        float xScale = baseScale * Mathf.Cos(latRad); // longitude-scale
        float zScale = baseScale; // latitude-scale

        float deltaLon = worldPos.x / xScale;
        float deltaLat = worldPos.z / zScale;

        float lat = refLat + deltaLat;
        float lon = refLon + deltaLon;

        return new Vector2(lat, lon); // Notera: lat = y, lon = x
    }
}
