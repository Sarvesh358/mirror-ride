using UnityEngine;
using UnityEngine.XR.Management;
using UnityEngine.SubsystemsImplementation; // Include this namespace
using System.Collections.Generic; // May be required for XRManagerSettings

public class DisableXR : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Disabling XR...");
        
        // --- FIX FOR CS0103 / CS1069 ---
        // This attempts to disable the XR systems directly.
        
        // This typically works for modern Unity versions:
        if (XRGeneralSettings.Instance != null && XRGeneralSettings.Instance.Manager != null)
        {
            XRGeneralSettings.Instance.Manager.DeinitializeLoader();
            Debug.Log("XR Loader Deinitialized.");
        }
        else
        {
            Debug.LogWarning("XR General Settings or Manager not found, cannot explicitly deinitialize XR.");
        }
        
        // Old (and less necessary) way using SubsystemManager, might still fail without package:
        // SubsystemManager.GetSubsystems<IntegratedSubsystem>().ForEach(s => s.Stop());
        
        // If the above code causes issues, delete the entire block in Awake.
        // However, this version should include the correct namespace.
    }
}
