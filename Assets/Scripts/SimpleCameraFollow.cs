using UnityEngine;

[RequireComponent(typeof(Camera))]
public class SimpleCameraFollow : MonoBehaviour
{
    public Transform target;        // assign player car transform
    public Vector3 offset = new Vector3(0, 5, -8);
    public float smoothTime = 0.15f;
    Vector3 velocity = Vector3.zero;

    void LateUpdate()
    {
        if (target == null) return;
        Vector3 desiredPos = target.position + target.TransformDirection(offset);
        transform.position = Vector3.SmoothDamp(transform.position, desiredPos, ref velocity, smoothTime);
        transform.LookAt(target.position + Vector3.up * 1.2f);
    }
}
