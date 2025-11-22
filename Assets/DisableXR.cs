using UnityEngine;
using UnityEngine.SubsystemsImplementation;

public class DisableXR
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void DisableXRSubsystems()
    {
        try
        {
            var instances = SubsystemManager.GetInstances<IntegratedSubsystem>();
            foreach (var inst in instances)
            {
                var id = inst.SubsystemDescriptor?.id;
                if (id != null && id.ToLower().Contains("xr"))
                {
                    Debug.Log("Disabling XR subsystem: " + id);
                    inst.Stop();
                    inst.Destroy();
                }
            }
        }
        catch
        {
            // No XR subsystems, safe to ignore
        }
    }
}
