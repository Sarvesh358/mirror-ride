using UnityEngine;

public class RuntimeSceneBuilder : MonoBehaviour
{
    void Awake()
    {
        // Remove all cameras
        foreach (var cam in Camera.allCameras)
        {
            Destroy(cam.gameObject);
        }

        // Create ONE clean camera
        GameObject camObj = new GameObject("MainCamera");
        camObj.tag = "MainCamera";

        Camera cam = camObj.AddComponent<Camera>();
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.1f, 0.1f, 0.1f);
        cam.fieldOfView = 60;
        cam.nearClipPlane = 0.3f;
        cam.farClipPlane = 1000;

        camObj.transform.position = new Vector3(0, 5, -10);
        camObj.transform.rotation = Quaternion.Euler(15, 0, 0);

        // Add a directional light
        GameObject lightObj = new GameObject("Light");
        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1f;
        lightObj.transform.rotation = Quaternion.Euler(50, -30, 0);

        // Create a ground plane
        GameObject ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "Ground";
        ground.transform.localScale = new Vector3(5, 1, 5);
        ground.GetComponent<Renderer>().material.color = new Color(0.2f, 0.2f, 0.2f);

        // Create player cube
        GameObject player = GameObject.CreatePrimitive(PrimitiveType.Cube);
        player.transform.position = Vector3.zero;
        player.GetComponent<Renderer>().material.color = Color.blue;

        // Camera follow script
        var follow = camObj.AddComponent<SimpleCameraFollow>();
        follow.target = player.transform;
        follow.offset = new Vector3(0, 5, -10);
        follow.smoothTime = 0.15f;
    }
}
