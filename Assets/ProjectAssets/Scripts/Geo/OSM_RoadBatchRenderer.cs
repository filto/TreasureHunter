using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;

public class OSM_RoadBatchRenderer : MonoBehaviour
{
    public float lat = 59.3326f;
    public float lon = 18.0649f;
    public float radius = 500f;
    public RoadRendererSettings roadSettings;

    private Dictionary<long, Vector2> nodeDict = new();
    private Dictionary<string, MeshBuilder> meshBuilders = new();

    void Start()
    {
        StartCoroutine(LoadRoads());
    }

    IEnumerator LoadRoads()
    {
        string query = $"[out:json];way[\"highway\"](around:{radius},{lat},{lon});out body;>;out skel qt;";
        string url = $"https://overpass-api.de/api/interpreter?data={UnityWebRequest.EscapeURL(query)}";

        UnityWebRequest www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("OSM download failed: " + www.error);
            yield break;
        }

        var response = JsonConvert.DeserializeObject<OverpassResponse>(www.downloadHandler.text);

        foreach (var el in response.elements)
        {
            if (el.type == "node")
            {
                nodeDict[el.id] = new Vector2(el.lon, el.lat);
            }
        }

        foreach (var el in response.elements)
        {
            if (el.type == "way" && el.tags != null && el.tags.TryGetValue("highway", out string type))
            {
                var rule = roadSettings.GetRuleFor(type) ?? roadSettings.defaultRule;
                if (rule == null) continue;

                if (!meshBuilders.TryGetValue(type, out MeshBuilder builder))
                {
                    builder = new MeshBuilder(rule.width);
                    meshBuilders[type] = builder;
                }

                AddRoadToBuilder(el.nodes, builder);
            }
        }

        // Skapa ett GameObject per vägtyp
        foreach (var kvp in meshBuilders)
        {
            var rule = roadSettings.GetRuleFor(kvp.Key) ?? roadSettings.defaultRule;
            var mesh = kvp.Value.BuildMesh();

            GameObject go = new GameObject($"Road_{kvp.Key}");
            var mf = go.AddComponent<MeshFilter>();
            var mr = go.AddComponent<MeshRenderer>();
            mf.mesh = mesh;
            mr.material = rule.material;
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

            builder.AddQuad(leftA, rightA, leftB, rightB);
        }
    }

    Vector3 LatLonToWorld(Vector2 latlon)
    {
        float scale = 100000f;
        float x = (latlon.x - lon) * scale;
        float z = (latlon.y - lat) * scale;
        return new Vector3(x, 0, z);
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
}
