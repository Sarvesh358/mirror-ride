using UnityEngine;

public class ScoreManager : MonoBehaviour
{
    public Transform player;
    public Transform ghost;
    public float score = 0f;
    public float scoringRadius = 3f; // meters - within this distance counts as "in sync"
    public float pointsPerSecond = 10f;
    float comboTime = 0f;

    void Update()
    {
        if (player == null || ghost == null) return;
        float dist = Vector3.Distance(player.position, ghost.position);
        if (dist <= scoringRadius)
        {
            score += pointsPerSecond * Time.deltaTime;
            comboTime += Time.deltaTime;
        }
        else
        {
            comboTime = 0f;
        }
    }

    public float GetScore() => score;
    public float GetComboTime() => comboTime;
}
