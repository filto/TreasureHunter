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
        }

        if (GUILayout.Button("✏️ Switch to Editor Mode"))
        {
            switcher.SwitchEditorMode();
        }
    }
}

