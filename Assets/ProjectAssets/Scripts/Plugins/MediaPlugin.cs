using UnityEngine;

public static class MediaPlugin
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaObject _plugin;

    static MediaPlugin()
    {
        using (var unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer"))
        {
            AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
            using (var pluginClass = new AndroidJavaClass("com.nearby.mediaplugin.MediaBridge"))
            {
                _plugin = pluginClass.CallStatic<AndroidJavaObject>("getInstance", activity);
            }
        }
    }

    public static void TakePhoto()
    {
        _plugin?.Call("takePhoto");
    }
#else
    public static void TakePhoto()
    {
        Debug.Log("MediaPlugin.TakePhoto called (no-op in editor)");
    }
#endif
}