using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LizardRunGameManager : MonoBehaviour
{
    public static LizardRunGameManager Instance;

    private bool isPlaying = false;

    private int score = 0;
    private int bestScore = 0;
    private float scorePerSecond = 10f;
    private const string BestScoreKey = "M_LizardRun_BestScore";

    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI nowScoreText;
    

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);
    }

    private void Start()
    {
        SetBestScore(bestScore);
    }

    private void Update()
    {
        if (isPlaying)
        {
            score += Mathf.FloorToInt(scorePerSecond * Time.deltaTime);
        }

        SetScore(score);
    }

    public void StartGame()
    {
        if (isPlaying) return;

        isPlaying = true;

        ObstacleManager.Instance.StartSpawning();
        Lizard.Instance.EnableControl();
    }

    public void GameOver()
    {
        isPlaying = false;

        ObstacleManager.Instance.StopSpawning();
        Lizard.Instance.DisableControl();
    }

    public void SetBestScore(int score)
    {
        bestScoreText.text = score.ToString();
    }

    public void SetScore(int score)
    {
        nowScoreText.text = score.ToString();
    }
}
