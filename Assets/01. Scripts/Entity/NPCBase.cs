using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public abstract class NPCBase : MonoBehaviour, IInteractable
{
    //public string npcName;
    public abstract void Interact();
    public TextMeshProUGUI interactionText;

    protected virtual void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (interactionText != null)
                interactionText.gameObject.SetActive(true);
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (interactionText != null)
                interactionText.gameObject.SetActive(false);
        }
    }
}
