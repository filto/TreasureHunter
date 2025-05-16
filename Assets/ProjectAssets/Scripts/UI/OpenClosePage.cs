using UnityEngine;

public class OpenClosePage : MonoBehaviour
{
    public GameObject pagePrefab;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public void openPage()
    {
       Instantiate(pagePrefab, GameManager.Instance.canvas.transform); 
       closePage();
    }

    // Update is called once per frame
    public void closePage()
    {
        Destroy(gameObject); 
    }
}
