using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    //[Header("Managers")]
    //public UIManager uiManager;
    //public FadeManager fadeManager;

    [Header("Game State")]
    public SceneType currentLevel;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        currentLevel = SceneType.MainScene;

        if (PlayerManager.Instance != null)
            PlayerManager.Instance.OnGoldChanged += UpdateGoldUI;

        StartGame();
    }

    public void StartGame()
    {
        FadeManager.Instance.FadeIn();
        //uiManager.UpdateUI();
    }

    public void LoadScene(SceneType sceneType)
    {
        currentLevel = sceneType;
        FadeManager.Instance.FadeToScene(currentLevel);
    }

    private void UpdateGoldUI(int currentGold)
    {
        if (currentLevel == SceneType.StackScene)
            return;

        if (UIManager.Instance != null)
            UIManager.Instance.SetGold(currentGold);
    }
}
