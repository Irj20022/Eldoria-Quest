using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [SerializeField] private float interactRange = 2f;
    [SerializeField] private LayerMask npcLayer;
    [SerializeField] private KeyCode interactKey = KeyCode.E;

    private DialogueTrigger currentNPC;

    private void Update()

    {
        DetectNPC();

        // Trigger dialogue
        if (currentNPC != null && Input.GetKeyDown(interactKey))
        {
            currentNPC.TriggerDialogue();
        }

        // Close dialogue if player walks away
        if (DialogueManager.Instance != null && DialogueManager.Instance.IsDialogueActive)
        {
            if (currentNPC == null || Vector2.Distance(transform.position, currentNPC.transform.position) > interactRange + 1f)
            {
                DialogueManager.Instance.HideDialogue();
            }
        }
    }

    private void DetectNPC()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRange, npcLayer);

        if (hit != null)
        {
            currentNPC = hit.GetComponent<DialogueTrigger>();
        }
        else
        {
            currentNPC = null;
        }
    }

    // Optional: visualize interaction range
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, interactRange);
    }
}
