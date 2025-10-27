using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class NPCBase : MonoBehaviour, IInteractable
{
    [SerializeField] protected List<string> dialogues;
    [SerializeField] protected string npcName;
    protected bool choice = false;
    protected bool isInteracting = false;

    public TextMeshProUGUI interactionText;

    public virtual void Interact()
    {
        if (dialogues.Count == 0)
            return;

        if (!isInteracting)
        {
            isInteracting = true;
            StartDialogue();
        }
    }

    protected virtual void StartDialogue()
    {
        UIManager.Instance.StartDialogue(npcName, dialogues, OnChoiceSelected);
    }

    protected abstract void OnChoiceSelected(bool accepted);

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (interactionText != null)
            {
                interactionText.gameObject.SetActive(true);
            }
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (interactionText != null)
                interactionText.gameObject.SetActive(false);

            if (isInteracting)
            {
                isInteracting = false;
                UIManager.Instance.QuitInteract();
            }
        }
    }
}
