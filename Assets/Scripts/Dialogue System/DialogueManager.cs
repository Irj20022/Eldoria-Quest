using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject dialoguePanel;
    [SerializeField] private TMP_Text nameText;
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private Image portraitImage;
    [SerializeField] private Button[] choiceButtons;
    [SerializeField] private Button continueButton;

    [Header("Typewriter Settings")]
    [SerializeField] private float typingSpeed = 0.02f;

    [Header("Animator")]
    [SerializeField] private Animator animator;

    private DialogueNode currentNode;
    private bool isTyping = false;

    public System.Action OnDialogueEnd;

    public bool IsDialogueActive => dialoguePanel.activeSelf;

    public static DialogueManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }

    public void StartDialogue(DialogueNode startNode, NPCDialogueData npcData)
    {
        currentNode = startNode;

        if (currentNode == null)
        {
            Debug.LogError("StartDialogue: currentNode is null! Check if the DialogueHolder has a valid rootNode.");
            return;
        }

        Debug.Log("Starting dialogue: " + currentNode.dialogueText);

        // Set NPC visuals
        nameText.text = npcData.npcName;
        portraitImage.sprite = npcData.npcPortrait;

        // Hide all choice buttons initially
        foreach (Button btn in choiceButtons)
        {
            btn.transform.localScale = Vector3.zero;

            CanvasGroup cg = btn.GetComponent<CanvasGroup>();
            if (cg != null)
            {
                cg.alpha = 0f;
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }
        }

        // Show and animate the dialogue panel
        dialoguePanel.SetActive(true);
        StartCoroutine(PlaySlideInNextFrame());




        // Begin typing dialogue
        StartCoroutine(TypeTextRoutine(currentNode.dialogueText));
    }
    private IEnumerator PlaySlideInNextFrame()
    {
        yield return null; // wait 1 frame
        animator.SetTrigger("SlideIn");
    }


    private IEnumerator TypeTextRoutine(string fullText)
    {
        isTyping = true;
        dialogueText.text = "";

        for (int i = 0; i < fullText.Length; i++)
        {
            dialogueText.text = fullText.Substring(0, i + 1);
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        ShowChoices();
    }

    private void ShowChoices()
    {
        bool hasChoices = currentNode.playerChoices != null &&
                          currentNode.playerChoices.Length > 0 &&
                          !string.IsNullOrEmpty(currentNode.playerChoices[0]);

        if (hasChoices)
        {
            continueButton.gameObject.SetActive(false);

            for (int i = 0; i < choiceButtons.Length; i++)
            {
                if (i < currentNode.playerChoices.Length && !string.IsNullOrEmpty(currentNode.playerChoices[i]))
                {
                    TMP_Text buttonText = choiceButtons[i].GetComponentInChildren<TMP_Text>();
                    buttonText.text = currentNode.playerChoices[i];

                    int choiceIndex = i;
                    choiceButtons[i].onClick.RemoveAllListeners();
                    choiceButtons[i].onClick.AddListener(() => OnChoiceSelected(choiceIndex));

                    CanvasGroup cg = choiceButtons[i].GetComponent<CanvasGroup>();
                    if (cg != null)
                    {
                        cg.alpha = 1f;
                        cg.interactable = true;
                        cg.blocksRaycasts = true;
                    }

                    choiceButtons[i].transform.localScale = Vector3.one * 0.01f;
                    choiceButtons[i].gameObject.SetActive(true);
                    StartCoroutine(ScalePopIn(choiceButtons[i].transform));
                }
                else
                {
                    choiceButtons[i].gameObject.SetActive(false);
                }
            }
        }
        else
        {
            foreach (Button btn in choiceButtons)
            {
                btn.gameObject.SetActive(false);
            }

            continueButton.gameObject.SetActive(true);
            continueButton.onClick.RemoveAllListeners();
            continueButton.onClick.AddListener(() =>
            {
                DialogueNode nextNode = currentNode.nextNodes != null && currentNode.nextNodes.Length > 0
                    ? currentNode.nextNodes[0]
                    : null;

                if (nextNode != null)
                {
                    currentNode = nextNode;
                    continueButton.gameObject.SetActive(false);
                    StartCoroutine(TypeTextRoutine(currentNode.dialogueText));
                }
                else
                {
                    HideDialogue();
                }
            });
        }
    }

    private void OnChoiceSelected(int index)
    {
        foreach (Button btn in choiceButtons)
        {
            btn.gameObject.SetActive(false);
        }

        string response = currentNode.npcResponses[index];
        DialogueNode nextNode = currentNode.nextNodes != null && currentNode.nextNodes.Length > index
            ? currentNode.nextNodes[index]
            : null;

        StartCoroutine(ShowResponseAndContinue(response, nextNode));
    }

    private IEnumerator ShowResponseAndContinue(string response, DialogueNode nextNode)
    {
        yield return TypeTextRoutine(response);
        yield return new WaitForSeconds(1f);

        if (nextNode != null)
        {
            currentNode = nextNode;
            StartCoroutine(TypeTextRoutine(currentNode.dialogueText));
        }
        else
        {
            HideDialogue();
        }
    }

    private IEnumerator ScalePopIn(Transform target)
    {
        float time = 0f;
        float duration = 0.2f;
        Vector3 overshoot = Vector3.one * 1.1f;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = time / duration;
            target.localScale = Vector3.Lerp(Vector3.zero, overshoot, t);
            yield return null;
        }

        target.localScale = Vector3.one;
    }

    public void HideDialogue()
    {
        dialoguePanel.SetActive(false);

        
        if (continueButton != null)
            continueButton.gameObject.SetActive(false);

        
        foreach (Button btn in choiceButtons)
        {
            btn.gameObject.SetActive(false);
        }

        OnDialogueEnd?.Invoke();
    }

}
