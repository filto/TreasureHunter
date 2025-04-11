using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class InteractionUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public event Action<TouchData, Vector3, GameObject> OnDragEnd;
    
    private RectTransform rectTransform;
    private Canvas canvas;
    private Vector2 originalPosition;
    public bool snapToStart;
    private TouchData touchData;
    public GameObject ghostVisual;
    private GameObject ghostInstance;
    public Transform ghostVisualParent;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        touchData = BuildTouchData(TouchPhase.Began, eventData);
        originalPosition = rectTransform.anchoredPosition;
        
        if (ghostVisual != null)
        {
            Debug.Log("Skapar kopia");
            Transform parentToUse = ghostVisualParent != null ? ghostVisualParent : canvas.transform;
            ghostInstance = Instantiate(ghostVisual, parentToUse);
            ghostInstance.GetComponent<RectTransform>().position = rectTransform.position;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (ghostVisual != null)
        {
            touchData = BuildTouchData(TouchPhase.Moved, eventData);
            ghostInstance.GetComponent<RectTransform>().anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
        
        else 
        {
            // Rör på objektet i canvas, kompensera för skalning
            touchData = BuildTouchData(TouchPhase.Moved, eventData);
            rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        
        OnDragEnd?.Invoke(touchData, transform.position, gameObject);

        if (snapToStart)
        {
            rectTransform.anchoredPosition = originalPosition;
        }
        
        if (ghostVisual != null)
        {
            Destroy(ghostInstance);
        }
    }

    private TouchData BuildTouchData(TouchPhase phase, PointerEventData eventData)
    {
        GameObject hitObject = null;
        Vector3 worldPos = Vector3.zero;

        Ray ray = Camera.main.ScreenPointToRay(eventData.position);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            hitObject = hit.collider.gameObject;
            worldPos = hit.point;
        }

        return new TouchData(
            phase: phase,
            worldPosition: worldPos,
            screenPosition: eventData.position,
            hitObject: hitObject,
            touchCount:Input.touchCount
        );
    }
}