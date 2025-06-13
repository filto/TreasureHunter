using System.Collections.Generic;
using UnityEngine;

public struct EdgeData
{
    public Vector3 vertexA;
    public Vector3 vertexB;
    public int indexA;
    public int indexB;

    public Vector3 Midpoint => (vertexA + vertexB) * 0.5f;
}
public class MeshBuilder
{
    public float width;

    private List<Vector3> verts = new();
    private List<int> tris = new();
    private int segmentCount = 0;
    public List<EdgeData> edges = new List<EdgeData>();

    public MeshBuilder(float width)
    {
        this.width = width;
    }
    
    public void AddQuad(Vector3 leftA, Vector3 rightA, Vector3 leftB, Vector3 rightB)
    {
        int index = verts.Count;

        verts.Add(leftA);   // 0
        verts.Add(rightA);  // 1
        verts.Add(leftB);   // 2
        verts.Add(rightB);  // 3

        // Byt ordning för att få rätt normals
        tris.Add(index + 0);
        tris.Add(index + 1);
        tris.Add(index + 2);

        tris.Add(index + 2);
        tris.Add(index + 1);
        tris.Add(index + 3);
        
        AddEdge(index + 0, index + 1); // top edge: leftA → rightA
        AddEdge(index + 2, index + 3); // bottom edge: leftB → rightB
    }
    
    void AddEdge(int index1, int index2)
    {
        var v1 = verts[index1];
        var v2 = verts[index2];
    
        // Sortera för konsekvent riktning
        if (v1.sqrMagnitude > v2.sqrMagnitude)
            (v1, v2, index1, index2) = (v2, v1, index2, index1);

        edges.Add(new EdgeData
        {
            vertexA = v1,
            vertexB = v2,
            indexA = index1,
            indexB = index2
        });
    }
    
    public void WeldEdges(float threshold, float lengthTolerance, float directionThreshold)
    {
        for (int i = 0; i < edges.Count; i++)
        {
            var edge1 = edges[i];
            var mid1 = edge1.Midpoint;
            Vector3 a1 = verts[edge1.indexA];
            Vector3 b1 = verts[edge1.indexB];
            float length1 = Vector3.Distance(a1, b1);
            Vector3 dir1 = (b1 - a1).normalized;

            for (int j = i + 1; j < edges.Count; j++)
            {
                var edge2 = edges[j];
                var mid2 = edge2.Midpoint;

                if (Vector3.Distance(mid1, mid2) > threshold)
                    continue;

                Vector3 a2 = verts[edge2.indexA];
                Vector3 b2 = verts[edge2.indexB];
                float length2 = Vector3.Distance(a2, b2);

                if (Mathf.Abs(length1 - length2) > lengthTolerance)
                    continue;

                Vector3 dir2 = (b2 - a2).normalized;
                if (Mathf.Abs(Vector3.Dot(dir1, dir2)) < directionThreshold)
                    continue;

                // Matcha hörn korrekt
                float dAA = Vector3.Distance(a1, a2);
                float dAB = Vector3.Distance(a1, b2);

                if (dAA < dAB)
                {
                    Vector3 midA = (a1 + a2) * 0.5f;
                    Vector3 midB = (b1 + b2) * 0.5f;
                    verts[edge1.indexA] = verts[edge2.indexA] = midA;
                    verts[edge1.indexB] = verts[edge2.indexB] = midB;
                }
                else
                {
                    Vector3 midA = (a1 + b2) * 0.5f;
                    Vector3 midB = (b1 + a2) * 0.5f;
                    verts[edge1.indexA] = verts[edge2.indexB] = midA;
                    verts[edge1.indexB] = verts[edge2.indexA] = midB;
                }

                // Debug-visualisering (valfri)
                // Debug.DrawLine(mid1, mid2, Color.magenta, 5f);
            }
        }
    }



    public void DebugDrawMidpoints(float duration = 5f)
    {
        foreach (var edge in edges)
        {
            Vector3 midpoint = edge.Midpoint;
            Debug.DrawLine(midpoint + Vector3.up * 0.5f, midpoint - Vector3.up * 0.5f, Color.red, duration);
        }
    }

    public Mesh BuildMesh()
    {
        Mesh mesh = new Mesh();
        mesh.SetVertices(verts);
        mesh.SetTriangles(tris, 0);
        mesh.RecalculateNormals();
        return mesh;
    }
}