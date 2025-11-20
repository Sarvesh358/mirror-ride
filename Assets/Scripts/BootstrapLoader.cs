using UnityEngine;

[DefaultExecutionOrder(-1000)]
public class BootstrapLoader : MonoBehaviour
{
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    static void OnLoad()
    {
        GameObject go = new GameObject("Bootstrap");
        go.AddComponent<Bootstrap>();
    }
}
