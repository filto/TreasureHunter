using UnityEditor;
using UnityEngine;
using DG.Tweening; // ✅ Lägg till DOTween

[CustomPropertyDrawer(typeof(EffectSteps))] // 🔗 Kopplar till EffectSteps
public class EffectStepsEditor : PropertyDrawer
{
    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);
        
        float yOffset = position.y; // 🔹 Startposition
        
        // 📌 Hämta fält
        SerializedProperty effectType = property.FindPropertyRelative("effectType");
        SerializedProperty targetObject = property.FindPropertyRelative("targetObject");
        SerializedProperty duration = property.FindPropertyRelative("duration");
        SerializedProperty startDelay = property.FindPropertyRelative("startDelay");
        SerializedProperty easeCurve = property.FindPropertyRelative("easeCurve");
        SerializedProperty startColor = property.FindPropertyRelative("startColor");
        SerializedProperty endColor = property.FindPropertyRelative("endColor");
        
        // 🎭 Dropdown för `effectType`
        Rect dropdownRect = new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(dropdownRect, effectType);
        yOffset += EditorGUIUtility.singleLineHeight + 5; // 🔹 Flytta ner
        
        // 🎯 Fält för `targetObject` (rad 2 – manuell höjd)
        Rect objectRect = new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(objectRect, targetObject);
        yOffset += EditorGUIUtility.singleLineHeight + 5; // 🔹 Flytta ner
        
        // ⏳ Fält för `duration`
        Rect durationRect = new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(durationRect, duration);
        yOffset += EditorGUIUtility.singleLineHeight + 5; // 🔹 Flytta ner
        
        // ⏳ Fält för `startDelay`
        Rect startDelayRect = new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight);
        EditorGUI.PropertyField(startDelayRect, startDelay);
        yOffset += EditorGUIUtility.singleLineHeight + 5; // 🔹 Flytta ner
        
        // 🔄 DOTween Ease Dropdown istället för Unity Curve
        SerializedProperty easeEnum = property.FindPropertyRelative("easeEnum"); // 🔥 Nytt Ease-fält
        Rect easeRect = new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight);
        //easeEnum.enumValueIndex = (int)(Ease)EditorGUI.EnumPopup(easeRect, "Ease Type", (Ease)easeEnum.enumValueIndex);
        yOffset += EditorGUIUtility.singleLineHeight + 5;
        
        if ((EffectType)effectType.enumValueIndex == EffectType.Fade)
        {
            EditorGUI.PropertyField(new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight), startColor);
            yOffset += EditorGUIUtility.singleLineHeight + 5;

            EditorGUI.PropertyField(new Rect(position.x, yOffset, position.width, EditorGUIUtility.singleLineHeight), endColor);
            yOffset += EditorGUIUtility.singleLineHeight + 5;
        }
        
        EditorGUI.EndProperty();
    }
    
    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int rows = 5; // 🔹 Grundläggande fält (effectType, targetObject, duration, startDelay, easeCurve)
        SerializedProperty effectType = property.FindPropertyRelative("effectType");

        if ((EffectType)effectType.enumValueIndex == EffectType.Fade)
        {
            rows += 2; // 🔹 Fade behöver startColor och endColor
        }

        if ((EffectType)effectType.enumValueIndex == EffectType.Move ||
            (EffectType)effectType.enumValueIndex == EffectType.Scale ||
            (EffectType)effectType.enumValueIndex == EffectType.Rotate)
        {
            rows += 4; // 🔹 Move/Scale/Rotate behöver startValue och endValue
        }

        return rows * (EditorGUIUtility.singleLineHeight + 5); // 🔹 Automatisk höjd baserad på rader
    }
}
