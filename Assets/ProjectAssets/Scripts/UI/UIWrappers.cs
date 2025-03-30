using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public static class UIWrappers
{
    /// <summary>
    /// Returnerar true om skärmpositionen ligger över ett UI-element (Canvas).
    /// </summary>
    public static bool IsPointerOverUI(Vector2 screenPosition)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        return results.Count > 0;
    }
}