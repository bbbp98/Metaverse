using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;

public class NPCPortal : NPCBase
{
    public SceneType sceneType;

    //public override void Interact()
    //{
    //    if (isInteracting)
    //        return;

    //    //isInteracting = true;

    //    //UIManager.Instance.StartDialogue(gameObject.name, dialogues, OnChoiceSelected);
    //}

    protected override void OnChoiceSelected(bool accepted)
    {
        if (accepted)
        {
            GameManager.Instance.TransitionToScene(sceneType);
        }
        else
        {
            Debug.Log("대화 종료");
        }

        isInteracting = false;
    }

    protected override void OnTriggerStay2D(Collider2D collision)
    {
        base.OnTriggerStay2D(collision);
    }

    protected override void OnTriggerExit2D(Collider2D collision)
    {
        base.OnTriggerExit2D(collision);
    }
}
