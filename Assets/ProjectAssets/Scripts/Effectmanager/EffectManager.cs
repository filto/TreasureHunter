using UnityEngine;
//using DG.Tweening;
using System.Collections;

public class EffectManager : MonoBehaviour
{
    public static EffectManager Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }

    // 🔥 Spela upp en effekt och ersätt `targetObject` om det skickas in
    public void PlayEffect(EffectData effectData, GameObject overrideTarget = null)
    {
        foreach (EffectSteps step in effectData.effectSteps)
        {
            StartCoroutine(PlayEffectWithDelay(step, overrideTarget));
        }
    }

    private IEnumerator PlayEffectWithDelay(EffectSteps effect, GameObject overrideTarget)
    {
        yield return new WaitForSeconds(effect.startDelay); // ⏳ Vänta på startDelay

        // 🚀 Ersätt `targetObject` om vi fått ett nytt i anropet
        if (overrideTarget != null)
        {
            effect.targetObject = overrideTarget;
        }

        ApplyEffect(effect);
    }

    // 🔹 Hantera olika EffectTypes
    private void ApplyEffect(EffectSteps effect)
    {
        if (effect.targetObject == null)
        {
            Debug.LogWarning("EffectManager: Ingen targetObject angiven.");
            return;
        }

        switch (effect.effectType)
        {
            case EffectType.Fade:
                ApplyFadeEffect(effect);
                break;

            case EffectType.Move:
                ApplyMoveEffect(effect);
                break;

            case EffectType.Scale:
                ApplyScaleEffect(effect);
                break;

            case EffectType.Rotate:
                ApplyRotateEffect(effect);
                break;

            default:
                Debug.LogWarning($"EffectManager: Ingen implementation för EffectType {effect.effectType}");
                break;
        }
    }

    // 🔥 Fade-effekt
    private void ApplyFadeEffect(EffectSteps effect)
    {
        SpriteRenderer spriteRenderer = effect.targetObject.GetComponent<SpriteRenderer>();

        if (spriteRenderer != null)
        {
            // 🔥 Multiplicera färger med originalfärgen
            Color originalColor = spriteRenderer.color;
            Color startColor = new Color(
                originalColor.r * effect.startColor.r,
                originalColor.g * effect.startColor.g,
                originalColor.b * effect.startColor.b,
                originalColor.a * effect.startColor.a
            );

            Color endColor = new Color(
                originalColor.r * effect.endColor.r,
                originalColor.g * effect.endColor.g,
                originalColor.b * effect.endColor.b,
                originalColor.a * effect.endColor.a
            );

            /*// 🟡 Tweena färg med multiplicerade värden
            spriteRenderer.color = startColor; // Sätt startfärgen direkt
            spriteRenderer.DOColor(endColor, effect.duration)
                .SetEase(effect.easeEnum);*/
        }
        else
        {
            Debug.LogWarning($"EffectManager: {effect.targetObject.name} har ingen SpriteRenderer.");
        }
    }

    // 🔄 Move-effekt
    private void ApplyMoveEffect(EffectSteps effect)
    {
        /*effect.targetObject.transform.DOMove(effect.endValue, effect.duration)
            .SetEase(effect.easeEnum);*/
    }

    // 🔄 Scale-effekt
    private void ApplyScaleEffect(EffectSteps effect)
    {
        /*effect.targetObject.transform.DOScale(effect.endValue, effect.duration)
            .SetEase(effect.easeEnum);*/
    }

    // 🔄 Rotate-effekt
    private void ApplyRotateEffect(EffectSteps effect)
    {
        /*effect.targetObject.transform.DORotate(effect.endValue, effect.duration)
            .SetEase(effect.easeEnum);*/
    }
}
