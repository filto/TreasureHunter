using UnityEngine;
using UnityEngine.EventSystems;

public class Joystick : MonoBehaviour, IDragHandler, IPointerUpHandler, IPointerDownHandler
{
    public RectTransform handle;         // Den rörliga knoppen
    public float moveRadius = 100f;      // Max avstånd från mittpunkten

    private Vector2 input = Vector2.zero;
    public Vector2 Direction => input;   // Kan läsas externt

    private Vector2 startPosition;

    void Start()
    {
        if (handle != null)
            startPosition = handle.anchoredPosition;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        OnDrag(eventData); // Direkt reagera på första trycket
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector2 pos;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            transform as RectTransform,
            eventData.position,
            eventData.pressEventCamera,
            out pos
        );

        pos = Vector2.ClampMagnitude(pos, moveRadius);
        input = pos / moveRadius;

        if (handle != null)
            handle.anchoredPosition = pos;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        input = Vector2.zero;

        if (handle != null)
            handle.anchoredPosition = startPosition;
    }
    
}