using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTrigger : Button
{
    public Dialogue dialogue;
    public TextMeshProUGUI dialogueTextBox;

    public void TriggerDialogue () {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue, dialogueTextBox);
    }
  
}
