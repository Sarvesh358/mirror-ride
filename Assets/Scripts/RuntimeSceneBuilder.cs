using UnityEngine;

public class RuntimeSceneBuilder : MonoBehaviour
{
    void Awake()
    {
        BuildScene();
    }

    void BuildScene()
    {
        // --- Remove all existing cameras -------------------------
        Camera[] cameras = Camera.allCameras;
        foreach (var c in cameras)
        {
            Destroy(c.gameObject);
        }

        // --- Create ONE clean camera -----------------------------
        GameObject camObj = new GameObject("MainCamera");
        camObj.tag = "MainCamera";

        Camera mainCam = camObj.AddComponent<Camera>();
        mainCam.clearFlags = CameraClearFlags.SolidColor;
        mainCam.backgroundColor = new Color(0.1f, 0.1f, 0.1f);
        mainCam.fieldOfView = 60f;
        mainCam.nearClipPlane = 0.3f;
        mainCam.farClipPlane = 1000f;

        camObj.transform.position = new Vector3(0, 5, -10);
        camObj.transform.rotation = Quaternion.Euler(15, 0, 0);

        // --- Light ----------------------------------------------
        GameObject lightObj = new GameObject("DirectionalLight");
        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1f;
        lightObj.transform.rotation = Quaternion.Euler(50, -30, 0);

        // --- Ground plane ----------------------------------------
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(5, 1, 5);
        ground.GetComponent<Renderer>().material.color = new Color(0.2f, 0.2f, 0.2f);

        // --- Player cube -----------------------------------------
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Cube);
        player.name = "Player";
        player.transform.localScale = new Vector3(1.2f, 0.6f, 2f);
        player.transform.position = new Vector3(0, 0.3f, 0);
        player.GetComponent<Renderer>().material.color = Color.blue;

        // --- Camera follow ---------------------------------------
        var follow = camObj.AddComponent<SimpleCameraFollow>();
        follow.target = player.transform;
        follow.offset = new Vector3(0, 5, -10);
        follow.smoothTime = 0.15f;

        Debug.Log("RuntimeSceneBuilder: Scene built successfully.");
    }
}
