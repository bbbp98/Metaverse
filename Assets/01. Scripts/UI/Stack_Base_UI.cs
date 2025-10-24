using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Stack_Base_UI : MonoBehaviour
{
    protected Stack_UIManager uiManager;

    public virtual void Init(Stack_UIManager uiManager)
    {
        this.uiManager = uiManager;
    }

    protected abstract UIState GetUIState();

    public void SetActive(UIState state)
    {
        gameObject.SetActive(GetUIState() == state);
    }
}
