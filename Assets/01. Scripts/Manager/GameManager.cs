using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        StartCoroutine(FadeManager.Instance.FadeIn());
        AudioManager.Instance.PlayBgm(currentLevel);
        UIManager.Instance.SetGold(PlayerManager.Instance.GetPlayerData().Gold);
    }

    public void TransitionToScene(SceneType sceneType)
    {
        currentLevel = sceneType;
        StartCoroutine(SceneTransitonCoroutine(currentLevel));
    }

    private IEnumerator SceneTransitonCoroutine(SceneType sceneType)
    {
        yield return FadeManager.Instance.FadeOut();

        SceneManager.LoadScene(sceneType.ToString());
        AudioManager.Instance.PlayBgm(sceneType);

        if (currentLevel == SceneType.StackScene || currentLevel == SceneType.LizardRunScene)
            UIManager.Instance.gameObject.SetActive(false);
        else
            UIManager.Instance.gameObject.SetActive(true);

        yield return FadeManager.Instance.FadeIn();
    }


    private void UpdateGoldUI(int currentGold)
    {
        if (UIManager.Instance != null)
            UIManager.Instance.SetGold(currentGold);
    }
}
