using UnityEngine;
using UnityEngine.Rendering;

public class RenderFix : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSplashScreen)]
    static void DisableDebugSurfaces()
    {
        Debug.Log("RenderFix: Disabling Render Pipeline Debug Overlays.");

        // Disable SRP debug UI overlay
        RenderPipelineManager.beginFrameRendering += (context, cameras) =>
        {
            try
            {
                SRPHelpers.DisableDebugModes();
            }
            catch { }
        };
    }
}

public static class SRPHelpers
{
    public static void DisableDebugModes()
    {
        // Force disable debug render features
        var pipeline = GraphicsSettings.currentRenderPipeline;
        if (pipeline != null)
        {
            var debugDataField = pipeline.GetType().GetField("debugDisplaySettings");
            if (debugDataField != null)
            {
                var debugData = debugDataField.GetValue(pipeline);
                if (debugData != null)
                {
                    var debugUIField = debugData.GetType().GetField("AreAnySettingsActive");
                    if (debugUIField != null)
                    {
                        debugUIField.SetValue(debugData, false);
                    }
                }
            }
        }
    }
}
