using UnityEngine;

public class StarterController : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Mirror Ride 3D Initialized!");

        // Make sure RuntimeSceneBuilder exists
        if (FindObjectOfType<RuntimeSceneBuilder>() == null)
        {
            Debug.Log("No RuntimeSceneBuilder in scene, creating one now.");
            GameObject go = new GameObject("SceneBuilder");
            go.AddComponent<RuntimeSceneBuilder>();
        }
    }
}
