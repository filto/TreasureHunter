using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public static class UIWrappers
{
    /// <summary>
    /// Returnerar true om skärmpositionen ligger över ett UI-element (Canvas).
    /// </summary>
    public static bool IsPointerOverUI(Vector2 screenPosition, GameObject ignore = null)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            if (ignore == null || result.gameObject != ignore)
            {
                return true; // Vi träffade något (och det var inte oss själva)
            }
        }

        return false; // Bara det vi ignorerar (eller inget alls)
    }
    
    public static bool DroppedOnTrashCan(Vector2 screenPosition)
    {
        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = screenPosition
        };

        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        foreach (var result in results)
        {
            if (result.gameObject.CompareTag("TrashCan"))
            {
                return true;
            }
        }

        return false;
    }
}