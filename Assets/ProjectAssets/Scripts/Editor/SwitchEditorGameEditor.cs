using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(SwitchEditorGame))]
public class SwitchEditorGameEditor : Editor
{
    public override void OnInspectorGUI()
    {
        // Standard Inspector först
        DrawDefaultInspector();

        SwitchEditorGame switcher = (SwitchEditorGame)target;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("Editor Controls", EditorStyles.boldLabel);

        if (GUILayout.Button("🔁 Switch to Game Mode"))
        {
            switcher.SwitchGameMode();
            
            ModeSwitcher[] allSwitchers = GameObject.FindObjectsByType<ModeSwitcher>(FindObjectsSortMode.None);
            foreach (var m in allSwitchers)
            {
                m.SetGameMode(true);
            }
        }

        if (GUILayout.Button("✏️ Switch to Editor Mode"))
        {
            switcher.SwitchEditorMode();
            
            ModeSwitcher[] allSwitchers = GameObject.FindObjectsByType<ModeSwitcher>(FindObjectsSortMode.None);
            foreach (var m in allSwitchers)
            {
                m.SetGameMode(false);
            }
        }
    }
}

