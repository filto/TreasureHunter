using UnityEngine;

public class mediaController : MonoBehaviour
{
    public void OnTakePhotoClicked()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        try
        {
            using (var plugin = new AndroidJavaClass("com.nearby.mediaplugin.MediaBridge"))
            {
                plugin.CallStatic("logSomething");
            }
        }
        catch (System.Exception e)
        {
            Debug.LogError("Plugin call failed: " + e.Message);
        }
#else
        Debug.Log("Simulerad plugin-anrop i editor");
#endif
    }
    
    public void TakePhoto()
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass plugin = new AndroidJavaClass("com.nearby.mediaplugin.MediaBridge");
            plugin.CallStatic("takePhoto", activity);
        }
    }
    
    public void PickPhoto()
    {
        using (AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");

            AndroidJavaClass plugin = new AndroidJavaClass("com.nearby.mediaplugin.MediaBridge");
            plugin.CallStatic("pickPhoto", activity);
        }
    }
}