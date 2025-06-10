using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro; // ← TextMeshPro namespace
using UnityEngine.Android;

public class GeoTracker : MonoBehaviour
{
    public TextMeshProUGUI gpsText; // ← Ändrat till TextMeshProUGUI
    public GPSMarker gpsMarker;

    IEnumerator Start()
    {
        if (!Input.location.isEnabledByUser)
        {
            gpsText.text = "GPS är avstängd";
            yield break;
        }
        
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.FineLocation))
        {
            Permission.RequestUserPermission(Permission.FineLocation);
            gpsText.text = "Begär platsrättigheter...";
            yield break; // Vänta tills användaren godkänner och starta om appen
        }
#endif

        Input.location.Start();

        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            gpsText.text = "Startar GPS...";
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            gpsText.text = "Kunde inte hämta GPS-data";
            yield break;
        }

        StartCoroutine(UpdateGPS());
    }

    IEnumerator UpdateGPS()
    {
        while (true)
        {
            var data = Input.location.lastData;

            // Uppdatera UI
            gpsText.text = $"Lat: {data.latitude:F6}\nLon: {data.longitude:F6}\nAlt: {data.altitude:F1} m";

            // Uppdatera GPSMarker
            if (gpsMarker != null)
            {
                gpsMarker.latitude = data.latitude;
                gpsMarker.longitude = data.longitude;
            }

            yield return new WaitForSeconds(1f);
        }
    }
    
    
}