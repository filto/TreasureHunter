using UnityEngine;

public class SwitchEditorGame : MonoBehaviour
{
    public GameObject[] gameModeObjects;
    public GameObject[] editorModeObjects;
    
    
    public void SwitchGameMode()
    {
        for (int i = 0; i < gameModeObjects.Length; i++)
        {
            if (gameModeObjects[i] != null)
                gameModeObjects[i].SetActive(true);
        }

        for (int i = 0; i < editorModeObjects.Length; i++)
        {
            if (editorModeObjects[i] != null)
                editorModeObjects[i].SetActive(false);
        }
    }
    
    public void SwitchEditorMode()
    {
        for (int i = 0; i < editorModeObjects.Length; i++)
        {
            if (editorModeObjects[i] != null)
                editorModeObjects[i].SetActive(true);
        }
        
        for (int i = 0; i < gameModeObjects.Length; i++)
        {
            if (gameModeObjects[i] != null)
                gameModeObjects[i].SetActive(false);
        }
        
    }
}
