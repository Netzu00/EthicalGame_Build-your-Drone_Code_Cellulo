using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    //public TextMeshProUGUI nameText; //Not using for now
    public TextMeshProUGUI dialogueText;
    private Queue<string> sentences; //Load sentences as read through dialog
   
    void Start() {
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue){
        //Debug.Log("Starting dialogue :" + dialogue.name);
        //nameText.text = dialogue.name;

        sentences.Clear(); //clear previous 

        foreach(string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();

    }

    public void DisplayNextSentence() {
        //reach end of queue
        if(sentences.Count == 0) {
            EndDialogue();
            return;
        } 
        string sentence = sentences.Dequeue();

        //Debug.Log(sentence);
        StopAllCoroutines();//Stop if click continue before last coroutine ended
        StartCoroutine(TypeSentence(sentence));
    }

    IEnumerator TypeSentence(string sentence) {
        dialogueText.text = "";
        foreach(char letter in sentence.ToCharArray()) {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.08f);
        }
    }

    void EndDialogue() {
        Debug.Log("End of conversation");
    }
}
