using UnityEngine;

public class RuntimeSceneBuilder : MonoBehaviour
{
    public GameObject playerPrefab; // optional, will be created as cube if null
    public GameObject ghostPrefab;

    void Awake()
    {
        // Player
        GameObject player = CreateOrUse(playerPrefab, "PlayerCar", Color.blue, new Vector3(0,0,0));
        PlayerCarController pc = player.AddComponent<PlayerCarController>();

        // Ghost
        GameObject ghost = CreateOrUse(ghostPrefab, "GhostCar", Color.red, new Vector3(0,0,10));
        GhostCarController gc = ghost.AddComponent<GhostCarController>();

        // Camera setup
        Camera cam = Camera.main;
        if (cam == null)
        {
            GameObject cgo = new GameObject("MainCamera");
            cam = cgo.AddComponent<Camera>();
            cgo.tag = "MainCamera";
        }
        SimpleCameraFollow camFollow = cam.gameObject.GetComponent<SimpleCameraFollow>();
        if (camFollow == null) camFollow = cam.gameObject.AddComponent<SimpleCameraFollow>();
        camFollow.target = player.transform;

        // Path
        PathManager pm = gameObject.AddComponent<PathManager>();
        pm.ghost = gc;

        // ScoreManager and UIManager (UI needs manual Text objects to display, but ScoreManager will exist)
        ScoreManager sm = gameObject.AddComponent<ScoreManager>();
        sm.player = player.transform;
        sm.ghost = ghost.transform;
    }

    GameObject CreateOrUse(GameObject prefab, string name, Color color, Vector3 pos)
    {
        GameObject go;
        if (prefab != null) go = Instantiate(prefab, pos, Quaternion.identity);
        else
        {
            go = GameObject.CreatePrimitive(PrimitiveType.Cube);
            go.transform.position = pos;
        }
        go.name = name;
        Renderer r = go.GetComponent<Renderer>();
        if (r != null) r.material.color = color;
        return go;
    }
}
