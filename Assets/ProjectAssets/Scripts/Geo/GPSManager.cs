using UnityEngine;

public class GPSManager : MonoBehaviour
{
    public static GPSManager Instance;
    
    public GPSMarker gpsMarker;
    public GameObject mapParent;
    public float heading = 0;
    public float speed = 0;

    public float referenceLatitude;
    public float referenceLongitude;
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

    private void Start()
    {
        SetReferencePoint();
    }

    public void SetReferencePoint()
    {
        referenceLatitude = gpsMarker.latitude;
        referenceLongitude = gpsMarker.longitude;
    }
    
}