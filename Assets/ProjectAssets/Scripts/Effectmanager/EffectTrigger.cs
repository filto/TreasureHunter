using UnityEngine;
using System;
using System.Collections.Generic;
using System.Reflection;

public class EffectTrigger : MonoBehaviour
{
    [Serializable]
    public class EffectBinding
    {
        public EffectData effect;
        public string triggerFunction;
        public MonoBehaviour targetScript;
        public GameObject targetObject;
    }

    public EffectBinding effectBinding;

    public List<string> availableEvents = new List<string>(); // 🔹 Lista över alla events i targetScript

    private void OnValidate()
    {
        UpdateAvailableEvents();
    }

    private void UpdateAvailableEvents()
    {
        availableEvents.Clear();

        if (effectBinding.targetScript != null)
        {
            Type scriptType = effectBinding.targetScript.GetType();
            EventInfo[] events = scriptType.GetEvents(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

            foreach (EventInfo evt in events)
            {
                availableEvents.Add(evt.Name);
            }
        }
    }
}