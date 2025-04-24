// MachineConnectorManager.cs
using System.Collections.Generic;
using UnityEngine;

public class MachineConnectorManager : MonoBehaviour
{
    public static MachineConnectorManager Instance { get; private set; }
    
    private List<Connection> connections = new List<Connection>();
    public Material connectionMaterial;
    public float lineWidth = 0.1f;
    public int connectionLayer = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    private void Update()
    {
        foreach (var conn in connections)
        {
            conn.UpdateMesh();
        }
    }

    public void CreateConnection(MachineConnector from, MachineConnector to)
    {
        if (ConnectionExists(from, to)) return;
        
        GameObject visual = new GameObject("ConnectionVisual");
        visual.AddComponent<MeshFilter>().mesh = new Mesh();
        var renderer = visual.AddComponent<MeshRenderer>();
        renderer.material = connectionMaterial;
        visual.layer = connectionLayer;

        var conn = new Connection(from, to, visual, lineWidth);
        connections.Add(conn);
    }
    
    public void RemoveConnection(MachineConnector target)
    {
        // Hitta alla kopplingar där target är to och ta bort
        for (int i = connections.Count - 1; i >= 0; i--)
        {
            if (connections[i].to == target)
            {
                Destroy(connections[i].visual);
                connections.RemoveAt(i);
            }
        }
    }
    
    public bool ConnectionExists(MachineConnector a, MachineConnector b)
    {
        foreach (var conn in connections)
        {
            if ((conn.from == a && conn.to == b) || (conn.from == b && conn.to == a))
                return true;
        }
        return false;
    }
}