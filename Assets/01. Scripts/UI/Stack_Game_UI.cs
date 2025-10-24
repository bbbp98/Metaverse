using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Stack_Game_UI : Stack_Base_UI
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI comboText;

    protected override UIState GetUIState()
    {
        return UIState.Game;
    }

    public override void Init(Stack_UIManager uiManager)
    {
        base.Init(uiManager);
    }

    public void SetUI(int score, int combo)
    {
        scoreText.text = score.ToString();
        comboText.text = combo.ToString();
    }
}
