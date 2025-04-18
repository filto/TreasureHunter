using UnityEngine;

public class ModeSwitcher : MonoBehaviour
{
    public GameObject[] gameModeObjects;
    public GameObject[] editorModeObjects;
    public Component[] gameModeComponents;
    public Component[] editorModeComponents;
    
    private void OnEnable()
    {
        SwitchEditorGame.OnModeChanged += SetGameMode;
    }

    private void OnDisable()
    {
        SwitchEditorGame.OnModeChanged -= SetGameMode;
    }
    
    private void SetComponentState(Component comp, bool state)
    {
        if (comp == null) return;

        var type = comp.GetType();
        var enabledProperty = type.GetProperty("enabled");

        if (enabledProperty != null && enabledProperty.PropertyType == typeof(bool))
        {
            enabledProperty.SetValue(comp, state, null);
        }
    }

// 🔥 Den här metoden körs automatiskt när eventet skickas!
    public void SetGameMode(bool isGameMode)
    {
        foreach (var obj in gameModeObjects)
            if (obj != null) obj.SetActive(isGameMode);

        foreach (var obj in editorModeObjects)
            if (obj != null) obj.SetActive(!isGameMode);

// Scripts / Komponenter
        foreach (var comp in gameModeComponents)
            SetComponentState(comp, isGameMode);

        foreach (var comp in editorModeComponents)
            SetComponentState(comp, !isGameMode);
    }
}
