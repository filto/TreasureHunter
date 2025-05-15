using UnityEngine;
using System.Collections.Generic;

public class ThemeSwitcher : MonoBehaviour
{
    [Tooltip("Lista med teman (alla objekt med samma namn aktiveras när temat väljs)")]
    public List<string> themeNames = new List<string>();

    private int currentThemeIndex = 0;

    // Kallas från knapp (nästa tema)
    public void SwitchToNextTheme()
    {
        currentThemeIndex++;
        if (currentThemeIndex >= themeNames.Count)
            currentThemeIndex = 0;

        SetActiveTheme(themeNames[currentThemeIndex]);
    }

    // Kallas för att aktivera ett specifikt tema
    public void SetActiveTheme(string themeName)
    {
        GameObject[] allObjects = GameObject.FindObjectsOfType<GameObject>(true); // även inaktiva

        foreach (var obj in allObjects)
        {
            if (themeNames.Contains(obj.name))
            {
                obj.SetActive(obj.name == themeName);
            }
        }

        Debug.Log("Aktivt tema: " + themeName);
    }
    
    public void ActivateCurrentTheme()
    {
        if (currentThemeIndex >= themeNames.Count)
            currentThemeIndex = 0;

        SetActiveTheme(themeNames[currentThemeIndex]);
    }
}