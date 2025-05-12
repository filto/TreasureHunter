using UnityEngine;

public class SpawnUIPrefabOnTrigger : MonoBehaviour
{
    [Tooltip("UI-prefab som ska skapas (ex. panel med knapp)")]
    public GameObject uiPrefab;
    
    private void OnTriggerEnter(Collider other)
    {
        if (uiPrefab != null)
        {
            Instantiate(uiPrefab, GameManager.Instance.canvas.transform);
        }
    }
}