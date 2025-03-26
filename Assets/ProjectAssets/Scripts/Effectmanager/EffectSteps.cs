using UnityEngine;
//using DG.Tweening; // ✅ Lägg till DOTween

public enum EffectType
{
    Fade,
    Move,
    Scale,
    Rotate
}

[System.Serializable]
public class EffectSteps
{
    public EffectType effectType; // Välj typ av effekt
    
    public GameObject targetObject;
    public float duration = 0f;
    public float startDelay = 0f;
    //public Ease easeEnum = Ease.Linear;
    public Color startColor = Color.white;
    public Color endColor = new Color(1, 1, 1, 0);
    public Vector3 startValue;
    public Vector3 endValue;
}