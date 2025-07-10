using UnityEngine;
using UnityEngine.UI;

public class PluginReceiver : MonoBehaviour
{
    public Text imageDebug;
    public Renderer targetRenderer; // Dra in SpriteRenderer i Inspector
    
    public void OnPhotoTaken(string path)
    {
        imageDebug.text ="Foto sparat på: " + path;
        StartCoroutine(LoadImage(path));
    }
    
    private System.Collections.IEnumerator LoadImage(string path)
    {
        string fullPath = "file://" + path;

        using (WWW www = new WWW(fullPath))
        {
            yield return www;

            Texture2D texture = www.texture;
            imageDebug.text= "Fel: " + www.error;

            if (texture != null)
            {
                targetRenderer.material.mainTexture = texture;
            }
            else
            {
                imageDebug.text= "Kunde inte ladda bild: " + fullPath;
            }
        }
    }
    
}