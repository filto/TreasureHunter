using UnityEngine;
using System.Collections;

public class MapPositioner : MonoBehaviour
{
    private Vector2 initialReference; // Ursprunglig GPS-startpunkt
    private Vector2 lastGPSPosition;
    private Vector3 targetWorldPosition;
    private bool isInterpolatingToGPS = false;
    public float interpolationTime = 1f;
    private float interpolationProgress = 0f;
    private Vector3 gravity = Vector3.zero;
    private float filterFactor = 0.1f;
    private Vector3 simulatedOffset;
    public float simulationSpeed = 1f;
    

    void Awake()
    {
        StartCoroutine(DelayedStart());
    }
    IEnumerator DelayedStart()
    {
        yield return new WaitForSeconds(5f); 
        
        lastGPSPosition = GetCurrentLatLon();
        targetWorldPosition = OffsetFromGPS(lastGPSPosition);
        transform.position = targetWorldPosition;
    }

    void Update()
    {
        Vector2 currentGPSPosition = GetCurrentLatLon();
        
        if (currentGPSPosition != lastGPSPosition)
        {
            lastGPSPosition = currentGPSPosition;
            isInterpolatingToGPS = true;
            simulatedOffset = Vector3.zero;
        }
        
        if (isInterpolatingToGPS)
        {
            InterpolateToGPSPosition();
        }

        else
        {
            SimulateMotion(); 
        }
        
    }
    
    Vector3 OffsetFromGPS(Vector2 gpsPosition)
    {
        Vector2 delta = gpsPosition - GetReferenceLatLon();

        float baseScale = GPSManager.Instance.worldScale;
        float latRad = GPSManager.Instance.referenceLatitude * Mathf.Deg2Rad;
        float xScale = baseScale * Mathf.Cos(latRad); // longitude-korrektion
        float zScale = baseScale;

        return new Vector3(-delta.y * xScale, 0, -delta.x * zScale);
    }

    Vector2 GetCurrentLatLon()
    {
        return new Vector2(
            GPSManager.Instance.gpsMarker.latitude,
            GPSManager.Instance.gpsMarker.longitude
        );
    }
    
    Vector2 GetReferenceLatLon()
    {
        return new Vector2(
            GPSManager.Instance.referenceLatitude,
            GPSManager.Instance.referenceLongitude
        );
    }
    
    void InterpolateToGPSPosition()
    {
        interpolationProgress += Time.deltaTime / interpolationTime;

        float t = Mathf.Clamp01(interpolationProgress);

        // Räkna om målet i world space baserat på senaste GPS-position
        targetWorldPosition = OffsetFromGPS(lastGPSPosition);

        Vector3 currentPosition = transform.position;
        Vector3 newPosition = Vector3.Lerp(currentPosition, targetWorldPosition, t);
        GPSManager.Instance.mapParent.transform.position = newPosition;

        if (t >= 1f)
        {
            isInterpolatingToGPS = false;
            interpolationProgress = 0f;
        }
    }
    void SimulateMotion()
    {
        

        float heading = GPSManager.Instance.heading;
        Vector3 direction = Quaternion.Euler(0, heading, 0) * Vector3.forward;
        
        float speed = GPSManager.Instance.speed;
        float baseSpeed = GPSManager.Instance.worldScale * (0.00001f*simulationSpeed); 

        simulatedOffset += direction * speed * baseSpeed * Time.deltaTime;

        // Lägg till simulering ovanpå GPS-position
        GPSManager.Instance.mapParent.transform.position = OffsetFromGPS(lastGPSPosition) - simulatedOffset;
    }
    
    float GetMotionStrength()
    {
        Vector3 rawAccel = Input.acceleration;
        gravity = Vector3.Lerp(gravity, rawAccel, filterFactor);
        Vector3 motion = rawAccel - gravity;

        return motion.magnitude > 0.05f ? motion.magnitude : 0f; // tröskel för brus
    }
    
}