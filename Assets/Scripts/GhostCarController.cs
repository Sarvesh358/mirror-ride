using UnityEngine;
using System.Collections.Generic;

[RequireComponent(typeof(Transform))]
public class GhostCarController : MonoBehaviour
{
    [Tooltip("Path to follow: list of world-space points.")]
    public List<Vector3> pathPoints = new List<Vector3>();
    [Range(0f, 1f)]
    public float progress = 0f;           // normalized 0..1 along the path
    public float travelSpeed = 8f;        // units/sec (fallback when GPS not used)

    // internal
    float pathLength = 0f;
    List<float> segmentLengths = new List<float>();

    void Start()
    {
        RecalculatePath();
    }

    void Update()
    {
        if (pathPoints == null || pathPoints.Count < 2) return;

        // if progress set externally (e.g., by GPSManager), jump to progress
        // otherwise, advance progress by travelSpeed
        if (Mathf.Approximately(travelSpeed, 0f) == false)
        {
            float delta = travelSpeed * Time.deltaTime;
            float currentDist = progress * pathLength;
            currentDist = Mathf.Clamp(currentDist + delta, 0f, pathLength);
            progress = pathLength > 0f ? currentDist / pathLength : 0f;
        }

        Vector3 pos = EvaluatePositionAt(progress);
        transform.position = pos;

        // orient forward along tangent
        Vector3 nextPos = EvaluatePositionAt(Mathf.Min(progress + 0.001f, 1f));
        Vector3 forward = (nextPos - pos);
        if (forward.sqrMagnitude > 0.0001f) transform.rotation = Quaternion.LookRotation(forward.normalized, Vector3.up);
    }

    public void RecalculatePath()
    {
        segmentLengths.Clear();
        pathLength = 0f;
        if (pathPoints == null || pathPoints.Count < 2) return;
        for (int i = 0; i < pathPoints.Count - 1; i++)
        {
            float len = Vector3.Distance(pathPoints[i], pathPoints[i + 1]);
            segmentLengths.Add(len);
            pathLength += len;
        }
    }

    public Vector3 EvaluatePositionAt(float t)
    {
        t = Mathf.Clamp01(t);
        if (pathPoints.Count == 0) return Vector3.zero;
        if (pathPoints.Count == 1) return pathPoints[0];
        if (Mathf.Approximately(pathLength, 0f)) return pathPoints[0];

        float dist = t * pathLength;
        float acc = 0f;
        for (int i = 0; i < segmentLengths.Count; i++)
        {
            float segLen = segmentLengths[i];
            if (acc + segLen >= dist)
            {
                float segT = (dist - acc) / segLen;
                return Vector3.Lerp(pathPoints[i], pathPoints[i + 1], segT);
            }
            acc += segLen;
        }
        return pathPoints[pathPoints.Count - 1];
    }

    // External setter for GPS-based progress (0..1)
    public void SetProgress(float normalized)
    {
        progress = Mathf.Clamp01(normalized);
    }

    // Helper to assign path from Vector3 array
    public void SetPath(List<Vector3> points)
    {
        pathPoints = new List<Vector3>(points);
        RecalculatePath();
    }
}

