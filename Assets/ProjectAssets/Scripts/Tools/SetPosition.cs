using UnityEngine;

public class SetPosition : MonoBehaviour
{
    public GameObject target;
    public GameObject positioner;
    private Vector3 targetPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    { 
        targetPosition = positioner.transform.position;
        target.transform.position = targetPosition;
    }
}
