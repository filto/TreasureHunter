using UnityEngine;

[RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
public class PathManager : MonoBehaviour
{
    public Transform[] points = new Transform[4]; // Dra in P0–P3 i editorn
    public float width = 0.1f;
    public float tilingFactor = 1;

    private Mesh mesh;

    void Start()
    {
        mesh = new Mesh();
        GetComponent<MeshFilter>().mesh = mesh;
        
        // Auto-assign material if none exists
        var renderer = GetComponent<MeshRenderer>();
        if (renderer.sharedMaterial == null)
        {
            Material mat = new Material(Shader.Find("Unlit/Color"));
            mat.color = Color.yellow;
            renderer.material = mat;
        }
    }

    void Update()
    {
        if (points.Length != 4 || points[0] == null || points[1] == null || points[2] == null || points[3] == null)
            return;

        UpdateMesh();
    }

    void UpdateMesh()
    {
        mesh.Clear();

        Vector3[] vertices = new Vector3[4 * 4];
        Vector2[] uvs = new Vector2[vertices.Length];
        int[] triangles = new int[4 * 6];

        for (int i = 0; i < 4; i++)
        {
            int next = (i + 1) % 4;

            Vector3 from = points[i].position;
            Vector3 to = points[next].position;

            // Steg 1: riktning från A till B
            Vector3 direction = (to - from).normalized;

            // Steg 2: rotera 90 grader runt Y-axeln för att få "bredd"-riktning
            Vector3 rotated = Quaternion.AngleAxis(90f, Vector3.up) * direction;

            // Steg 3: offset = bredd
            Vector3 offset = rotated * width;

            // Steg 4: skapa 4 vertexar, transformera till lokala koordinater
            int vi = i * 4;
            int ti = i * 6;

            vertices[vi + 0] = transform.InverseTransformPoint(from + offset);
            vertices[vi + 1] = transform.InverseTransformPoint(from - offset);
            vertices[vi + 2] = transform.InverseTransformPoint(to + offset);
            vertices[vi + 3] = transform.InverseTransformPoint(to - offset);

            // 2 trianglar per rektangel
            triangles[ti + 0] = vi + 0;
            triangles[ti + 1] = vi + 1;
            triangles[ti + 2] = vi + 2;

            triangles[ti + 3] = vi + 2;
            triangles[ti + 4] = vi + 1;
            triangles[ti + 5] = vi + 3;
            
            float length = Vector3.Distance(from, to);

            uvs[vi + 0] = new Vector2(0, 0);
            uvs[vi + 1] = new Vector2(0, 1);
            uvs[vi + 2] = new Vector2(length * tilingFactor, 0);
            uvs[vi + 3] = new Vector2(length * tilingFactor, 1);
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
}