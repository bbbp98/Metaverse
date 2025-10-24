using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public enum UIState
{
    Home,
    Game,
    GameOver,
}

public class Stack_UIManager : MonoBehaviour
{
    private static Stack_UIManager instance;
    public static Stack_UIManager Instance { get { return instance; } }

    UIState currentState = UIState.Home;

    TheStack theStack = null;

    Stack_Home_UI homeUI = null;
    Stack_Game_UI gameUI = null;
    Stack_GameOver_UI gameOverUI = null;

    private void Awake()
    {
        if (Instance == null)
            instance = this;

        theStack = FindObjectOfType<TheStack>();

        homeUI = GetComponentInChildren<Stack_Home_UI>(true);
        homeUI?.Init(this);

        gameUI = GetComponentInChildren<Stack_Game_UI>(true);
        gameUI?.Init(this);

        gameOverUI = GetComponentInChildren<Stack_GameOver_UI>(true);
        gameOverUI?.Init(this);

        ChangeState(UIState.Home);
    }

    public void ChangeState(UIState state)
    {
        currentState = state;

        homeUI?.SetActive(currentState);
        gameUI?.SetActive(currentState);
        gameOverUI?.SetActive(currentState);
    }

    public void OnClickStart()
    {
        theStack.Restart();
        ChangeState(UIState.Game);
    }

    public void OnClickEnd()
    {
        FadeManager.Instance.FadeToScene(SceneType.MainScene);
    }

    public void UpdateScore()
    {
        gameUI.SetUI(theStack.Score, theStack.Combo);
    }

    public void SetGameOverUI()
    {
        //scoreUI.SetUI(theStack.Score, theStack.MaxCombo, theStack.BestScore, theStack.BestCombo);
        ChangeState(UIState.GameOver);
    }
}
