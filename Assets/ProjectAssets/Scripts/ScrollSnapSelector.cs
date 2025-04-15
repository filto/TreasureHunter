using UnityEngine;
using UnityEngine.UI;
using System;

public class ScrollSnapSelector : MonoBehaviour
{
    public ScrollRect scrollRect;
    public RectTransform content;
    public RectTransform viewport;
    
    public float offsetY = 0f;
    public GameObject[] menuViews;

    private RectTransform[] items;
    private bool isDragging;
    private int currentIndex;
    

    void Start()
    {
        // Hämta alla barn (knappar) från content
        items = new RectTransform[content.childCount];
        for (int i = 0; i < content.childCount; i++)
        {
            items[i] = content.GetChild(i) as RectTransform;
        }
        
        Debug.Log($"[ScrollSnapSelector] Antal knappar hittade: {items.Length}");

        scrollRect.onValueChanged.AddListener(_ => isDragging = true);
    }

    void Update()
    {
        if (isDragging && Input.GetMouseButtonUp(0))
        {
            Debug.Log("Släppte scrollen – dags att snappa");
            isDragging = false;
            SnapToClosest();
        }
    }

    void SnapToClosest()
    {
        float closestDistance = float.MaxValue;
        int closestIndex = 0;

        for (int i = 0; i < items.Length; i++)
        {
            float distance = Mathf.Abs(GetItemCenterY(items[i]) - GetViewportCenterY());
            if (distance < closestDistance)
            {
                closestDistance = distance;
                closestIndex = i;
            }
        }
        Debug.Log($"📍 Närmast mitten: Knapp {closestIndex} ({items[closestIndex].name}) – avstånd: {closestDistance:F2}");

        float itemY = items[closestIndex].position.y;
        float viewportY = viewport.position.y;
        float delta = itemY - viewportY;

        // Flytta content upp eller ner för att centrera knappen
        content.position -= new Vector3(0f, delta -offsetY, 0f);

        Debug.Log($"Snappade till knapp {closestIndex} – flyttade content {delta:F2} i Y-led");
        
        for (int i = 0; i < menuViews.Length; i++)
        {
            if (menuViews[i] != null)
                menuViews[i].SetActive(i == closestIndex);
        }
    }

    float GetItemCenterY(RectTransform item)
    {
        return item.position.y;
    }

    float GetViewportCenterY()
    {
        return viewport.position.y;
    }
    
}
