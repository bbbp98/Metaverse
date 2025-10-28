using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LizardRunGameManager : MonoBehaviour
{
    public static LizardRunGameManager Instance;

    private bool isPlaying = false;

    private float fScore = 0f;
    private int currentScore = 0;
    private int bestScore = 0;
    private float scorePerSecond = 10f;
    private int nextSpeedUpScore = 100;
    private int speedUpInterval = 100;
    private const string BestScoreKey = "M_LizardRun_BestScore";

    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI nowScoreText;

    public GameObject gameStartUI;
    public Button startButton;
    public Button exitButton;
    public GameObject gameOverUI;

    public AudioClip speedUpSfx;
    private float speedIncrease = 1f;
    private float maxSpeed = 15f;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        bestScore = PlayerPrefs.GetInt(BestScoreKey, 0);

        startButton.onClick.AddListener(StartGame);
        exitButton.onClick.AddListener(ExitGame);
    }

    private void Start()
    {
        Lizard.Instance.Unfreeze();
        gameStartUI.SetActive(true);
        SetBestScore(bestScore);
        SetScore(0);
    }

    private void Update()
    {
        if (isPlaying)
        {
            fScore += scorePerSecond * Time.deltaTime;
        }

        currentScore = (int)fScore;
        SetScore(currentScore);

        if (currentScore % 100 == 0)
            CheckSpeedUp(currentScore);
    }

    private void CheckSpeedUp(int score)
    {
        if (score >= nextSpeedUpScore)
        {
            IncreasePlayerSpeed();
            nextSpeedUpScore += speedUpInterval;
        }
    }

    private void IncreasePlayerSpeed()
    {
        AudioManager.Instance.PlaySfx(speedUpSfx);
        float playerSpeed = Lizard.Instance.GetMoveSpeed();
        playerSpeed += speedIncrease;
        playerSpeed = Mathf.Min(playerSpeed, maxSpeed);
        Lizard.Instance.SetMoveSpeed(playerSpeed);
        Debug.Log("speed up");
    }

    public void StartGame()
    {
        if (isPlaying) return;

        isPlaying = true;
        gameStartUI.SetActive(false);

        ObstacleManager.Instance.StartSpawning();
        Lizard.Instance.EnableControl();
    }

    public void GameOver()
    {
        isPlaying = false;

        ObstacleManager.Instance.StopSpawning();
        Lizard.Instance.DisableControl();
        PlayerPrefs.SetInt(BestScoreKey, currentScore);
        PlayerManager.Instance.AddGold(currentScore);
        gameOverUI.SetActive(true);
    }

    public void SetBestScore(int score)
    {
        bestScoreText.text = score.ToString();
    }

    public void SetScore(int score)
    {
        nowScoreText.text = score.ToString();
    }

    public void ExitGame()
    {
        GameManager.Instance.TransitionToScene(SceneType.MainScene);
    }

    public void RestartGame()
    {
        GameManager.Instance.TransitionToScene(SceneType.LizardRunScene);
        gameOverUI.SetActive(false);
    }
}
