using UnityEngine;

public class Bootstrap : MonoBehaviour
{
    void Awake()
    {
        // Create an empty object that has StarterController
        GameObject starter = new GameObject("Starter");
        starter.AddComponent<StarterController>();
    }
}
