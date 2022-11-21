using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    //public TextMeshProUGUI nameText; //Not using for now
    private TextMeshProUGUI dialogueText;
    private int currentSentence = 0;
    public bool finishedDialogue = false;
    private Queue<string> sentences; //Load sentences as read through dialog

    private bool waitTillFinishTyping = false;

    void Start() {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue, TextMeshProUGUI dialogueTextBox){
        //Debug.Log("Starting dialogue :" + dialogue.name);
        //nameText.text = dialogue.name;
        finishedDialogue = false;
        sentences.Clear(); //clear previous 
        dialogueText = dialogueTextBox;
        foreach(string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();

    }
    public void DisplayNextSentence() {
        //reach end of queue
        if(sentences.Count == 0) {
            finishedDialogue = true;
            EndDialogue();
            
            return;
        } 
        //Debug.Log(sentence);
        if(waitTillFinishTyping == false) {
            waitTillFinishTyping = true;
            string sentence = sentences.Dequeue();
            StopAllCoroutines();//Stop if click continue before last coroutine ended
            StartCoroutine(TypeSentence(sentence));
        }
    }

    //Code for animating the typing of sentence letter by letter
    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.05f);
        }
        waitTillFinishTyping = false;
    }

    void EndDialogue() {
        Debug.Log("End of conversation");
    }
}
