using UnityEngine;
using System;
using System.Collections.Generic;
using DG.Tweening; // ✅ Lägg till DOTween


public class AnimationSequencer : MonoBehaviour
{
    [Serializable]
    public class AnimationStep
    {
        public EffectType effectType;
        public GameObject targetObject;
        public float duration = 0f;
        public float startDelay = 0f;
        public Ease easeEnum = Ease.Linear;
        public Color startColor = Color.white;
        public Color endColor = new Color(1, 1, 1, 0);
        public Vector3 startValue;
        public Vector3 endValue;
    }

    public List<AnimationStep> animationSteps = new List<AnimationStep>(); // ✅ Lista med animationer
    
    public void Play()
    {
        Sequence sequence = DOTween.Sequence(); // 🔥 Skapa en sekvens för att stapla animationer

        foreach (var step in animationSteps)
        {
            if (step.targetObject == null) continue;

            Tween tween = null;

            switch (step.effectType)
            {
                case EffectType.Fade:
                    tween = ApplyFade(step);
                    break;
                case EffectType.Move:
                    tween = ApplyMove(step);
                    break;
                case EffectType.Scale:
                    tween = ApplyScale(step);
                    break;
                case EffectType.Rotate:
                    tween = ApplyRotate(step);
                    break;
            }

            if (tween != null)
            {
                sequence.Append(tween); // 🔥 Lägg till tweenen i sekvensen
            }
        }

        sequence.Play(); // 🚀 Starta sekvensen
    }

    private Tween ApplyFade(AnimationSequencer.AnimationStep step)
    {
        SpriteRenderer spriteRenderer = step.targetObject.GetComponent<SpriteRenderer>();
        if (spriteRenderer != null)
        {
            return spriteRenderer.DOColor(step.endColor, step.duration)
                .SetDelay(step.startDelay)
                .SetEase(step.easeEnum)
                .OnStart(() =>
                {
                    // 🔥 Sätter färgen först när tweenen faktiskt börjar, efter startDelay
                    spriteRenderer.color = step.startColor;
                });
        }
        return null;
    }

    private Tween ApplyMove(AnimationStep step)
    {
        //step.targetObject.transform.DOMove(step.endValue, step.duration)
            //.SetDelay(step.startDelay)
            //.SetEase(step.easeEnum);
            return null;
    }

    private Tween ApplyScale(AnimationStep step)
    {
        //step.targetObject.transform.DOScale(step.endValue, step.duration)
            //.SetDelay(step.startDelay)
            //.SetEase(step.easeEnum);
            return null;
    }

    private Tween ApplyRotate(AnimationStep step)
    {
        //step.targetObject.transform.DORotate(step.endValue, step.duration)
            //.SetDelay(step.startDelay)
            //.SetEase(step.easeEnum);
            return null;
    }
}