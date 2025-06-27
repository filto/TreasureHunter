using UnityEngine;

[System.Serializable]
public class PrefabSpawnInfo
{
    public GameObject prefab;
    public Transform parent;
    public Vector3 position;
    public Quaternion rotation = Quaternion.identity;
}

public class BootStrapper : MonoBehaviour
{
    [Header("Aktivera vid start")]
    public GameObject[] objectsToEnable;

    [Header("Inaktivera vid start")]
    public GameObject[] objectsToDisable;

    [Header("Instansiera prefabs")]
    public PrefabSpawnInfo[] prefabsToSpawn;

    void Awake()
    {
        InitSceneState();
        SpawnPrefabs();
    }

    void InitSceneState()
    {
        foreach (var go in objectsToEnable)
        {
            if (go != null) go.SetActive(true);
        }

        foreach (var go in objectsToDisable)
        {
            if (go != null) go.SetActive(false);
        }
    }

    void SpawnPrefabs()
    {
        foreach (var info in prefabsToSpawn)
        {
            if (info.prefab != null)
            {
                Instantiate(
                    info.prefab,
                    info.position,
                    info.rotation,
                    info.parent
                );
            }
        }
    }
}