using UnityEngine;

public class SetWorldParentOffset : MonoBehaviour
{
    public GameObject positioner;
    private Vector3 targetPosition;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void LateUpdate()
    { 
        targetPosition = positioner.transform.position;
        targetPosition = -targetPosition;
        GameManager.Instance.worldParent.transform.position = targetPosition;
        positioner.transform.position = new Vector3(0f, 0f, 0f);
    }
}
