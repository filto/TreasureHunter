using System.Collections.Generic;
using UnityEngine;

public class RoadRendererSettings : MonoBehaviour
{
    public List<RoadRenderRule> roadRules;
    public RoadRenderRule defaultRule;  // Används om ingen matchar

    public RoadRenderRule GetRuleFor(string type)
    {
        foreach (var rule in roadRules)
        {
            if (rule.highwayType == type)
                return rule;
        }
        return defaultRule;
    }
}