using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MenuBaseFunctions : MonoBehaviour
{
    public GameObject createMenuItem;
    
    public void ExitMenuPress()
    {
        Destroy(gameObject);
    }

    public void CreateMenuPress()
    {
        Instantiate(createMenuItem, GameManager.Instance.canvas.transform);
    }
}