using UnityEngine;

public class UIGeo : MonoBehaviour
{
    public GameObject gpsTrackerObject;

    public void ActivateGPSTracker()
    {
        if (gpsTrackerObject != null)
        {
            gpsTrackerObject.SetActive(true);
        }
    }
}
