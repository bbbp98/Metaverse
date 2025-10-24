using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stack_GameOver_UI : Stack_Base_UI
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button endButton;

    protected override UIState GetUIState()
    {
        return UIState.GameOver;
    }

    public override void Init(Stack_UIManager uiManager)
    {
        base.Init(uiManager);

        restartButton.onClick.AddListener(OnClickStartButton);
        endButton.onClick.AddListener(OnClickEndButton);
    }

    public void OnClickStartButton()
    {
        uiManager.OnClickStart();
    }

    public void OnClickEndButton()
    {
        uiManager.OnClickEnd();
    }
}
