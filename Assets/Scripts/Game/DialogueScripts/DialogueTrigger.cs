using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueTrigger : Button
{
    public Dialogue dialogue;
    public TextMeshProUGUI dialogueTextBox;

    //dont allow same button to trigger dialogue several times
    //This fix was implemented to avoid the missionStatementScene continue button to restart dialogue on even click
    public bool allowRestart = true; //default behavior
    private bool hasBeenTriggered = false;

    public void TriggerDialogue () {
        if(!hasBeenTriggered || allowRestart) {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue, dialogueTextBox);
            hasBeenTriggered = true;
            Debug.Log("trigger Dialogue of buttom with name: " + this.name);
        }
    }
  
}
