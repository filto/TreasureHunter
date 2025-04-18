using UnityEngine;
using System;

public class SwitchEditorGame : MonoBehaviour
{
    public GameObject[] gameModeObjects;
    public GameObject[] editorModeObjects;
    public static bool CurrentModeIsGame = false;
    
    public static event Action<bool> OnModeChanged; // true = Game, false = Editor
    
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
        CurrentModeIsGame = true;
        OnModeChanged?.Invoke(true);
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
        CurrentModeIsGame = false;
        OnModeChanged?.Invoke(false);
    }
}
