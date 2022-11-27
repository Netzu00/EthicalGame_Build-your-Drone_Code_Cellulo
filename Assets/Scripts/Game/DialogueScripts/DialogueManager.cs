using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class DialogueManager : MonoBehaviour
{
    //public TextMeshProUGUI nameText; //Not using for now
    public Button continueButton; //TODO make this button set upon startDialogue!!!!!!!!!!! now its always the main continue button
    public Button acceptButton; //Update drones ranges and and go to next sentence if there is one.
    public Button refuseButton; //same as accept but doesnt updateDroneRanges
    public GameControler gameController; //Needed to notify gameController that drone rangesAndBalance can be updated based on choice to accept or refuse.
    //Alternatively choice could just notify gameController.acceptRefuseChoiceMade();
    private TextMeshProUGUI dialogueText;
    public bool isMainTabText = false;
    private int currentSentence = 0;
    public bool finishedDialogue = false;
    private Queue<string> sentences; //Load sentences as read through dialog

    private bool waitTillFinishTyping = false;

    void Start() {
        sentences = new Queue<string>();
    }

    /**
    Setup dialogue sentences and dialogue box and begin by displaying first sentence.
    */
    public void StartDialogue(Dialogue dialogue, TextMeshProUGUI dialogueTextBox, bool isMainTabTextBox){
        isMainTabText = isMainTabTextBox; //if in main tab then will want to spawn buttons else no
        //Debug.Log("Starting dialogue :" + dialogue.name);
        //nameText.text = dialogue.name;
        continueButton.gameObject.SetActive(true);
        finishedDialogue = false;
        sentences.Clear(); //clear previous 
        dialogueText = dialogueTextBox;
        foreach(string sentence in dialogue.sentences) {
            sentences.Enqueue(sentence);
        }
        DisplayNextSentence();

    }
    
    /**
    Display next sentence in queue
    */
    public void DisplayNextSentence() {
        //reach end of queue
        if(sentences.Count == 0) {
            EndDialogue();
            finishedDialogue = true;
            return;
        } 
        
        //If have reached the the texts with possible subchoices and we are in the mainTabText
        //Then spawn accept and refuse buttons to make the subChoices
        if(sentences.Count <= gameController.numSubChoices[gameController.latestChoiceId] && isMainTabText) {
            spawnRefuseAcceptButtons();
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
            yield return new WaitForSeconds(0.005f);
        }
        waitTillFinishTyping = false;
    }

    void EndDialogue() {
        continueButton.gameObject.SetActive(false);
        Debug.Log("End of conversation");
    }

    void spawnRefuseAcceptButtons(){
        refuseButton.gameObject.SetActive(true);
        acceptButton.gameObject.SetActive(true);
        continueButton.gameObject.SetActive(false);
    }

    public void acceptChanges(){
        refuseButton.gameObject.SetActive(false);
        acceptButton.gameObject.SetActive(false);
        if(sentences.Count > 0){
            continueButton.onClick.Invoke();
            continueButton.gameObject.SetActive(true);
            gameController.incrementSubChoiceNum();
        }
         gameController.updateDroneRangesAndBalance();  
        
    }

    public void refuseChanges() {
        if(sentences.Count > 0){
            continueButton.onClick.Invoke();
            continueButton.gameObject.SetActive(true);
            gameController.incrementSubChoiceNum();
        }
        refuseButton.gameObject.SetActive(false);
        acceptButton.gameObject.SetActive(false);
        
    }
}
