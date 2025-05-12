using UnityEngine;

public class ThemeSwitcher : MonoBehaviour
{
    [Header("Namn att växla mellan (måste vara exakt)")]
    public string groupAName = "GameCity";
    public string groupBName = "GameWestern";

    private bool usingGroupA = true;

    // Kallas från UI-knapp
    public void ToggleGroup()
    {
        usingGroupA = !usingGroupA;
        SetActiveGroup(usingGroupA);
    }

    void SetActiveGroup(bool enableGroupA)
    {
        var allObjects = GameObject.FindObjectsByType<GameObject>(FindObjectsSortMode.None);

        foreach (var obj in allObjects)
        {
            if (obj.name == groupAName)
                obj.SetActive(enableGroupA);
            else if (obj.name == groupBName)
                obj.SetActive(!enableGroupA);
        }
    }
}