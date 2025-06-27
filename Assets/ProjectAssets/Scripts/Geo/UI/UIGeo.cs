using UnityEngine;

public class UIGeo : MonoBehaviour
{
    public GameObject gpsTrackerObject;
    public GameObject GPSMapPositioner;
    public GameObject GPSUI;
    public bool isActive=false;

    public void ToggleGPSTracker()
    {
        if (!isActive)
        {
            gpsTrackerObject.SetActive(true);
            GPSMapPositioner.SetActive(true);
            GPSUI.SetActive(true);
            isActive = true;
        }

        else
        {
            gpsTrackerObject.SetActive(false);
            GPSMapPositioner.SetActive(false);
            GPSUI.SetActive(false);
            isActive = false;
        }
    }
}
