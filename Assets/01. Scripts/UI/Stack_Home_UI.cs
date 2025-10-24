using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Stack_Home_UI : Stack_Base_UI
{
    [SerializeField] private Button startButton;
    [SerializeField] private Button endButton;

    protected override UIState GetUIState()
    {
        return UIState.Home;
    }

    public override void Init(Stack_UIManager uiManager)
    {
        base.Init(uiManager);

        startButton.onClick.AddListener(OnClickStartButton);
        endButton.onClick.AddListener(OnClickEndButton);
    }

    private void OnClickStartButton()
    {
        uiManager.OnClickStart();
    }

    private void OnClickEndButton()
    {
        uiManager.OnClickEnd();
    }
}
