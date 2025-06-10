using UnityEditor;
using UnityEngine;

[CustomPropertyDrawer(typeof(RoadRenderRule))]
public class RoadRenderRuleDrawer : PropertyDrawer
{
    string[] highwayTypes = new string[]
    {
        "motorway", "trunk", "primary", "secondary", "tertiary",
        "unclassified", "residential", "service",
        "footway", "path", "cycleway", "pedestrian", "track", "steps",
        "living_street", "construction", "road", "sidewalk"
    };

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        Rect foldoutRect = new Rect(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);
        property.isExpanded = EditorGUI.Foldout(foldoutRect, property.isExpanded, label, true);

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;
            float y = foldoutRect.y + EditorGUIUtility.singleLineHeight + 2f;

            var typeProp = property.FindPropertyRelative("highwayType");
            var widthProp = property.FindPropertyRelative("width");
            var materialProp = property.FindPropertyRelative("material");

            int selected = Mathf.Max(0, System.Array.IndexOf(highwayTypes, typeProp.stringValue));
            selected = EditorGUI.Popup(new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight), "Highway Type", selected, highwayTypes);
            typeProp.stringValue = highwayTypes[selected];
            y += EditorGUIUtility.singleLineHeight + 2f;

            EditorGUI.PropertyField(new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight), widthProp);
            y += EditorGUIUtility.singleLineHeight + 2f;

            EditorGUI.PropertyField(new Rect(position.x, y, position.width, EditorGUIUtility.singleLineHeight), materialProp);

            EditorGUI.indentLevel--;
        }

        EditorGUI.EndProperty();
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        if (!property.isExpanded) return EditorGUIUtility.singleLineHeight;
        return EditorGUIUtility.singleLineHeight * 4 + 8f;
    }
}