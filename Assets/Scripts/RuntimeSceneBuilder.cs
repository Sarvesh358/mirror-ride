using UnityEngine;
using System.Collections.Generic;

public class RuntimeSceneBuilder : MonoBehaviour
{
    // sizes and colors
    readonly Vector3 playerSize = new Vector3(1.2f, 0.6f, 2.0f);
    readonly Vector3 ghostSize  = new Vector3(1.0f, 0.6f, 2.0f);
    readonly Color playerColor  = new Color(0.0f, 0.45f, 0.8f); // blue
    readonly Color ghostColor   = new Color(1.0f, 0.45f, 0.0f); // orange

    void Awake()
    {
        // Remove extra cameras to avoid split/dual-camera issues
        RemoveExtraCameras();

        // Ensure main objects exist
        EnsureMainCamera();
        EnsureDirectionalLight();
        CreateGround();

        // Build player & ghost
        GameObject player = CreateCar("PlayerCar", playerSize, playerColor, new Vector3(0f, 0.3f, 0f));
        GameObject ghost  = CreateCar("GhostCar", ghostSize, ghostColor, new Vector3(0f, 0.3f, 6f));

        // Attach controllers if not already
        if (player.GetComponent<PlayerCarController>() == null) player.AddComponent<PlayerCarController>();
        if (ghost.GetComponent<GhostCarController>() == null) ghost.AddComponent<GhostCarController>();

        // Attach ScoreManager
        var scoreManager = FindOrCreate<ScoreManager>("ScoreManager");
        scoreManager.player = player.transform;
        scoreManager.ghost = ghost.transform;

        // Attach PathManager and assign ghost
        var pathManager = FindOrCreate<PathManager>("PathManager");
        var ghostController = ghost.GetComponent<GhostCarController>();
        pathManager.ghost = ghostController;

        // Attach GPSManager (disabled by default)
        var gps = FindOrCreate<GPSManager>("GPSManager");
        gps.ghost = ghostController;
        gps.useGps = false;

        // Camera follow
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            var follow = mainCam.gameObject.GetComponent<SimpleCameraFollow>();
            if (follow == null) follow = mainCam.gameObject.AddComponent<SimpleCameraFollow>();
            follow.target = player.transform;
            follow.offset = new Vector3(0f, 5f, -8f);
            follow.smoothTime = 0.12f;
        }

        // Add runtime starter logging (optional)
        Debug.Log("RuntimeSceneBuilder: scene built — Player & Ghost spawned.");
    }

    // Ensure only one camera is active (keeps best camera)
    void RemoveExtraCameras()
    {
        Camera[] cams = Camera.allCameras;
        if (cams == null || cams.Length == 0) return;
        // Keep the first camera if any, disable others
        bool kept = false;
        for (int i = 0; i < cams.Length; i++)
        {
            if (!kept)
            {
                cams[i].gameObject.SetActive(true);
                // ensure clear flags are solid color to avoid split visuals
                cams[i].clearFlags = CameraClearFlags.SolidColor;
                cams[i].backgroundColor = new Color(0.08f, 0.08f, 0.08f); // dark grey
                cams[i].depth = 0;
                kept = true;
            }
            else
            {
                cams[i].gameObject.SetActive(false);
            }
        }
    }

    void EnsureMainCamera()
    {
        if (Camera.main != null) return;
        GameObject camObj = new GameObject("MainCamera");
        camObj.tag = "MainCamera";
        Camera cam = camObj.AddComponent<Camera>();
        cam.nearClipPlane = 0.3f;
        cam.farClipPlane = 1000f;
        cam.fieldOfView = 60f;
        cam.clearFlags = CameraClearFlags.SolidColor;
        cam.backgroundColor = new Color(0.08f, 0.08f, 0.08f);
        camObj.transform.position = new Vector3(0f, 5f, -8f);
        camObj.transform.rotation = Quaternion.Euler(18f, 0f, 0f);
    }

    void EnsureDirectionalLight()
    {
        // If a directional light exists, keep it; otherwise create one.
        Light existing = FindObjectOfType<Light>();
        if (existing != null) return;

        GameObject lightObj = new GameObject("Directional Light");
        Light light = lightObj.AddComponent<Light>();
        light.type = LightType.Directional;
        light.intensity = 1.0f;
        light.shadows = LightShadows.Soft;
        lightObj.transform.rotation = Quaternion.Euler(50f, -30f, 0f);
    }

    void CreateGround()
    {
        GameObject ground = GameObject.Find("GroundPlane");
        if (ground != null) return;

        ground = GameObject.CreatePrimitive(PrimitiveType.Plane);
        ground.name = "GroundPlane";
        ground.transform.position = new Vector3(0f, 0f, 40f); // place ahead so path is visible
        ground.transform.localScale = new Vector3(10f, 1f, 20f); // wide and long
        // set a simple material color
        var rend = ground.GetComponent<Renderer>();
        if (rend != null)
        {
            rend.material = new Material(Shader.Find("Standard"));
            rend.material.color = new Color(0.18f, 0.18f, 0.18f);
        }
    }

    GameObject CreateCar(string name, Vector3 size, Color color, Vector3 position)
    {
        GameObject go = GameObject.Find(name);
        if (go != null) return go;

        go = GameObject.CreatePrimitive(PrimitiveType.Cube);
        go.name = name;
        go.transform.localScale = size;
        go.transform.position = position;
        // give it a child "body" to avoid collider weirdness
        var r = go.GetComponent<Renderer>();
        if (r != null)
        {
            r.material = new Material(Shader.Find("Standard"));
            r.material.color = color;
        }

        // remove collider to avoid physics jitter unless needed
        var col = go.GetComponent<Collider>();
        if (col != null) Destroy(col);

        // make it a kinematic object by default (we'll move by transform)
        var rb = go.GetComponent<Rigidbody>();
        if (rb == null) rb = go.AddComponent<Rigidbody>();
        rb.isKinematic = true;

        return go;
    }

    T FindOrCreate<T>(string name) where T : Component
    {
        var existing = GameObject.FindObjectOfType<T>();
        if (existing != null) return existing;

        GameObject go = new GameObject(name);
        return go.AddComponent<T>();
    }
}
