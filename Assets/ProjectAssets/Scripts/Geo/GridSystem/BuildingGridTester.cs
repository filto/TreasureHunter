using UnityEngine;
using System.Collections.Generic;

[ExecuteAlways]
public class BuildingGridGenerator : MonoBehaviour
{
    [Header("Input Mesh")]
    public MeshFilter meshFilter;

    [Header("Grid")]
    public float cellSize = 2f;

    private List<Vector2> outerPolygon = new();
    private List<List<Vector2>> allLoops = new();
    private List<Vector3> validCenters = new();
    private Quaternion gridRotation = Quaternion.identity;

    void OnDrawGizmosSelected()
    {
        if (meshFilter == null || meshFilter.sharedMesh == null) return;

        allLoops = MeshOutlineUtility.FindAllPolygonLoops(meshFilter.sharedMesh, meshFilter.transform);
        if (allLoops.Count == 0) return;

        outerPolygon = allLoops[0];
        if (outerPolygon.Count < 3) return;

        // Beräkna gridrotation från längsta kant
        gridRotation = GetRotationFromLongestEdge(outerPolygon);

        Bounds bounds = Get2DBounds(outerPolygon);
        
        validCenters.Clear();

        for (float x = bounds.min.x; x < bounds.max.x; x += cellSize)
        {
            for (float y = bounds.min.y; y < bounds.max.y; y += cellSize)
            {
                Vector2 center2D = new Vector2(x + cellSize / 2f, y + cellSize / 2f);
                
                Vector2[] corners = new Vector2[]
                {
                    new Vector2(-cellSize / 2f, -cellSize / 2f),
                    new Vector2(+cellSize / 2f, -cellSize / 2f),
                    new Vector2(-cellSize / 2f, +cellSize / 2f),
                    new Vector2(+cellSize / 2f, +cellSize / 2f)
                };
                
                //Vector3 rotatedCenter = gridRotation * new Vector3(center2D.x, 0, center2D.y);
                
                for (int i = 0; i < corners.Length; i++) //Rotera cellen längst längsta edgen
                {
                    Debug.Log(corners[i]);
                    Debug.Log(gridRotation);
                    Vector3 rotated = gridRotation * new Vector3(corners[i].x, 0, corners[i].y);
                    Debug.Log(rotated);
                    corners[i] = new Vector2(rotated.x + center2D.x, rotated.z + center2D.y);
                    Debug.Log(center2D);
                    Debug.Log(corners[i]);
                }

                
                /*
                for (int i = 0; i < corners.Length; i++)
                {
                    corners[i] += center2D;
                    Debug.Log(corners[i]);
                }
                */
                
               
                bool valid = true;
                foreach (var corner in corners) //Kontrollera att gridcellens alla hörn INTE ligger utanför polygonen kontur
                {   
                    if (!PointInPolygon(corner, outerPolygon))
                    {
                        valid = false;
                        break;
                    }

                    for (int i = 1; i < allLoops.Count; i++) //Kontrollera att gridcellens alla hörn ligger utanför ett hål
                    {
                        if (PointInPolygon(corner, allLoops[i]))
                        {
                            valid = false;
                            break;
                        }
                    }

                    if (!valid) break;
                }

                if (valid) //kontrollera att gridcellens mittpunkt inte ligger i ett hål
                {
                    for (int i = 1; i < allLoops.Count; i++)
                    {
                        if (PointInPolygon(center2D, allLoops[i]))
                        {
                            valid = false;
                            break;
                        }
                    }
                }
                
                if (valid) 
                {
                    Vector3 center = new Vector3(center2D.x, meshFilter.transform.position.y, center2D.y);
                    validCenters.Add(center);
                }
            }
        }

        // Rita giltiga gridceller
        Gizmos.color = Color.green;
        foreach (var pos in validCenters)
        {
            Gizmos.DrawWireCube(pos + Vector3.up * 0.05f, new Vector3(cellSize, 0.1f, cellSize));

            // Visa rotation som en blå pil
            Vector3 dir = gridRotation * Vector3.forward;
            Gizmos.color = Color.cyan;
            Gizmos.DrawRay(pos + Vector3.up * 0.1f, dir * (cellSize * 0.4f));
        }

        // Rita konturer
        for (int i = 0; i < allLoops.Count; i++)
        {
            Gizmos.color = (i == 0) ? Color.yellow : Color.red;
            var loop = allLoops[i];
            for (int j = 0; j < loop.Count; j++)
            {
                Vector3 a = new Vector3(loop[j].x, meshFilter.transform.position.y + 0.1f, loop[j].y);
                Vector3 b = new Vector3(loop[(j + 1) % loop.Count].x, a.y, loop[(j + 1) % loop.Count].y);
                Gizmos.DrawLine(a, b);
            }
        }
    }

    Quaternion GetRotationFromLongestEdge(List<Vector2> polygon)
    {
        float maxDist = 0f;
        Vector2 bestA = Vector2.zero, bestB = Vector2.zero;

        for (int i = 0; i < polygon.Count; i++)
        {
            Vector2 a = polygon[i];
            Vector2 b = polygon[(i + 1) % polygon.Count];
            float dist = Vector2.Distance(a, b);

            if (dist > maxDist)
            {
                maxDist = dist;
                bestA = a;
                bestB = b;
            }
        }

        // Säkerställ att riktningen alltid går åt höger (eller uppåt om horisontell)
        if (bestA.x > bestB.x || (Mathf.Approximately(bestA.x, bestB.x) && bestA.y > bestB.y))
        {
            (bestA, bestB) = (bestB, bestA); // swap
        }

        Vector2 dir = (bestB - bestA).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

        return Quaternion.Euler(0, angle, 0); // ingen invertering
    }

    bool PointInPolygon(Vector2 point, List<Vector2> polygon)
    {
        int j = polygon.Count - 1;
        bool inside = false;
        for (int i = 0; i < polygon.Count; j = i++)
        {
            if (((polygon[i].y > point.y) != (polygon[j].y > point.y)) &&
                (point.x < (polygon[j].x - polygon[i].x) * (point.y - polygon[i].y) /
                 (polygon[j].y - polygon[i].y) + polygon[i].x))
            {
                inside = !inside;
            }
        }
        return inside;
    }

    Bounds Get2DBounds(List<Vector2> poly)
    {
        Vector2 min = poly[0];
        Vector2 max = poly[0];
        foreach (var p in poly)
        {
            min = Vector2.Min(min, p);
            max = Vector2.Max(max, p);
        }
        return new Bounds((min + max) / 2f, max - min);
    }
}
