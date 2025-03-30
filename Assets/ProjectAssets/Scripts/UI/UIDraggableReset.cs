using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class UIDraggableReset : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public event Action<Vector3, GameObject, Vector3, GameObject> OnDragEnd;
    
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalPosition;
    public bool snapToStart;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Rör på objektet i canvas, kompensera för skalning
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        GameObject hitObject = null;
        Vector3 worldPos = Vector3.zero;
        
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            hitObject = hit.collider.gameObject;
            worldPos = hit.point;
            
        }
        OnDragEnd?.Invoke(worldPos, hitObject, transform.position, gameObject);

        if (snapToStart)
        {
            rectTransform.anchoredPosition = originalPosition;
        }
    }
}