using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoadVisualizer : MonoBehaviour
{
    public OSMDownloader downloader;
    public RoadRendererSettings roadSettings;

    private Dictionary<long, Vector2> nodeDict = new();
    private Dictionary<string, MeshBuilder> meshBuilders = new();

    void Start()
    {
        if (GPSManager.Instance == null)
        {
            Debug.LogError("GPSManager.Instance saknas!");
            return;
        }

        downloader.onDataDownloaded += BuildRoads;
    }

    void BuildRoads(OSMDownloader.OverpassResponse response)
    {
        nodeDict.Clear();
        meshBuilders.Clear();

        // Samla noder
        foreach (var el in response.elements)
        {
            if (el.type == "node")
            {
                //nodeDict[el.id] = new Vector2(el.lon, el.lat);
                nodeDict[el.id] = new Vector2(el.lat, el.lon);
            }
        }

        // Skapa vägar
        foreach (var el in response.elements)
        {
            if (el.type != "way" || el.tags == null) continue;

            if (!el.tags.TryGetValue("highway", out string type)) continue;

            var baseRule = roadSettings.GetRuleFor(type) ?? roadSettings.defaultRule;

            // Skapa en lokal kopia av regeln så vi inte ändrar instans i inspectorn
            var rule = new RoadRenderRule
            {
                highwayType = baseRule.highwayType,
                material = baseRule.material,
                width = baseRule.width
            };

            

            // Fortsätt med vanlig väg
            if (!meshBuilders.TryGetValue(type, out MeshBuilder builder))
            {
                builder = new MeshBuilder(rule.width);
                meshBuilders[type] = builder;
            }

            AddRoadToBuilder(el.nodes, builder);
            
        }

        // Skapa mesh per vägtyp
        foreach (var pair in meshBuilders)
        {
            var finalRule = roadSettings.GetRuleFor(pair.Key) ?? roadSettings.defaultRule;
            var mesh = pair.Value.BuildMesh();

            GameObject go = new GameObject($"Road_{pair.Key}");
            var mf = go.AddComponent<MeshFilter>();
            var mr = go.AddComponent<MeshRenderer>();
            mf.mesh = mesh;
            mr.material = finalRule.material;
            
            // Lägg som barn till mapParent
            if (GPSManager.Instance != null && GPSManager.Instance.mapParent != null)
            {
                go.transform.SetParent(GPSManager.Instance.mapParent.transform, worldPositionStays: true);
            }
        }
    }

    void AddRoadToBuilder(List<long> nodeIds, MeshBuilder builder)
    {
        if (nodeIds.Count < 2) return;

        for (int i = 0; i < nodeIds.Count - 1; i++)
        {
            if (!nodeDict.TryGetValue(nodeIds[i], out Vector2 a)) continue;
            if (!nodeDict.TryGetValue(nodeIds[i + 1], out Vector2 b)) continue;

            Vector3 a3 = LatLonToWorld(a);
            Vector3 b3 = LatLonToWorld(b);

            Vector3 dir = (b3 - a3).normalized;
            Vector3 normal = Vector3.Cross(dir, Vector3.up);
            Vector3 offset = normal * (builder.width / 2f);

            Vector3 leftA = a3 - offset;
            Vector3 rightA = a3 + offset;
            Vector3 leftB = b3 - offset;
            Vector3 rightB = b3 + offset;
            
            //Debug.Log($"leftA: {leftA}, rightA: {rightA}, leftB: {leftB}, rightB: {rightB}");

            builder.AddQuad(leftA, rightA, leftB, rightB);
        }
    }
    
    Vector3 LatLonToWorld(Vector2 latlon)
    {
        float refLat = GPSManager.Instance.referenceLatitude;
        float refLon = GPSManager.Instance.referenceLongitude;
        float baseScale = GPSManager.Instance.worldScale;

        float latRad = refLat * Mathf.Deg2Rad;
        float xScale = baseScale * Mathf.Cos(latRad); // justera longitude
        float zScale = baseScale; // latitude skalar vi inte

        float x = (latlon.y - refLon) * xScale;
        float z = (latlon.x - refLat) * zScale;

        return new Vector3(x, 0, z);
    }
}
