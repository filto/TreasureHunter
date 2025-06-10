/*using UnityEngine;
using System.Collections;

public class MapChunkLoader : MonoBehaviour
{
    public GPSMarker gpsMarker;
    public float reloadDistanceMeters = 100f;
    public Transform mapParent; // Samma objekt som MapPositioner ligger på
    public OSMDownloader osmDownloader;

    private Vector3 lastWorldOffset;
    private bool isLoading = false;
    

    void Start()
    {
        if (gpsMarker == null || mapParent == null)
        {
            Debug.LogWarning("GPSMarker eller MapParent är inte tilldelad.");
            enabled = false;
            return;
        }

        lastWorldOffset = mapParent.position;
        StartCoroutine(LoadChunkAt(new Vector2(gpsMarker.latitude, gpsMarker.longitude)));
    }

    void Update()
    {
        if (isLoading) return;

        float movedDistance = Vector3.Distance(mapParent.position, lastWorldOffset);

        if (movedDistance > reloadDistanceMeters)
        {
            StartCoroutine(LoadChunkAt(new Vector2(gpsMarker.latitude, gpsMarker.longitude)));
        }
    }

    IEnumerator LoadChunkAt(Vector2 latlon)
    {
        isLoading = true;

        // Ladda ny data (anpassa detta till din OSMDownloader)
        yield return StartCoroutine(osmDownloader.DownloadOverpassData(latlon));

        lastWorldOffset = mapParent.position;
        isLoading = false;
    }
}*/