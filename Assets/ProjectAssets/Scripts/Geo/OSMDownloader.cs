using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using UnityEngine.UI;

public class OSMDownloader : MonoBehaviour
{
    public Action<OverpassResponse> onDataDownloaded;
    
    [System.Serializable]
    public class OSMFilter
    {
        public string key = "highway";
        public string value = "*";
        public bool enabled = true;
    }

    [Header("Position")]
    /*public float latitude = 59.3326f;
    public float longitude = 18.0649f;*/
    public float radius = 500f;
    

    [Header("OSM Filters")]
    public List<OSMFilter> filters = new List<OSMFilter>
    {
    };

    public void Start()
    {
        
        if (GPSManager.Instance == null)
        {
            Debug.LogError("GPSManager.Instance saknas!");
            return;
        }

        float latitude = GPSManager.Instance.referenceLatitude;
        float longitude = GPSManager.Instance.referenceLongitude;
        
        StartCoroutine(DownloadOverpassData(latitude, longitude, (response) =>
        {
            Debug.Log($"Downloaded {response.elements.Count} OSM elements.");
        }));
    }

    public IEnumerator DownloadOverpassData(float latitude, float longitude, Action<OverpassResponse> callback)
    {
        string query = "[out:json];(";

        foreach (var filter in filters)
        {
            if (!filter.enabled) continue;

            if (string.IsNullOrEmpty(filter.value) || filter.value == "*")
                query += $"way[\"{filter.key}\"](around:{radius},{latitude},{longitude});";
            else
                query += $"way[\"{filter.key}\"=\"{filter.value}\"](around:{radius},{latitude},{longitude});";
        }

        query += ");out body;>;out skel qt;";
        string url = $"https://overpass-api.de/api/interpreter?data={UnityWebRequest.EscapeURL(query)}";

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Overpass API error: " + www.error);
            yield break;
        }

        var response = JsonConvert.DeserializeObject<OverpassResponse>(www.downloadHandler.text);
        onDataDownloaded?.Invoke(response);
    }
    
    
    
    
    [Serializable]
    public class OverpassResponse
    {
        public List<OverpassElement> elements;
    }

    [Serializable]
    public class OverpassElement
    {
        public string type;
        public long id;
        public float lat;
        public float lon;
        public List<long> nodes;
        public Dictionary<string, string> tags;
    }
    
    private int LonToTileX(float lon, int zoom)
    {
        return (int)((lon + 180f) / 360f * (1 << zoom));
    }

    private int LatToTileY(float lat, int zoom)
    {
        float latRad = lat * Mathf.Deg2Rad;
        return (int)((1f - Mathf.Log(Mathf.Tan(latRad) + 1f / Mathf.Cos(latRad)) / Mathf.PI) / 2f * (1 << zoom));
    }
}