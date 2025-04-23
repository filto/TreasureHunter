using UnityEngine;

public class MachineConnector : MonoBehaviour
{
    public string machineID;
    public Transform connectionPoint;
    private MachineConnector dummyConnector;
    private MachineConnector currentFrom;

    public void OnTouchEvent(TouchData touchData)
    {
        switch (touchData.phase)
        {
            case TouchPhase.Began:
                currentFrom = this;
                StartConnectionFrom(currentFrom, touchData.screenPosition);
                break;

            case TouchPhase.Moved:
                if (dummyConnector != null)
                {
                    dummyConnector.transform.position = GetMouseOnConnectorPlane(touchData.screenPosition);

                    if (touchData.hitObject != null && touchData.hitObject.CompareTag("MachineConnector"))
                    {
                        MachineConnector hit = touchData.hitObject.GetComponent<MachineConnector>();
                        
                        if (hit != currentFrom  && !MachineConnectorManager.Instance.ConnectionExists(currentFrom , hit))
                        {
                            CleanupDummyConnection();
                            MachineConnectorManager.Instance.CreateConnection(currentFrom, hit);
                            currentFrom = hit;
                            StartConnectionFrom(hit, touchData.screenPosition);
                        }
                    }
                }
                break;

            case TouchPhase.Ended:
                CleanupDummyConnection();
                break;
        }
    }

    private void StartConnectionFrom(MachineConnector from, Vector2 screenPosition)
    {
        dummyConnector = CreateMouseFollower();
        dummyConnector.transform.position = GetMouseOnConnectorPlane(screenPosition);
        MachineConnectorManager.Instance.CreateConnection(from, dummyConnector);
    }

    private void CleanupDummyConnection()
    {
        if (dummyConnector != null)
        {
            MachineConnectorManager.Instance.RemoveConnection(dummyConnector);
            Destroy(dummyConnector.gameObject);
            dummyConnector = null;
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

        return connectionPoint.position;
    }
}
