using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class NodeManager : MonoBehaviour
{
    public static NodeManager Instance { get; private set; }  // Singleton-instans
    public List<NodeData> availableNodeTypes;
    
    // ✅ Lagrar noder baserat på deras gridposition
    private Dictionary<Vector2Int, Node> nodeGrid = new Dictionary<Vector2Int, Node>();
    
    //Lagrar kopplingar
    private Dictionary<(Node, Node), (GameObject connectionObject, bool isGhost)> connections = new Dictionary<(Node, Node), (GameObject, bool)>();
    public NodeData StartNode; 
    public NodeData StopNode;
    public Node stopNodeInstance;
    public Node startNodeInstance;
    private static int nodeCounter = 0;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);  // Förhindra att flera instanser skapas
        }
    }

    private void Start()
    {
        InitializeNodes();
    }
    #region Initialisera
    public void InitializeNodes()
    {
        CreateStartNode();
        CreateStopNode();
    }

    public void CreateStartNode()
    {
        Vector2Int startNodePos = new Vector2Int(0, 0);
        GameObject newNode = Instantiate(GameManager.Instance.nodePrefab, new Vector3(startNodePos.x, startNodePos.y, 0), Quaternion.identity);
        Node node = newNode.GetComponent<Node>();
        newNode.name = "StartNode";
        startNodeInstance = node;
        node.ApplyNodeData(StartNode);
        
        // 🚫 Gör så att Startnoden inte kan dras
        node.GetComponent<Interaction>().isDraggable = false;
        AddNodeInGrid(node, startNodePos);
    }
    
    public void CreateStopNode()
    {
        Vector2Int stopNodepos = new Vector2Int(0, 1);
        GameObject newNode = Instantiate(GameManager.Instance.nodePrefab, new Vector3(stopNodepos.x, stopNodepos.y, 0), Quaternion.identity);
        Node node = newNode.GetComponent<Node>();
        newNode.name = "StopNode";
        stopNodeInstance = node;
        node.ApplyNodeData(StopNode);
        AddNodeInGrid(node, stopNodepos);
        CheckConnections(node);
    }
    
    #endregion

    #region Hantera Nodplaceringar
    
    public void PlaceNode(Vector3 position)
    {
        // Snappa till gridden
        Vector2Int gridPosition = SnapToGrid(position);
        
        // 🚫 Kolla om platsen är upptagen
        if (IsPositionOccupied(gridPosition))
        {
           //Debug.LogWarning($"❌ plats {gridPosition} är redan upptagen!");
           Vector2Int pushDirection = GetPushDirection(position);
           if (pushDirection == Vector2Int.left)
           {
               gridPosition.x += 1;
           }
           
           PushNodes(gridPosition, pushDirection);
        }

        // 🆕 Skapa en ny nod från prefab
        GameObject newNode = Instantiate(GameManager.Instance.nodePrefab, new Vector3(gridPosition.x, gridPosition.y, 0), Quaternion.identity);
        nodeCounter++;
        newNode.name = $"Node_{nodeCounter}";
        Node node = newNode.GetComponent<Node>();

        // ✅ Lägg till noden i NodeManager
        AddNodeInGrid(node, gridPosition);

        //Debug.Log($"✅ Ny nod skapad på {gridPosition}");
        
        CheckConnections(node);
        
        // 📌 Logga alla kopplingar
        PrintAllConnections();
    }
    
    public void MoveNode(Node node, Vector3 position)
    {
        Vector2Int gridPosition = SnapToGrid(position);
        
        // 🚫 Kolla om platsen är upptagen
        if (IsPositionOccupied(gridPosition))
        {
            //Debug.LogWarning($"plats upptagen");
            
            // 🔄 Återställ noden till sin registrerade position
            Vector2Int registeredGridPos = GetRegisteredNodePosition(node);
            node.transform.position = new Vector3(registeredGridPos.x, registeredGridPos.y, position.z);
            
            return; // ⛔ Stoppa om platsen är upptagen
        }
        
        // 🗑️ 1. Ta bort gamla kopplingar innan noden flyttas
        RemoveAllConnections(node);
        
        // ✅ Uppdatera nodens nya position
        node.transform.position = new Vector3(gridPosition.x, gridPosition.y, 0); //Flytta nod fysiskt
        RemoveNodeInGrid(node);                 //Ta bort nod från gamla platsen
        AddNodeInGrid(node, gridPosition);     // Lägg till nod på ny plats
        CheckConnections(node);
        CheckIfValid();
    }
    
    public void DeleteNode(Node node, Vector3 position)
    {
        RemoveAllConnections(node);
        RemoveNodeInGrid(node);
        Destroy(node.gameObject);
    }
    
    private void PushNodes(Vector2Int startPosition, Vector2Int pushDirection)
    {
        List<Node> nodesToMove = new List<Node>();
        Debug.Log($"PusharNodes");
        
        // 🔄 Flytta noder beroende på riktning
        if (pushDirection == Vector2Int.up || pushDirection == Vector2Int.down)
        {
            //Debug.Log($"PusharNodes Right");
            
            foreach (var entry in nodeGrid)
            {
                Vector2Int nodePos = entry.Key;

                if (nodePos.y >= startPosition.y)
                {
                    nodesToMove.Add(entry.Value);
                    //Debug.Log($"Lagt till i listan");
                }
            }

            // ✅ Sortera från högsta x-värdet först (höger → vänster)
            nodesToMove.Sort((a, b) => GetRegisteredNodePosition(b).y.CompareTo(GetRegisteredNodePosition(a).y));
            
            // Börja längst till vänster
            foreach (var node in nodesToMove)
            {
                Vector3 newPosition = new Vector3(node.transform.position.x, node.transform.position.y+1, 0);

                // ✅ Anropa MoveNode() som vi redan har!
                MoveNode(node, newPosition);
            }
        }
        
    }
    
    #endregion
   
    #region Hantera Grid-registreringar
    public void AddNodeInGrid(Node node, Vector2Int position)
    {
            nodeGrid[position] = node;

            //Debug.Log($"🗂️ Nod tillagd i manager på position {position}");
        
        // 📝 Skriv ut alla noder i dictionaryn
        //PrintAllNodes();
    }

    // ❌ Ta bort en nod
    public void RemoveNodeInGrid(Node node)
    {
        // 🔍 Leta upp noden i `nodeGrid`
        foreach (var kvp in nodeGrid)
        {
            if (kvp.Value == node) // 🟢 Om vi hittar noden
            {
                nodeGrid.Remove(kvp.Key); // 🗑️ Ta bort den från dictionaryn
                //Debug.Log($"🗑️ Nod borttagen från {kvp.Key}");
                //PrintAllNodes(); // Debugga efter borttagning
                return; // ⏹️ Avsluta loopen så vi inte kör vidare i onödan
            }
        }
    }
    
    #endregion
    
    #region nodkopplingar
    
    public void CheckConnections(Node node)
    {
        
        // 🔍 Lista med alla möjliga riktningar (upp, ner, vänster, höger)
        Vector2Int[] directions = { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };

        foreach (Vector2Int dir in directions)
        {
            Vector2Int nodePosition = GetRegisteredNodePosition(node);  // ✅ Hämta nodens position från NodeManager
            Vector2Int neighborPos = nodePosition + dir;      // ✅ Använd rätt position
        
            if (nodeGrid.TryGetValue(neighborPos, out Node neighbor))
            {
                AddConnection(node, neighbor);
            }
        }
    }
    
    public void AddConnection(Node nodeA, Node nodeB, bool isGhost = false)
    {
        if (connections.ContainsKey((nodeA, nodeB)) || connections.ContainsKey((nodeB, nodeA)))
        {
            return;
        }
        
        connections[(nodeA, nodeB)] = (null, isGhost); 
        CreateConnectionPrefab(nodeA, nodeB, isGhost);
        //Invoke("CheckIfValid", 0.1f);
        PrintAllConnections();
        CheckIfValid();
    }
    
    
    private void CreateConnectionPrefab(Node fromNode, Node toNode, bool isGhost)
    {
        
        // 🏗️ Skapa en koppling mellan noderna
        GameObject connection = Instantiate(GameManager.Instance.connectionPrefab, Vector3.zero, Quaternion.identity);
        
        // 📍 Beräkna mittpunkten mellan noderna
        Vector3 midPoint = (fromNode.transform.position + toNode.transform.position) / 2f;
        connection.transform.position = midPoint + new Vector3(0.5f,0.5f,-0.5f);
    
        // 🔄 Rotera kopplingen så att den pekar mot rätt nod
        Vector2 direction = toNode.transform.position - fromNode.transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        connection.transform.rotation = Quaternion.Euler(0, 0, angle);
        
        // 🗂️ Spara kopplingen i dictionary
        connections[(fromNode, toNode)] = (connection, isGhost);

        connection.GetComponent<Connector>().setConnectorType(isGhost);
    
        //Debug.Log($"🔗 Visuell koppling skapad mellan {fromNode.name} och {toNode.name}");
    }
    
    public void RemoveConnection(Node fromNode, Node toNode)
    {

        // 🗑️ Ta bort kopplingen om den finns
        if (connections.TryGetValue((fromNode, toNode), out (GameObject connectionObject, bool isGhost) connectionData))
        {
            Destroy(connectionData.connectionObject);// Ta bort den visuella kopplingen
            connections.Remove((fromNode, toNode)); // Ta bort kopplingen från dictionaryn

            //Debug.Log($"🗑️ Koppling mellan {fromNode.name} och {toNode.name} borttagen!");
        }
        else
        {
            Debug.LogWarning($"⚠️ Ingen koppling hittades mellan {fromNode.name} och {toNode.name}.");
        }
        
    }
    
    private void RemoveAllConnections(Node node)
    {
        List<(Node, Node)> connectionsToRemove = new List<(Node, Node)>();

        foreach (var connection in connections.Keys)
        {
            if (connection.Item1 == node || connection.Item2 == node)
            {
                connectionsToRemove.Add(connection);
            }
        }

        foreach (var connection in connectionsToRemove)
        {
            if (connections[connection].connectionObject != null) // Kolla om det visuella objektet finns
            {
                Destroy(connections[connection].connectionObject); // ❌ Ta bort kopplingsobjektet från scenen
            }
            connections.Remove(connection);
            Debug.Log($"🗑️ Koppling borttagen: {connection.Item1.name} ↔ {connection.Item2.name}");
        }
    }


    
    #endregion

    #region stödfunktioner
    // 🚫 Kolla om en position är upptagen
    public bool IsPositionOccupied(Vector2Int position)
    {
        bool occupied = nodeGrid.ContainsKey(position);
        //Debug.Log($"🔍 Kollar position {position}: " + (occupied ? "UPPTAGEN" : "LEDIG"));
        return occupied;
    }
   
    //Hämta en nod vid en specifik position
    public Vector2Int GetRegisteredNodePosition(Node node)
    {
        foreach (var kvp in nodeGrid)
        {
            if (kvp.Value == node) // 🔍 Hittar noden i dictionaryn
            {
                return kvp.Key;
            }
        }

        Debug.LogWarning($"⚠️ Nod {node.name} har ingen registrerad position i NodeManager!");
        return Vector2Int.zero; // 🔄 Om noden saknas, returnera (0,0)
    }
    
    public Vector2Int SnapToGrid(Vector3 position)
    {
        int x = Mathf.FloorToInt(position.x);
        int y = Mathf.FloorToInt(position.y);
        return new Vector2Int(x, y);
    }
    
    public (Node, Node)? GetNodesFromConnection(GameObject connectionObject)
    {
        foreach (var entry in connections)
        {
            if (entry.Value.connectionObject == connectionObject)
            {
                return (entry.Key.Item1, entry.Key.Item2); // 🏗️ Returnera noderna som är kopplade
            }
        }

        return null; // ❌ Hittade ingen koppling
    }
    
    private Vector2Int GetPushDirection(Vector3 position)
    {
        // 🔍 Hämta decimaldelarna
        float xDecimal = position.x - Mathf.Floor(position.x);
        float yDecimal = position.y - Mathf.Floor(position.y);

        // ✅ Välj den största decimalen och bestäm riktning
        if (Mathf.Abs(xDecimal - 0.5f) > Mathf.Abs(yDecimal - 0.5f))
        {
            return xDecimal > 0.5f ? Vector2Int.left : Vector2Int.right;
        }
        else
        {
            return yDecimal > 0.5f ? Vector2Int.down : Vector2Int.up;
        }
    }
    
    #endregion
    
    #region NodTyper
    
    public NodeData GetNextNodeData(NodeData currentData)
    {
        if (availableNodeTypes.Count == 0) return currentData;

        int currentIndex = availableNodeTypes.IndexOf(currentData);
        int nextIndex = (currentIndex + 1) % availableNodeTypes.Count; // Loopar tillbaka
        return availableNodeTypes[nextIndex];
    }
    #endregion
    
    #region Debugging
    private void PrintAllNodes()
    {
        Debug.Log("📌 Alla noder i NodeManager:");
        foreach (var entry in nodeGrid)
        {
            Debug.Log($"📍 Position: {entry.Key} | Nod: {entry.Value.name}");
        }
    }
    
    public void PrintAllConnections()
    {
        Debug.Log("📌 Alla kopplingar i NodeManager:");

        foreach (var connection in connections)
        {
            Node nodeA = connection.Key.Item1;
            Node nodeB = connection.Key.Item2;
            Debug.Log($"🔗 Koppling mellan {nodeA.name} ↔ {nodeB.name}");
        }
    }

    #endregion

    public void CheckIfValid()
    {
        if (startNodeInstance == null || stopNodeInstance == null)
        {
            Debug.LogError("❌ StartNode eller StopNode saknas!");
            return;
        }
    
        // 🔄 STEP 0: Reset all nodes to invalid
        foreach (var node in nodeGrid.Values)
        {
            node.SetValid(false); // 🚫 Reset all nodes before checking
        }
    
        // 🛠 Create HashSets to track nodes
        HashSet<Node> toCheck = new HashSet<Node>(); // Nodes to process
        HashSet<Node> checkedNodes = new HashSet<Node>(); // Nodes already processed
        HashSet<Node> processedNodes = new HashSet<Node>();
        
        // 🔹 STEP 1: Add StopNode to toCheck
        toCheck.Add(stopNodeInstance);
        
        Debug.Log("✅ Step 1: Initialized toCheck with StopNode.");
        
        int safeCounter=0;
        
        while (toCheck.Count > 0)
        {
            Node currentNode = null;
            
            safeCounter++;
            
            foreach (Node node in toCheck)
            {
                currentNode = node;
                break; // Get only one node and exit loop
            }
            
            toCheck.Remove(currentNode); // Remove from toCheck since we're processing it
            checkedNodes.Add(currentNode); // Mark it as checked
            
            foreach (var connection in connections)
            {
                if (connection.Key.Item2 == currentNode) // 🔍 If currentNode is the destination
                {
                    Node incomingNode = connection.Key.Item1; // Get the node that leads into StopNode
                    
                    if (connection.Value.isGhost) continue;
                    
                    if (!toCheck.Contains(incomingNode) && !checkedNodes.Contains(incomingNode)) // 🛑 Avoid duplicates
                    {
                        toCheck.Add(incomingNode);
                        Debug.Log($"🔄 Added {incomingNode.name} to toCheck (leads to StopNode).");
                    }
                }
            }

            foreach (Node node in toCheck)
            {
                Debug.Log($"➡️ {node.name} is in toCheck");
            }
            
            foreach (Node node in checkedNodes)
            {
                Debug.Log($"➡️ {node.name} is in checkedNodes");
            }
            
            if (safeCounter > 5000)
            {
                Debug.Log("Went over safecounter");
                return;
            }
            
        }
        
        // 🔹 STEP 6: Add StartNode to toCheck
        toCheck.Add(startNodeInstance);
        Debug.Log($"✅ Step 6: Added StartNode ({startNodeInstance.name}) to toCheck.");
        
        safeCounter=0;
        
        // 🔄 Process nodes in toCheck one by one
        while (toCheck.Count > 0)
        {
            Node currentNode = null;
            foreach (Node node in toCheck)
            {
                currentNode = node;
                break;
            }
            
            safeCounter++;
            toCheck.Remove(currentNode); // Remove from toCheck since we're processing it
            processedNodes.Add(currentNode);

            // ✅ Find all nodes in `checkedNodes` that currentNode can reach
            foreach (var connection in connections)
            {
                if (connection.Key.Item1 == currentNode) // 🔍 If currentNode has an outgoing connection
                {
                    Node connectedNode = connection.Key.Item2;
                    
                    if (connection.Value.isGhost) continue;
                    
                    if (!toCheck.Contains(connectedNode) && checkedNodes.Contains(connectedNode) && !processedNodes.Contains(connectedNode)) // ✅ Only check valid nodes
                    {
                        toCheck.Add(connectedNode);
                        connectedNode.SetValid(true); // 🏆 Mark as valid
                        Debug.Log($"🔍 Checking if {connectedNode.name} should be valid. " +
                                  $"Is in checkedNodes: {checkedNodes.Contains(connectedNode)}, " +
                                  $"Has outgoing connection from StartNode: {connections.ContainsKey((startNodeInstance, connectedNode))}");
                    }
                }
            }

            if (safeCounter > 5000)
            {
                Debug.Log("Went over safecounter");
                return;
            }
        }
    }
}