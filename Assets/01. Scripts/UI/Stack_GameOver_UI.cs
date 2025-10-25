using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Stack_GameOver_UI : Stack_Base_UI
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button endButton;

    [SerializeField] private TextMeshProUGUI nowScoreText;
    [SerializeField] private TextMeshProUGUI nowComboText;

    [SerializeField] private TextMeshProUGUI bestScoreText;
    [SerializeField] private TextMeshProUGUI bestComboText;

    [SerializeField] private TextMeshProUGUI earnGoldText;

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

    public void SetUI(int nowScore, int nowCombo, int bestScore, int bestCombo)
    {
        nowScoreText.text = nowScore.ToString();
        nowComboText.text = nowCombo.ToString();

        bestScoreText.text = bestScore.ToString();
        bestComboText.text = bestCombo.ToString();

        earnGoldText.text = $"+ {nowScore} G";
    }
}
