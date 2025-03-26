using UnityEngine;

[CreateAssetMenu(fileName = "NewEffect", menuName = "Effects/EffectData")]
public class EffectData : ScriptableObject
{
    public string effectName;
    public EffectSteps[] effectSteps;
}