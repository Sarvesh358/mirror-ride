using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// PathManager is responsible for providing world-space path points.
/// Right now it includes a simple test route. Later, integrate Google Maps Directions API
/// to obtain lat/lon polyline, convert to world coordinates, and set to GhostCarController.SetPath().
/// </summary>
public class PathManager : MonoBehaviour
{
    public GhostCarController ghost;
    public Transform[] debugWaypoints; // optional: you can place empty GameObjects in scene and assign

    void Start()
    {
        if (ghost == null && debugWaypoints != null && debugWaypoints.Length > 0)
        {
            List<Vector3> pts = new List<Vector3>();
            foreach (var t in debugWaypoints) pts.Add(t.position);
            ghost.SetPath(pts);
            Debug.Log("PathManager: set path from debug waypoints");
        }
        else if (ghost != null && (debugWaypoints == null || debugWaypoints.Length == 0))
        {
            // fallback: build a simple curved test path in front of origin
            List<Vector3> test = new List<Vector3>()
            {
                new Vector3(0,0,0),
                new Vector3(10,0,20),
                new Vector3(20,0,40),
                new Vector3(10,0,60),
                new Vector3(0,0,80)
            };
            ghost.SetPath(test);
            Debug.Log("PathManager: set default test path");
        }
    }

    // TODO: Add method to request route using Google Maps Directions API:
    // 1. Request directions (HTTP) for origin/destination
    // 2. Decode polyline -> lat/lon points
    // 3. Convert lat/lon to Unity world positions (requires a mapping strategy)
    // 4. ghost.SetPath(worldPoints)
}
