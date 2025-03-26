using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using DG.DOTweenEditor;

[CustomEditor(typeof(AnimationSequencer))]
public class AnimationSequencerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        AnimationSequencer sequencer = (AnimationSequencer)target;
        serializedObject.Update();

        EditorGUILayout.LabelField("Animation Sequencer", EditorStyles.boldLabel);

        SerializedProperty steps = serializedObject.FindProperty("animationSteps");

        // 🔥 Hantera listan manuellt
        for (int i = 0; i < steps.arraySize; i++)
        {
            SerializedProperty step = steps.GetArrayElementAtIndex(i);
            SerializedProperty effectType = step.FindPropertyRelative("effectType");

            EditorGUILayout.BeginVertical("box");

            EditorGUILayout.PropertyField(effectType);

            EffectType selectedEffect = (EffectType)effectType.enumValueIndex;

            if (selectedEffect == EffectType.Fade)
            {
                SerializedProperty targetObject = step.FindPropertyRelative("targetObject");
                SerializedProperty duration = step.FindPropertyRelative("duration");
                SerializedProperty startDelay = step.FindPropertyRelative("startDelay");
                SerializedProperty startColor = step.FindPropertyRelative("startColor");
                SerializedProperty endColor = step.FindPropertyRelative("endColor");
                //SerializedProperty easeEnum = step.FindPropertyRelative("easeEnum");
                
                EditorGUILayout.PropertyField(targetObject);
                EditorGUILayout.PropertyField(duration);
                EditorGUILayout.PropertyField(startDelay);
                EditorGUILayout.PropertyField(startColor);
                EditorGUILayout.PropertyField(endColor);
                //EditorGUILayout.PropertyField(easeEnum);
            }
            else if (selectedEffect == EffectType.Move || selectedEffect == EffectType.Scale || selectedEffect == EffectType.Rotate)
            {
                SerializedProperty targetObject = step.FindPropertyRelative("targetObject");
                SerializedProperty startValue = step.FindPropertyRelative("startValue");
                SerializedProperty endValue = step.FindPropertyRelative("endValue");

                EditorGUILayout.PropertyField(targetObject);
                EditorGUILayout.PropertyField(startValue);
                EditorGUILayout.PropertyField(endValue);
            }

            // 🗑️ Ta bort knapp
            if (GUILayout.Button("Remove Effect"))
            {
                steps.DeleteArrayElementAtIndex(i);
            }

            EditorGUILayout.EndVertical();
            EditorGUILayout.Space();
        }

        // ➕ Lägg till ny effekt
        if (GUILayout.Button("Add Effect"))
        {
            steps.arraySize++;
        }

        serializedObject.ApplyModifiedProperties();
        
        if (GUILayout.Button("▶ Play Animation"))
        {
            sequencer.Play();
        }

        serializedObject.ApplyModifiedProperties();
    }
}
