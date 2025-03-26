using UnityEngine;

public class EffectTester : MonoBehaviour
{
    public AnimationSequencer animationSequencer;

    private void Start()
    {
            Debug.Log("Spelar effekt på customTarget...");
            animationSequencer.Play();
    }
}