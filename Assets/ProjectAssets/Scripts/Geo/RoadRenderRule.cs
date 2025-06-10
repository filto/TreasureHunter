using UnityEngine;

[System.Serializable]
public class RoadRenderRule
{
    public string highwayType;      // Ex: "residential"
    public float width = 5f;        // I Unity-enheter
    public Material material;
}