using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public static class DisableXRReflection
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void TryDisableXR()
    {
        Debug.Log("DisableXRReflection: Attempting to disable XR subsystems via reflection...");

        try
        {
            // Search for SubsystemManager
            Type subsystemManager = FindTypeByName("SubsystemManager");
            if (subsystemManager != null)
            {
                Debug.Log("DisableXRReflection: Found SubsystemManager = " + subsystemManager.FullName);
                DisableSubsystems(subsystemManager);
            }
            else
            {
                Debug.Log("DisableXRReflection: No SubsystemManager found, scanning for any *Subsystem* classes...");
                ScanAssembliesForSubsystems();
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("DisableXRReflection: Exception while disabling XR: " + ex);
        }
    }

    static Type FindTypeByName(string name)
    {
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type[] types;
            try { types = asm.GetTypes(); }
            catch { continue; }

            foreach (var t in types)
            {
                if (t.Name.Equals(name, StringComparison.OrdinalIgnoreCase))
                    return t;
            }
        }
        return null;
    }

    static void DisableSubsystems(Type managerType)
    {
        try
        {
            MethodInfo[] methods = managerType.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var m in methods)
            {
                if (!m.Name.Contains("GetInstances") || !m.IsGenericMethodDefinition)
                    continue;

                Debug.Log("DisableXRReflection: Found potential GetInstances<T>() = " + m.Name);

                // Guess common subsystem types
                Type integrated = FindTypeByName("IntegratedSubsystem")
                               ?? FindTypeByName("XRDisplaySubsystem")
                               ?? FindTypeByName("Subsystem");

                if (integrated == null)
                {
                    Debug.Log("DisableXRReflection: No subsystem base type found.");
                    return;
                }

                var generic = m.MakeGenericMethod(integrated);
                var listType = typeof(List<>).MakeGenericType(integrated);
                var list = Activator.CreateInstance(listType);

                generic.Invoke(null, new object[] { list });

                foreach (var subsystem in (IEnumerable)list)
                {
                    StopAndDestroy(subsystem);
                }
            }
        }
        catch (Exception ex)
        {
            Debug.LogWarning("DisableXRReflection: DisableSubsystems() exception: " + ex);
        }
    }

    static void ScanAssembliesForSubsystems()
    {
        foreach (var asm in AppDomain.CurrentDomain.GetAssemblies())
        {
            Type[] types;
            try { types = asm.GetTypes(); }
            catch { continue; }

            foreach (var t in types)
            {
                if (!t.Name.Contains("Subsystem"))
                    continue;

                Debug.Log("DisableXRReflection: Found subsystem-like type: " + t.FullName);

                MethodInfo stop = t.GetMethod("Stop", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
                MethodInfo destroy = t.GetMethod("Destroy", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);

                if (stop != null || destroy != null)
                    Debug.Log("DisableXRReflection: Type may be a real subsystem.");

                // Cannot disable here, we don't have instances; handled via GetInstances if possible.
            }
        }
    }

    static void StopAndDestroy(object subsystem)
    {
        if (subsystem == null) return;

        Type t = subsystem.GetType();
        Debug.Log("DisableXRReflection: Stopping subsystem: " + t.FullName);

        try
        {
            var stop = t.GetMethod("Stop", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            stop?.Invoke(subsystem, null);

            var destroy = t.GetMethod("Destroy", BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            destroy?.Invoke(subsystem, null);

            Debug.Log("DisableXRReflection: Subsystem fully disabled: " + t.FullName);
        }
        catch (Exception ex)
        {
            Debug.LogWarning("DisableXRReflection: Failed to disable subsystem: " + ex);
        }
    }
}
