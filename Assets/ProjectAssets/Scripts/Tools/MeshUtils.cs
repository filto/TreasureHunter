using System.Collections.Generic;
using UnityEngine;

public static class MeshUtils
{
    public static void WeldVerticesByThreshold(Mesh mesh, float threshold = 0.001f)
    {
        Vector3[] verts = mesh.vertices;
        int[] triangles = mesh.triangles;

        int vertexCount = verts.Length;
        bool[] visited = new bool[vertexCount];
        int[] vertexMap = new int[vertexCount];

        List<Vector3> newVerts = new List<Vector3>();
        Dictionary<int, List<int>> groups = new();

        for (int i = 0; i < vertexCount; i++)
        {
            if (visited[i]) continue;

            List<int> group = new() { i };
            visited[i] = true;

            Vector3 center = verts[i];

            for (int j = i + 1; j < vertexCount; j++)
            {
                if (!visited[j] && Vector3.Distance(verts[i], verts[j]) < threshold)
                {
                    group.Add(j);
                    visited[j] = true;
                }
            }

            // Räkna ut centerposition
            Vector3 avg = Vector3.zero;
            foreach (int idx in group)
                avg += verts[idx];
            avg /= group.Count;

            int newIndex = newVerts.Count;
            foreach (int idx in group)
                vertexMap[idx] = newIndex;

            newVerts.Add(avg);
        }

        // Remap triangles
        List<int> newTris = new();
        foreach (int oldIndex in triangles)
        {
            newTris.Add(vertexMap[oldIndex]);
        }

        mesh.Clear();
        mesh.vertices = newVerts.ToArray();
        mesh.triangles = newTris.ToArray();
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
    }
    
}

