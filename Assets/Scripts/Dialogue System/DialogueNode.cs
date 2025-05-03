using UnityEngine;

[System.Serializable]
public class DialogueNode
{
    [TextArea]
    public string dialogueText;

    public string[] playerChoices = new string[3];
    public string[] npcResponses = new string[3];

    public DialogueNode[] nextNodes = new DialogueNode[3]; 
}
