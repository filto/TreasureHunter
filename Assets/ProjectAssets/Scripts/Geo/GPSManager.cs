using UnityEngine;

public class GPSManager : MonoBehaviour
{
    public static GPSManager Instance;
    
    public GPSMarker gpsMarker;
    public GameObject mapParent;

    public float referenceLatitude => gpsMarker != null ? gpsMarker.latitude : 0f;
    public float referenceLongitude => gpsMarker != null ? gpsMarker.longitude : 0f;
    public float worldScale = 100000f;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }
    
}