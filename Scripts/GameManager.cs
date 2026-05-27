using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    public int score = 0;
    public int bestScore = 0;
    public int episodeCount = 0;
    public int collisionCount = 0;
    public int coinsCollected = 0;
    public int currentSteps = 0;
    public int bestScoreEpisode = 0;

    public TMP_Text statsText;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        UpdateUI();
    }

    public void StartEpisode()
    {
        episodeCount++;
        score = 0;
        collisionCount = 0;
        coinsCollected = 0;
        currentSteps = 0;

        Debug.Log("Episode " + episodeCount + " started");
        UpdateUI();
    }

    public void AddScore(int value)
    {
        score += value;

        if (score > bestScore)
        {
            bestScore = score;
            bestScoreEpisode = episodeCount;
        }

        UpdateUI();
    }

    public void AddCoin()
    {
        coinsCollected++;
        UpdateUI();
    }

    public void AddCollisionPenalty(int penalty)
    {
        collisionCount++;
        score -= penalty;
        UpdateUI();
    }

    public void UpdateSteps(int steps)
    {
        currentSteps = steps;
        UpdateUI();
    }

    public void EndEpisode(int stepsUsed)
    {
        currentSteps = stepsUsed;
        UpdateUI();

        Debug.Log(
            "Episode " + episodeCount +
            " finished | Score: " + score +
            " | Best Score: " + bestScore +
            " | Coins: " + coinsCollected +
            " | Collisions: " + collisionCount +
            " | Steps: " + stepsUsed
        );
    }

    private void UpdateUI()
    {
        if (statsText != null)
        {
            statsText.text =
                "Episode: " + episodeCount + "\n" +
                "Score: " + score + "\n" +
                "Best Score: " + bestScore + " (Ep. " + bestScoreEpisode + ")\n" +
                "Coins: " + coinsCollected + "\n" +
                "Collisions: " + collisionCount + "\n" +
                "Steps: " + currentSteps;
        }
    }
}