using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class NodeMenu : MonoBehaviour
{
    
    public GameObject buttonPrefab; // En UI-knapp prefab för att skapa menyknappar
    public Transform menuContainer; // Panelen där knapparna ska placeras
    public NodeDatabase nodeDatabase;
    private Node currentNode;
    
    public void GenerateMenu()
    {
        // 🧹 Clear existing buttons
        foreach (Transform child in menuContainer)
        {
            Destroy(child.gameObject);
        }
        
        foreach (var node in nodeDatabase.nodes)
        {
            GameObject newButton = Instantiate(buttonPrefab, menuContainer);
            NodeButton buttonScript = newButton.GetComponent<NodeButton>(); // Get the script

            /*buttonScript.text.text = node.nodeType;  // Set text
            buttonScript.icon.sprite = node.icon;    // Set icon
            buttonScript.icon.color = node.color;    // Set color*/
            
            buttonScript.Initialize(node);
        }
    }
    
    public void OpenMenu(Node node)
    {
        currentNode = node; // Store the node that opened the menu
        gameObject.SetActive(true); // Show the menu
        GenerateMenu();
    }

    public void ApplyNode(NodeData nodeData)
    {
        currentNode.ApplyNodeData(nodeData);
        OnExitMenuPress();
    }
    
    public void OnExitMenuPress()
    {
        GameManager.Instance.nodeMenu.SetActive(false);
        GameManager.Instance.interactionManager.SetActive(true);
    }
}
