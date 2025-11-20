using UnityEngine;

public class StarterController : MonoBehaviour
{
    void Awake()
    {
        Debug.Log("Mirror Ride Startup: Initializing Scene...");

        SpawnCamera();
        SpawnLight();
        SpawnSceneBuilder();
    }

    void SpawnCamera()
    {
        if (Camera.main != null) return;

        Debug.Log("Creating Main Camera...");

        GameObject camObj = new GameObject("MainCamera");
        camObj.tag = "MainCamera";

        Camera cam = camObj.AddComponent<Camera>();
        cam.nearClipPlane = 0.3f;
        cam.farClipPlane = 1000f;
        cam.fieldOfView = 60f;

        camObj.transform.position = new Vector3(0, 5, -10);
        camObj.transform.rotation = Quaternion.Euler(20, 0, 0);
    }

    void SpawnLight()
    {
        Debug.Log("Creating Light...");
        GameObject lightObj = new GameObject("Directional Light");
        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1f;
        lightObj.transform.rotation = Quaternion.Euler(50, -30, 0);
    }

    void SpawnSceneBuilder()
    {
        Debug.Log("Creating RuntimeSceneBuilder...");

        GameObject builder = new GameObject("SceneBuilder");
        builder.AddComponent<RuntimeSceneBuilder>();
    }
}
