using UnityEngine;

public class Connection
{
    public MachineConnector from;
    public MachineConnector to;
    public GameObject visual;
    private Mesh mesh;
    private float width;

    public Connection(MachineConnector from, MachineConnector to, GameObject visual, float width)
    {
        this.from = from;
        this.to = to;
        this.visual = visual;
        this.mesh = visual.GetComponent<MeshFilter>().mesh;
        this.width = width;
    }

    public void UpdateMesh()
    {
        if (from == null || to == null || mesh == null) return;

        Vector3 a = from.connectionPoint.position;
        Vector3 b = to.connectionPoint.position;

        Vector3 dir = (b - a).normalized;
        Vector3 offset = Quaternion.AngleAxis(90f, Vector3.up) * dir * width;

        Vector3[] verts = new Vector3[]
        {
            a + offset, a - offset, b + offset, b - offset
        };

        int[] tris = new int[] { 0, 1, 2, 2, 1, 3 };
        Vector2[] uvs = new Vector2[]
        {
            new Vector2(0, 0), new Vector2(0, 1),
            new Vector2(1, 0), new Vector2(1, 1)
        };

        mesh.Clear();
        mesh.vertices = verts;
        mesh.triangles = tris;
        mesh.uv = uvs;
        mesh.RecalculateNormals();
    }
}