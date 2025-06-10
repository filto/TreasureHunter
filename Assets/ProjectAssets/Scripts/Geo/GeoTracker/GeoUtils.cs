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
}
