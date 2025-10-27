using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [SerializeField] private TextMeshProUGUI coinText;

    public GameObject dialoguePanel;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI NPCNameText;
    public GameObject choicePanel;
    public Button yesButton;
    public Button noButton;

    [SerializeField] private float typingSpeed = 0.05f;

    private Coroutine typingCoroutine;
    private Queue<string> dialogueQueue = new Queue<string>();
    private Action<bool> onChoiceSelected;

    private bool isTyping = false;
    private bool isChoicePhase = false;
    private string currentText = "";

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
            Destroy(gameObject);

        yesButton.onClick.AddListener(() => OnChoice(true));
        noButton.onClick.AddListener(() => OnChoice(false));
    }

    private void Update()
    {
        if (!dialoguePanel.activeSelf || isChoicePhase)
            return;

        if (Input.GetKeyDown(KeyCode.E) || Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = currentText;
                isTyping = false;
            }
            else
            {
                DisplayNextSentence();
            }
        }
    }

    public void SetGold(int currentGold)
    {
        if (coinText != null)
            coinText.text = currentGold.ToString();
    }

    public void StartDialogue(string NpcName, List<string> sentences, Action<bool> onChoiceCallback)
    {
        NPCNameText.text = NpcName;

        dialogueQueue.Clear();
        foreach (var s in sentences)
            dialogueQueue.Enqueue(s);

        onChoiceSelected = onChoiceCallback;
        dialoguePanel.SetActive(true);
        choicePanel.SetActive(false);

        DisplayNextSentence();
    }

    private void DisplayNextSentence()
    {
        if (dialogueQueue.Count == 0)
        {
            ShowChoice();
            return;
        }

        string sentence = dialogueQueue.Dequeue();
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeTextEffect(sentence));
    }


    private IEnumerator TypeTextEffect(string text)
    {
        text = text.Replace("\\n", "\n");
        isTyping = true;
        currentText = text;
        dialogueText.text = "";

        foreach (char c in text)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
    }

    private void ShowChoice()
    {
        isChoicePhase = true;
        choicePanel.SetActive(true);
    }

    private void OnChoice(bool accepted)
    {
        isChoicePhase = false;
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);
        onChoiceSelected?.Invoke(accepted);

        EventSystem.current.SetSelectedGameObject(null); // initialize button color
    }

    public void QuitInteract()
    {
        StopCoroutine(typingCoroutine);
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);
    }
}
