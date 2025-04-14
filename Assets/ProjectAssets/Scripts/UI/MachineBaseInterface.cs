using UnityEngine;

public class MachineBaseInterface : MonoBehaviour
{
    public Interaction interaction;
    public GameObject machineMenu;
    
    private void OnEnable()
    {
        {
            interaction.OnClick += HandleClick; //Hantera klick
        }
    }
    
    void HandleClick(GameObject dragObject)
    {
        Instantiate(machineMenu, GameManager.Instance.canvas.transform);
    }
}
