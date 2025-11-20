using UnityEngine;
using UnityEngine.UI;
using TMPro; // if TextMeshPro is available

public class UIManager : MonoBehaviour
{
    public PlayerCarController playerController;
    public ScoreManager scoreManager;
    public Text scoreText;
    public Text modeText; // simple UI Text; if using TMP, replace type accordingly

    void Update()
    {
        if (scoreManager != null && scoreText != null)
            scoreText.text = $"Score: {Mathf.FloorToInt(scoreManager.GetScore())}";

        if (playerController != null && modeText != null)
            modeText.text = $"Mode: {playerController.mode.ToString()}";
    }

    // Called by on-screen button to toggle modes
    public void ToggleMode()
    {
        if (playerController == null) return;
        if (playerController.mode == PlayerCarController.DriveMode.AutoSpeed)
            playerController.mode = PlayerCarController.DriveMode.FullControl;
        else
            playerController.mode = PlayerCarController.DriveMode.AutoSpeed;
    }
}
