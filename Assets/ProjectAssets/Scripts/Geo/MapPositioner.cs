using UnityEngine;

public class MapPositioner : MonoBehaviour
{
    private Vector2 initialReference; // Ursprunglig GPS-startpunkt

    void Start()
    {
        initialReference = GetCurrentReferenceLatLon();
        UpdateMapPosition();
    }

    void Update()
    {
        UpdateMapPosition();
    }

    void UpdateMapPosition()
    {
        Vector2 current = GetCurrentReferenceLatLon();
        Vector2 delta = current - initialReference;

        float baseScale = GPSManager.Instance.worldScale;
        float latRad = GPSManager.Instance.referenceLatitude * Mathf.Deg2Rad;
        float xScale = baseScale * Mathf.Cos(latRad); // longitude-korrektion
        float zScale = baseScale; // latitude konstant

        Vector3 offset = new Vector3(delta.y * xScale, 0, delta.x * zScale);
        transform.position = -offset;

        //Debug.Log("World offset: " + offset);
    }

    Vector2 GetCurrentReferenceLatLon()
    {
        return new Vector2(
            GPSManager.Instance.referenceLatitude,
            GPSManager.Instance.referenceLongitude
        );
    }
}