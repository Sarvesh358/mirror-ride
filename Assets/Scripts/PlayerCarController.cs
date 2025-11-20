using UnityEngine;

public class PlayerCarController : MonoBehaviour
{
    public enum DriveMode { AutoSpeed, FullControl }
    public DriveMode mode = DriveMode.AutoSpeed;

    [Header("Common")]
    public float steerSpeed = 60f;        // degrees per second
    public float maxSpeed = 12f;          // units/sec for full-control
    public float brakeDecel = 30f;

    [Header("AutoSpeed Mode")]
    public float autoSpeed = 8f;          // default autopaced speed (overridden by GPS sync)
    public float autoSpeedSmoothing = 4f;

    float currentSpeed = 0f;
    float targetSpeed = 0f;
    float steerInput = 0f;

    void Update()
    {
        ReadInput();
        UpdateMovement(Time.deltaTime);
    }

    void ReadInput()
    {
        // Simple input: keyboard (for testing) and basic single-touch steering
        steerInput = Input.GetAxis("Horizontal"); // -1..1

        // mobile touch: left half = steer left, right half = steer right while held
        if (Input.touchCount > 0)
        {
            Touch t = Input.GetTouch(0);
            if (t.phase == TouchPhase.Stationary || t.phase == TouchPhase.Moved)
            {
                if (t.position.x < Screen.width / 2) steerInput = -1f;
                else steerInput = 1f;
            }
        }
    }

    void UpdateMovement(float dt)
    {
        if (mode == DriveMode.FullControl)
        {
            // acceleration via vertical axis or two-finger touch: simple keyboard control for now
            float accelInput = Input.GetAxis("Vertical"); // -1..1
            targetSpeed = accelInput * maxSpeed;
            // smooth speed
            currentSpeed = Mathf.MoveTowards(currentSpeed, targetSpeed, 30f * dt);
        }
        else // AutoSpeed
        {
            // targetSpeed should be controlled externally by GPSManager (set AutoSpeed via property)
            targetSpeed = autoSpeed;
            currentSpeed = Mathf.Lerp(currentSpeed, targetSpeed, Mathf.Clamp01(autoSpeedSmoothing * dt));
        }

        // braking - press space or touch with two fingers
        bool braking = Input.GetKey(KeyCode.Space) || Input.touchCount > 1;
        if (braking)
        {
            currentSpeed = Mathf.MoveTowards(currentSpeed, 0f, brakeDecel * dt);
        }

        // forward movement
        transform.position += transform.forward * currentSpeed * dt;

        // steering: rotate around up axis
        float turn = steerInput * steerSpeed * dt;
        transform.Rotate(0f, turn, 0f);
    }

    // Called by GPSManager or other code to set auto speed dynamically
    public void SetAutoSpeed(float speed)
    {
        autoSpeed = speed;
    }
}

