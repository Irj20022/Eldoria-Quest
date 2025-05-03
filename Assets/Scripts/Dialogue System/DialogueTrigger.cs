using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    private DialogueHolder dialogueHolder;

    [SerializeField] private NPCDialogueData npcData;

    private void Awake()
    {
        dialogueHolder = GetComponent<DialogueHolder>();
    }

    public void TriggerDialogue()
    {
        Debug.Log("TriggerDialogue called");

        if (DialogueManager.Instance == null)
        {
            Debug.LogError("DialogueManager not found in scene!");
            return;
        }

        if (dialogueHolder == null || dialogueHolder.rootNode == null)
        {
            Debug.LogError("DialogueHolder or rootNode is missing!");
            return;
        }

        DialogueManager.Instance.StartDialogue(dialogueHolder.rootNode, npcData);
    }

}
