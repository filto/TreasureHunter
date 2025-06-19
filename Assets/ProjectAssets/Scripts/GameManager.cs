using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }  // Singleton-instans

    [Header("Globala Referenser")]
    public GameObject interactionManager;
    public GameObject canvas;
    public GameObject nodeContainer;
    public GameObject worldParent;
    
    [Header("Old Nodesystem")]
    public GameObject trashCan;            // SoptunneObjektet
    public GameObject nodePrefab;           // Node prefab objektet
    public GameObject connectionPrefab;    //Connection prefab
    public GameObject nodeMenu;
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