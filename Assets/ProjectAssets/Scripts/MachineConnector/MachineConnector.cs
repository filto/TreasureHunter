using UnityEngine;

public class MachineConnector : MonoBehaviour
{
    public string machineID;
    public Transform connectionPoint;
    private MachineConnector dummyConnector;

    private void Reset()
    {
        if (connectionPoint == null)
            connectionPoint = this.transform;
    }

    public void OnConnected(MachineConnector other)
    {
        Debug.Log($"{name} connected to {other.name}");
    }

    public void OnTouchEvent(TouchData touchData)
    {
        switch (touchData.phase)
        {
            case TouchPhase.Began:
                dummyConnector = CreateMouseFollower();
                MachineConnectorManager.Instance.CreateConnection(this, dummyConnector);
                dummyConnector.transform.position = GetMouseOnConnectorPlane(touchData.screenPosition);
                break;

            case TouchPhase.Moved:
                if (dummyConnector != null)
                {
                    dummyConnector.transform.position = GetMouseOnConnectorPlane(touchData.screenPosition);
                }
                break;

            case TouchPhase.Ended:
                if (dummyConnector != null)
                {
                    MachineConnectorManager.Instance.RemoveConnection(dummyConnector);
                    Destroy(dummyConnector.gameObject);
                    
                    if (touchData.hitObject != null && touchData.hitObject.CompareTag("MachineConnector"))
                    {
                        MachineConnector other = touchData.hitObject.GetComponent<MachineConnector>();
                        MachineConnectorManager.Instance.CreateConnection(this, other);
                        OnConnected(other);
                        other.OnConnected(this);
                    }
                    dummyConnector = null;
                }
                break;
        }
    }

    private MachineConnector CreateMouseFollower()
    {
        GameObject follower = new GameObject("MouseFollowerConnector");
        MachineConnector connector = follower.AddComponent<MachineConnector>();
        connector.connectionPoint = follower.transform;
        return connector;
    }
    
    private Vector3 GetMouseOnConnectorPlane(Vector2 screenPos)
    {
        Ray ray = Camera.main.ScreenPointToRay(screenPos);
        Plane plane = new Plane(Vector3.up, connectionPoint.position);

        if (plane.Raycast(ray, out float enter))
        {
            return ray.GetPoint(enter);
        }

        return connectionPoint.position; // fallback ifall raycasten failar
    }
}