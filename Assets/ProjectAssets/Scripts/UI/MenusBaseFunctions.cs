using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class MenuBaseFunctions : MonoBehaviour
{
    
    public void ExitMenuPress()
    {
        Destroy(gameObject);
    }
}