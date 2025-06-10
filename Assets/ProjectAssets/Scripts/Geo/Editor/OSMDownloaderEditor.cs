using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

[CustomEditor(typeof(OSMDownloader))]
public class OSMDownloaderEditor : Editor
{
    private string selectedKey = "highway";
    private HashSet<string> selectedValues = new();
    private bool selectAll = true;
    private Dictionary<string, bool> valueToggles = new();

    private static readonly Dictionary<string, string[]> presets = new()
    {
        { "highway", new[] { "residential", "primary", "secondary", "tertiary", "footway", "cycleway", "service" } },
        { "building", new[] { "yes", "house", "residential", "commercial" } },
        { "natural", new[] { "water", "wood", "scrub" } },
        { "leisure", new[] { "park", "pitch", "playground" } },
        { "amenity", new[] { "school", "bench", "toilets", "cafe", "parking" } },
    };

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        EditorGUILayout.Space();
        EditorGUILayout.LabelField("Filter Preset Generator", EditorStyles.boldLabel);

        int index = EditorGUILayout.Popup("Key", GetKeyIndex(), GetKeys());
        selectedKey = GetKeys()[index];
        if (selectedKey == "tileImage")
        {
            EditorGUILayout.Space();
            EditorGUILayout.HelpBox("Detta alternativ laddar en kartbild (tile) från OpenStreetMap baserat på position och zoom.", MessageType.Info);

            if (GUILayout.Button("🌍 Hämta kartbild (tile)"))
            {
                var osm = (OSMDownloader)target;
                /*osm.DownloadTile();*/
            }

            return; // Hoppa resten av GUI:t
        }

        EditorGUILayout.Space();
        selectAll = EditorGUILayout.ToggleLeft("* (Alla typer)", selectAll);

        selectedValues.Clear();
        if (!selectAll)
        {
            var values = presets[selectedKey];
            foreach (var val in values)
            {
                if (!valueToggles.ContainsKey(val))
                    valueToggles[val] = false;

                valueToggles[val] = EditorGUILayout.ToggleLeft(val, valueToggles[val]);

                if (valueToggles[val])
                {
                    selectedValues.Add(val);
                }
            }
        }

        if (GUILayout.Button("➕ Lägg till filter"))
        {
            var osm = (OSMDownloader)target;

            if (selectAll)
            {
                osm.filters.Add(new OSMDownloader.OSMFilter
                {
                    key = selectedKey,
                    value = "*",
                    enabled = true
                });
            }
            else
            {
                foreach (var val in selectedValues)
                {
                    osm.filters.Add(new OSMDownloader.OSMFilter
                    {
                        key = selectedKey,
                        value = val,
                        enabled = true
                    });
                }
            }

            EditorUtility.SetDirty(osm);
        }
    }

    private int GetKeyIndex()
    {
        var keys = GetKeys();
        for (int i = 0; i < keys.Length; i++)
        {
            if (keys[i] == selectedKey) return i;
        }
        return 0;
    }

    private string[] GetKeys()
    {
        var arr = new List<string>(presets.Keys);
        arr.Add("tileImage"); // Lägg till specialfall
        return arr.ToArray();
    }
}
