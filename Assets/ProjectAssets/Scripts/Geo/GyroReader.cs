using UnityEngine;
using TMPro; 

public class GyroReader : MonoBehaviour
{
    public TextMeshProUGUI gyroText;
    private float filterFactor = 0.1f; 
    private Vector3 gravity = Vector3.zero;

    void Start()
    {
        
        Input.compass.enabled = true;
    }

    void Update()
    {
            Vector3 rawAccel = Input.acceleration;
            gravity = Vector3.Lerp(gravity, rawAccel, filterFactor);
            Vector3 motion = rawAccel - gravity;
            
            float heading = Input.compass.trueHeading;
            
            gyroText.text = $"Rotation: {heading:F0}: Motion: {motion.magnitude:F2}";

        
    }
}