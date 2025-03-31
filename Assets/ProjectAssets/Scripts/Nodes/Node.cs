using UnityEngine;
using System;

public class Node : MonoBehaviour
{
    public NodeData currentNodeData;
    public Interaction interaction;
    public GameObject iconObject;
    private SpriteRenderer spriteRenderer;
    public GameObject nodeBackground;
   
    private void OnEnable()
    {
        if (interaction != null)
        {
            interaction.OnDragEnd += HandleEndDrag; // ✅ Börja lyssna på OnDragEnd
            interaction.OnClick += HandleClick; //Hantera klick
        }
        
        spriteRenderer = iconObject.GetComponent<SpriteRenderer>();
    }
    
    // Flyttar node
    void MoveNode(Vector3 touchPosition)
    {
        //Debug.Log($"📌 MoveNode anropat från: {this.gameObject.name}"); // 🔍 Kolla vilket objekt som kallar MoveNode
        
        NodeManager.Instance.MoveNode(this,touchPosition); // Skicka direkt till NodeManager
    }
    
    // Tar bort node
    void DeleteNode(Vector3 touchPosition)
    {
        //Debug.Log($"📌 MoveNode anropat från: {this.gameObject.name}"); // 🔍 Kolla vilket objekt som kallar MoveNode
        
        NodeManager.Instance.DeleteNode(this,touchPosition); // Skicka direkt till NodeManager
    }
    
    public void ApplyNodeData(NodeData currentNodeData)
    {
        if (spriteRenderer == null)
        {
            //Debug.LogError($"❌ SpriteRenderer är NULL på {gameObject.name} vid ApplyNodeData!");
            return;
        }
        
        spriteRenderer.sprite = currentNodeData.icon; // Ändra sprite
        spriteRenderer.color = currentNodeData.color;   // Ändra färg
    }
    
    void HandleEndDrag(Vector3 touchPosition, GameObject hitObject, Vector3 startPosition, GameObject dragObject)
    {
        
        if (UIWrappers.DroppedOnTrashCan(touchPosition))
        {
            Debug.Log("💥 Släppt på papperskorgen!");
            Destroy(gameObject); // eller annan delete
            return;
        }
        
        MoveNode(touchPosition);  
    }
    
    void HandleClick(GameObject dragObject)
    {
        /*currentNodeData = NodeManager.Instance.GetNextNodeData(currentNodeData);
        ApplyNodeData(currentNodeData);*/
        
        GameManager.Instance.nodeMenu.GetComponent<NodeMenu>().OpenMenu(this);
        GameManager.Instance.interactionManager.SetActive(false);
        
    }

    public void SetValid(bool isValid)
    {
        if (isValid)
        {
            nodeBackground.SetActive(true);
        }
        
        if (!isValid)
        {
            nodeBackground.SetActive(false);
        }
    }
    
}