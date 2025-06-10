using System.Collections.Generic;
using UnityEngine;

public static class MeshOutlineUtility
{
    public static List<List<Vector2>> FindAllPolygonLoops(Mesh mesh, Transform tf)
    {
        var triangles = mesh.triangles;
        var verts = mesh.vertices;

        Dictionary<Edge, int> edgeCount = new();
        for (int i = 0; i < triangles.Length; i += 3)
        {
            AddEdge(edgeCount, new Edge(triangles[i], triangles[i + 1]));
            AddEdge(edgeCount, new Edge(triangles[i + 1], triangles[i + 2]));
            AddEdge(edgeCount, new Edge(triangles[i + 2], triangles[i]));
        }

        // Hitta konturkanter (anvands exakt en gang)
        List<Edge> outlineEdges = new();
        foreach (var kvp in edgeCount)
        {
            if (kvp.Value == 1)
                outlineEdges.Add(kvp.Key);
        }

        // Skapa kopplingsnätverk
        Dictionary<int, List<int>> connections = new();
        foreach (var edge in outlineEdges)
        {
            if (!connections.ContainsKey(edge.a)) connections[edge.a] = new();
            if (!connections.ContainsKey(edge.b)) connections[edge.b] = new();
            connections[edge.a].Add(edge.b);
            connections[edge.b].Add(edge.a);
        }

        // Bygg looper
        HashSet<Edge> usedEdges = new();
        List<List<Vector2>> loops = new();

        foreach (var edge in outlineEdges)
        {
            if (usedEdges.Contains(edge)) continue;

            List<Vector2> loop = new();
            int start = edge.a;
            int current = edge.b;

            loop.Add(ToV2(tf.TransformPoint(verts[start])));
            loop.Add(ToV2(tf.TransformPoint(verts[current])));

            usedEdges.Add(edge);
            int previous = start;

            while (current != start)
            {
                bool found = false;
                foreach (var next in connections[current])
                {
                    if (next == previous) continue;
                    var e = new Edge(current, next);
                    if (usedEdges.Contains(e)) continue;

                    loop.Add(ToV2(tf.TransformPoint(verts[next])));
                    usedEdges.Add(e);

                    previous = current;
                    current = next;
                    found = true;
                    break;
                }
                if (!found) break; // Avbruten loop
            }

            if (loop.Count >= 3)
                loops.Add(loop);
        }

        return loops;
    }

    private static Vector2 ToV2(Vector3 v3) => new Vector2(v3.x, v3.z);

    private static void AddEdge(Dictionary<Edge, int> dict, Edge edge)
    {
        if (dict.ContainsKey(edge)) dict[edge]++;
        else dict[edge] = 1;
    }

    private struct Edge
    {
        public int a, b;

        public Edge(int from, int to)
        {
            a = Mathf.Min(from, to);
            b = Mathf.Max(from, to);
        }

        public override bool Equals(object obj)
        {
            return obj is Edge other && a == other.a && b == other.b;
        }

        public override int GetHashCode()
        {
            return a * 73856093 ^ b * 19349663;
        }
    }
}

