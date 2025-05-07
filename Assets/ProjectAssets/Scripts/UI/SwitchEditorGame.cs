using UnityEngine;
using System;

public class SwitchEditorGame : MonoBehaviour
{
    public GameObject[] gameModeObjects;
    public GameObject[] editorModeObjects;
    public static bool CurrentModeIsGame = false;
    [Header("Ambient Lighting Colors")]
    [ColorUsage(true, true)]
    public Color gameModeAmbientColor = Color.white;
    [ColorUsage(true, true)]
    public Color editorModeAmbientColor = Color.gray;
    
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
        
        RenderSettings.ambientLight = gameModeAmbientColor;
        
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
        
        RenderSettings.ambientLight = editorModeAmbientColor;
        
        CurrentModeIsGame = false;
        OnModeChanged?.Invoke(false);
    }
}
