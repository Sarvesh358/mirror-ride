using UnityEngine;
using UnityEngine.Rendering;

public class RenderFix : MonoBehaviour
{
    private void OnEnable()
    {
        // FIX: Replaced obsolete 'beginFrameRendering' with 'beginContextRendering'
        RenderPipelineManager.beginContextRendering += OnBeginContextRendering;
    }

    private void OnDisable()
    {
        // FIX: Replaced obsolete 'beginFrameRendering' with 'beginContextRendering'
        RenderPipelineManager.beginContextRendering -= OnBeginContextRendering;
    }

    private void OnBeginContextRendering(ScriptableRenderContext context, List<Camera> cameras)
    {
        // Your existing rendering logic here (or empty if it was just for event attachment)
    }
}
