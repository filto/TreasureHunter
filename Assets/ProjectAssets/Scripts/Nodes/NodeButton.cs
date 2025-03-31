using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class NodeButton : MonoBehaviour
{
  public TextMeshProUGUI text;
  public Image icon;
  private NodeData nodeData;

  public void Initialize(NodeData currentnodeData)
  {
    nodeData = currentnodeData;
    icon.sprite = nodeData.icon;
    icon.color = nodeData.color;
    text.text = nodeData.nodeType;
    
  }

  public void OnClick()
  {
    GameManager.Instance.nodeMenu.GetComponent<NodeMenu>().ApplyNode(nodeData);
  }
}
