using UnityEngine;
using TMPro; 

public class HeadingAndAcceleration : MonoBehaviour
{
    private Vector3 gravity = Vector3.zero;
    private float filterFactor = 0.1f;
    public float heading = 0f;
    public float speed = 0f;
    public TextMeshProUGUI headingSpeedText;

    void Start()
    {
        Input.compass.enabled = true;
    }
   
    void Update()
    {
        heading = Input.compass.trueHeading;
        speed = GetMotionStrength(); 
        GPSManager.Instance.heading = heading;
        GPSManager.Instance.speed = speed;
        headingSpeedText.text = $"Speed: {speed:F2}Heading: {heading:F0}";
        
    }
    
    float GetMotionStrength()
    {
        Vector3 rawAccel = Input.acceleration;
        gravity = Vector3.Lerp(gravity, rawAccel, filterFactor);
        Vector3 motion = rawAccel - gravity;

        return motion.magnitude > 0.05f ? motion.magnitude : 0f; // tröskel för brus
    }
}
