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
    private int score = 0;
    private int bestScore = 0;
    private float scorePerSecond = 10f;
    private const string BestScoreKey = "M_LizardRun_BestScore";

    public TextMeshProUGUI bestScoreText;
    public TextMeshProUGUI nowScoreText;

    public GameObject panel;
    public Button startButton;
    public Button exitButton;

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
        SetBestScore(bestScore);
    }

    private void Update()
    {
        if (isPlaying)
        {
            fScore += scorePerSecond * Time.deltaTime;
        }
        score = (int)fScore;
        SetScore(score);
    }

    public void StartGame()
    {
        if (isPlaying) return;

        isPlaying = true;
        panel.SetActive(false);

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

    public void ExitGame()
    {
        GameManager.Instance.TransitionToScene(SceneType.MainScene);
    }
}
