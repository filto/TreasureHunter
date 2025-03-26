using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }  // Singleton-instans

    [Header("Globala Referenser")]
    public GameObject clickableObject;     // Objekt där noder kan placeras
    public GameObject trashCan;            // SoptunneObjektet
    public GameObject nodePrefab;           // Node prefab objektet
    public GameObject gridBackground;      // Referens till bakgrunden
    public Transform nodeContainer;        // Behållare för alla noder
    public GameObject connectionPrefab;    //Connection prefab
    public Camera uiCamera;
    public GameObject nodeMenu;
    public GameObject interactionManager;
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(gameObject);  // Behåll GameManager vid scenbyte (valfritt)
        }
        else
        {
            Destroy(gameObject);
        }
    }
}