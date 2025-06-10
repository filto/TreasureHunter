using UnityEngine;using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildingVisualizer : MonoBehaviour
{
    public OSMDownloader downloader;
    public Material buildingMaterial;
    public bool use3DBuildings = true;
    public float randomMinHeight = 4f;
    public float randomMaxHeight = 20f;

    void Start()
    {
        /*StartCoroutine(downloader.DownloadOverpassData((response) =>
        {
            BuildBuildings(response);
        }));*/
    }
    
    void BuildBuildings(OSMDownloader.OverpassResponse response)
    {
        Dictionary<long, Vector2> nodes = new();

        foreach (var el in response.elements)
        {
            if (el.type == "node")
            {
                nodes[el.id] = new Vector2(el.lon, el.lat);
            }
        }

        foreach (var el in response.elements)
        {
            if (el.type == "way" && el.tags != null && el.tags.ContainsKey("building"))
            {
                List<Vector3> points = new();

                foreach (var id in el.nodes)
                {
                    if (nodes.TryGetValue(id, out Vector2 latlon))
                    {
                        points.Add(LatLonToWorld(latlon));
                    }
                }
                
                if (points.Count >= 4 && points[0] == points[^1])
                {
                    points.RemoveAt(points.Count - 1);
                }

                if (points.Count >= 3)
                {
                    Mesh mesh = use3DBuildings
                        ? ExtrudePolygon(points, GetBuildingHeight(el.tags))
                        : TriangulatePolygon(points);

                    GameObject go = new GameObject("Building");
                    var mf = go.AddComponent<MeshFilter>();
                    var mr = go.AddComponent<MeshRenderer>();
                    mf.mesh = mesh;
                    mr.material = buildingMaterial;
                }
            }
        }
    }  
    Mesh TriangulatePolygon(List<Vector3> points3D)
    {
        Vector2[] points2D = new Vector2[points3D.Count];
        for (int i = 0; i < points2D.Length; i++)
            points2D[i] = new Vector2(points3D[i].x, points3D[i].z);

        Triangulator tr = new Triangulator(points2D);
        int[] indices = tr.Triangulate();

        // 🔁 FLIPPA trianglar
        for (int i = 0; i < indices.Length; i += 3)
        {
            int temp = indices[i];
            indices[i] = indices[i + 1];
            indices[i + 1] = temp;
        }

        Mesh mesh = new Mesh();
        mesh.SetVertices(points3D);
        mesh.SetTriangles(indices, 0);
        mesh.RecalculateNormals();
        return mesh;
    }
    
    int[] TriangulateTop(List<Vector3> points3D, bool flip = false)
    {
        Vector2[] points2D = new Vector2[points3D.Count];
        for (int i = 0; i < points3D.Count; i++)
            points2D[i] = new Vector2(points3D[i].x, points3D[i].z);

        Triangulator tr = new Triangulator(points2D);
        int[] tris = tr.Triangulate();

        if (flip)
        {
            /*// Flippa ordning på trianglarna
            for (int i = 0; i < tris.Length; i += 3)
            {
                int tmp = tris[i];
                tris[i] = tris[i + 1];
                tris[i + 1] = tmp;
            }*/
        }

        return tris;
    }
    
    Mesh ExtrudePolygon(List<Vector3> basePolygon, float height)
    {
        bool flipWalls = IsPolygonClockwise(basePolygon);
        List<Vector3> vertices = new();
        List<int> triangles = new();

        int count = basePolygon.Count;

        // Lägg till tak
        int[] topTris = TriangulateTop(basePolygon, true);
        int topOffset = vertices.Count;
        vertices.AddRange(basePolygon.ConvertAll(p => p + Vector3.up * height));
        foreach (var t in topTris)
            triangles.Add(topOffset + t);

        // Lägg till väggar
        for (int i = 0; i < count; i++)
        {
            Vector3 a = basePolygon[i];
            Vector3 b = basePolygon[(i + 1) % count];
            Vector3 aTop = a + Vector3.up * height;
            Vector3 bTop = b + Vector3.up * height;

            int idx = vertices.Count;
            
            vertices.Add(a);
            vertices.Add(b);
            vertices.Add(bTop);
            vertices.Add(aTop);

            if (flipWalls)
            {
                // Flippa triangelordning
                triangles.Add(idx);
                triangles.Add(idx + 1);
                triangles.Add(idx + 2);

                triangles.Add(idx);
                triangles.Add(idx + 2);
                triangles.Add(idx + 3);
            }
            else
            {
                triangles.Add(idx);
                triangles.Add(idx + 2);
                triangles.Add(idx + 1);

                triangles.Add(idx);
                triangles.Add(idx + 3);
                triangles.Add(idx + 2);
            }
        }

        Mesh mesh = new Mesh();
        mesh.SetVertices(vertices);
        mesh.SetTriangles(triangles, 0);
        mesh.RecalculateNormals();
        return mesh;
    }
    
    float GetBuildingHeight(Dictionary<string, string> tags)
    {
        if (tags.TryGetValue("height", out string heightStr))
        {
            if (float.TryParse(heightStr, System.Globalization.NumberStyles.Float, System.Globalization.CultureInfo.InvariantCulture, out float h))
                return Mathf.Clamp(h, 3f, 100f);
        }

        if (tags.TryGetValue("building:levels", out string levelsStr))
        {
            if (int.TryParse(levelsStr, out int levels))
                return Mathf.Clamp(levels * 3f, 3f, 100f); // 3m per våning
        }

        // Fallback: slumpad höjd
        return Random.Range(randomMinHeight, randomMaxHeight);
    }
    
    Vector3 LatLonToWorld(Vector2 latlon)
    {
        float scale = 100000f;
        float x = (latlon.x /*- downloader.longitude*/) * scale;
        float z = (latlon.y /*- downloader.latitude*/) * scale;
        return new Vector3(x, 0, z);
    }
    
    bool IsPolygonClockwise(List<Vector3> points)
    {
        float sum = 0f;
        for (int i = 0; i < points.Count; i++)
        {
            Vector3 a = points[i];
            Vector3 b = points[(i + 1) % points.Count];
            sum += (b.x - a.x) * (b.z + a.z);
        }
        return sum > 0f;
    }
    
}
