using UnityEngine;

/// <summary>
/// GPSManager reads device GPS and exposes normalized progress (0..1) on active path.
/// This is a minimal implementation that starts LocationService. It assumes the path points are in Unity world coords.
/// In production, you will convert geo coords to world coords or compute closest point on a geo-route and then derive percent.
/// </summary>
public class GPSManager : MonoBehaviour
{
    public GhostCarController ghost;
    public bool useGps = false;
    public float simulatedSpeed = 8f;

    void Start()
    {
        if (useGps) StartCoroutine(StartLocationService());
    }

    System.Collections.IEnumerator StartLocationService()
    {
        if (!Input.location.isEnabledByUser)
        {
            Debug.LogWarning("GPS not enabled by user.");
            yield break;
        }

        Input.location.Start();
        int maxWait = 20;
        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        if (Input.location.status == LocationServiceStatus.Failed)
        {
            Debug.LogWarning("Unable to determine device location.");
            yield break;
        }
        else
        {
            Debug.Log("GPS ready.");
            // You can use Input.location.lastData.latitude & longitude here to match against a geo-route
        }
    }

    void Update()
    {
        if (!useGps || ghost == null) return;

        // Placeholder logic: advance ghost progress by simulatedSpeed
        ghost.travelSpeed = simulatedSpeed;
        // In future: compute normalized progress from lat/lon vs route length and call ghost.SetProgress(percent);
    }

    // call this to set simulated speed (useful for AutoSpeed mode)
    public void SetSimulatedSpeed(float s)
    {
        simulatedSpeed = s;
    }
}
