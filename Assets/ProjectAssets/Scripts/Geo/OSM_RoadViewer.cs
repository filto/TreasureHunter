using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using Newtonsoft.Json;
using System;

public class OSM_RoadViewer : MonoBehaviour
{
    public float lat = 59.3326f;
    public float lon = 18.0649f;
    public float radius = 500f;
    public RoadRendererSettings roadSettings;

    Dictionary<long, Vector2> nodeDict = new();

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

        string json = www.downloadHandler.text;
        var wrappedJson = "{\"elements\":" + json.Split(new[] { "\"elements\":" }, StringSplitOptions.None)[1];
        var response = JsonConvert.DeserializeObject<OverpassResponse>(json);

        foreach (var el in response.elements)
        {
            if (el.type == "node")
            {
                nodeDict[el.id] = new Vector2(el.lon, el.lat);
            }
        }

        foreach (var el in response.elements)
        {
            if (el.type == "way" && el.tags != null && el.tags.ContainsKey("highway"))
            {
                Debug.Log("Found highway type: " + el.tags["highway"]);
                var rule = roadSettings.GetRuleFor(el.tags["highway"]);
                if (rule != null)
                {
                    GenerateRoadMesh(el.nodes, rule);
                }
            }
        }
        Debug.Log($"Totalt: {response.elements.Count} element från Overpass");
    }

    void GenerateRoadMesh(List<long> nodeIds, RoadRenderRule rule)
    {
        List<Vector3> leftSide = new();
        List<Vector3> rightSide = new();

        for (int i = 0; i < nodeIds.Count - 1; i++)
        {
            if (!nodeDict.TryGetValue(nodeIds[i], out Vector2 a) ||
                !nodeDict.TryGetValue(nodeIds[i + 1], out Vector2 b))
                continue;

            Vector3 a3 = LatLonToWorld(a);
            Vector3 b3 = LatLonToWorld(b);

            Vector3 dir = (b3 - a3).normalized;
            Vector3 normal = Vector3.Cross(dir, Vector3.up);
            Vector3 offset = normal * (rule.width / 2f);

            leftSide.Add(a3 - offset);
            leftSide.Add(b3 - offset);
            rightSide.Add(a3 + offset);
            rightSide.Add(b3 + offset);
        }

        if (leftSide.Count < 2) return;

        Mesh mesh = new Mesh();
        List<Vector3> verts = new();
        List<int> tris = new();

        for (int i = 0; i < leftSide.Count; i += 2)
        {
            verts.Add(leftSide[i]);
            verts.Add(rightSide[i]);
            verts.Add(rightSide[i + 1]);
            verts.Add(leftSide[i + 1]);

            int idx = i * 2;

            tris.Add(idx);
            tris.Add(idx + 1);
            tris.Add(idx + 2);

            tris.Add(idx);
            tris.Add(idx + 2);
            tris.Add(idx + 3);
        }

        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();

        GameObject go = new GameObject($"Road_{rule.highwayType}");
        MeshFilter mf = go.AddComponent<MeshFilter>();
        mf.mesh = mesh;

        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        mr.material = rule.material;
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
