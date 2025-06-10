using System.Collections.Generic;
using UnityEngine;

public class MeshBuilder
{
    public float width;

    private List<Vector3> verts = new();
    private List<int> tris = new();
    private int segmentCount = 0;

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