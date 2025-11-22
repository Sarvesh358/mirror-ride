using UnityEngine;
using UnityEngine.XR;

public class DisableXR
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void DisableXRDisplay()
    {
        try
        {
            XRSettings.enabled = false;
        }
        catch {}

        try
        {
            var loaders = XRGeneralSettings.Instance?.Manager?.activeLoaders;
            if (loaders != null)
            {
                loaders.Clear();
            }
        }
        catch {}
    }
}
