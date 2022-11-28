using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//TODO

/*
W10-11
4)Work on Story, thats it..
=> Make more choices and (tweak) how they affect drone specs
=> Map drone specs to final outcome text, maybe each var gets 1 small paragraph + 1 overall evaluation
W12
5)Test with a few players and take notes(1 week) + tweak game a little more
W13-14
6)
=> Program cellulo's to make choices
=> Make cellulo map and extend cellulo's with light effects
Xmas holidays
7)Write report and document code
*/

//General ideas: 
/*
    Most birds dont care about color, except birds of pray will attack
    Could include video of birds attack drone or something during testing! or just a picture
    multi-colored reflective tape/ or lights discourage bird attacks, aswell as loud sounds(or maybe cat?xd)
    //Googly eyes seem to work aswell..juju
*/

//TODO use the choices enum instead of ints

public enum choices
{
	None, 
	DroneExpert, //1
    BirdExpert, //expert on specific bird (choice2)
    TestLocally, //test in backyard (choice id: 3)
    //OnSiteVisit, //nothern ireland lots of rain and WIND! in winter for example
    OnFieldTesting, //choice id: 4
    macroTesting, //choice id: 5
    shipIt, //choice id: 6
}

public class GameControler : MonoBehaviour
{
    //TODO perhaps make new script for missionStatementScene so its less cluttered
    //Intro page 
    public DialogueTrigger MissionStatementTriggerButton; //TODO try to trigger on awake using text for now try with button
    public Dialogue MissionStatementDialogue;
    public TextMeshProUGUI MissionStatementDialogueTextBox;
    //Final outcome dialogue box
    public DialogueTrigger finalDialogueTriggerButton; 
    public Dialogue finalOutcomeDialogue;
    public TextMeshProUGUI finalOutcomeDialogueTextBox;

    //Drone Specs
    private static bool has_wetsuit = false;
    public TextMeshProUGUI droneSpecsText;
    public static List<string> colorList = new List<string>{"white", "purple", "blue"};
    public static List<string> materialList = new List<string>{"carbon fiber", "mat2", "mat3"};
    public static int[] droneSizeRange = {20, 150}; //min and max size range
    public static double[] droneWeightRange = {0.5, 10};

    //Main Tab Feedback text
    public DialogueTrigger EnterButton;
    public TextMeshProUGUI dialogueTextBox;
    public TextMeshProUGUI scrollBarText; //Contains text currently displayed in scrollBar

    //Array of locked choice and choice selection objects
    List<int> locked_choices = new List<int>(); //List of choices locked in by the players
    public DropSlot slot; //slot where choice is dropped into

    /* Tabs and dialogues -------------------------------------------------------*/
    //public DialogueManager dialogueManager;
    public Button mainTab;
    private int mainTabIndexInLayout; //Used to make sure the "main" tab is always to the right of all tabs
    public TabController tabController;
    //Array of dialogues 
    [SerializeField] private List<Dialogue> choiceFeedbackDialogues;//set in unity directly
    private string[] finalOutcomeDialogueSentences = {"var 0", "var 1", 
    "var 2", "var 3", "var 4", "var 5", "var 6", "var 7", "var 8", "var 9", "var 10"};

    /*ACCEPT/REFUSE------------------------------------------------------------------*/
    public int[] numSubChoices; //maps Choices => int representing number of subchoices for this main choice. 
    //(Necessary to know when to start using accept/refuse buttons)
    public int latestChoiceId = 0;
    int acceptedSubChoiceNumber = 0; //represents the current subchoice withing the main choice
    bool accept = false; //record user's choice
    //These buttons spawn when player must make choice of accepting to refusing the proposed changes from the expert
    public Button acceptButton; 
    public Button refuseButton;

    /*Money and Time System ---------------------------------------------------------------*/
    public TextMeshProUGUI availableBalanceText;
    public TextMeshProUGUI remainingTimeText;
    static public float remainingTime = 14; //number of weeks remaning till project deadline 
    static public int availableBalance = 1000; //set in unity and updated through code
    public int[] mainChoiceFinancialCosts; //Costs of each main choice, set in unity
    public float[] mainChoiceTimeCosts; //timeCosts of each main choice, set in unity

    //NOTE can have more then 3 subChoices per choice with this method!!! but that should be more then enough
    //subChoiceIndex = mainChoiceId * 3 + subChoiceNum
    public int[] subChoiceFinancialCosts; //Costs of each choice, set in unity
    public float[] subChoiceTimeCosts; //timeCosts of each subchoice, set in Unity

    // -----------------------------------------------------------------------------
    void Start()
    {   //Set on MissionStatement scene set up MissionStatementDialogue
        if(MissionStatementTriggerButton!= null) {
            MissionStatementTriggerButton.dialogueTextBox = MissionStatementDialogueTextBox;
            MissionStatementTriggerButton.dialogue = MissionStatementDialogue;
            MissionStatementTriggerButton.allowRestart = false;
        }

        //If on final scene setup final trigger with its dialogue
        if(finalDialogueTriggerButton!= null) {
            finalDialogueTriggerButton.dialogueTextBox = finalOutcomeDialogueTextBox;
            finalOutcomeDialogue = computeOutcomeDialogue();
            finalDialogueTriggerButton.dialogue = finalOutcomeDialogue;
            finalDialogueTriggerButton.allowRestart = false;
        }
      
        mainTabIndexInLayout = 1; 
        //Print balance and drone specs 
        remainingTimeText.text = "Remaining time: " +  remainingTime.ToString("F1") +" Weeks"; 
        availableBalanceText.text = "Balance: " + availableBalance.ToString() +"$"; 
        refreshDroneSpecs();
    }
    //TODO SET SPECS OF FINAL DRONE CHOICES ON THE FINAL DRONE OF THE RIGHT HAND SIDE!!!!!!!!!!!!!!!!!!
    //TODO THAT NEED TO DECIDE WHICH SPECS WILL KEEP SO CAN DO THAT PROBABLY NEXT WEEK.
    private Dialogue computeOutcomeDialogue(){
        Dialogue outcomeDialogue = new Dialogue();
        //has_wetsuit//colorList = new List<string>{"white", "purple", "blue"};
        //List<string> materialList = new List<string>{"carbon fiber", "mat2", "mat3"};
        //int[] droneSizeRange = {20, 150}; //min and max size range
        //double[] droneWeightRange = {0.5, 10};
        //One if for each var and set 1 of lets say 3 premade texts per var
        finalOutcomeDialogueSentences[0] =  "Your drone was donated to the the Scottish government go help monitor seabirdshelp\n\n" +
        "Press continue to receive feedback on your drone.";
        //IF worse case => program it here
        if(has_wetsuit) {   
            finalOutcomeDialogueSentences[1] = "The previous drone we used did not have a wet suit, so we are very satisfied" 
            + " to now be able to conduct our bird observation even in the rough ScottiSsh weather";
        } else {
            finalOutcomeDialogueSentences[1] = "The previous drone we used did not have a wet suit, so we are very satisfied" 
            + " to now be able to conduct our bird observation even in the rough Scottish weather";            
        }
        //Color: 
        if(colorList.Contains("blue")) {

        } else if(colorList.Contains("white")) {

        }

        if(droneSizeRange[0] <= 30) {
            finalOutcomeDialogueSentences[2] = "";
        }
        //Weight
        if(droneWeightRange[0] < 1.0) {
            finalOutcomeDialogueSentences[3] = "The drone is capable of flying and observing the birds however "+
        "its autonomy is only of 10-15 minutes, it remains a valuable tool but to be used sparingly\n";
        }else if(droneWeightRange[0] > 1.5) {
            finalOutcomeDialogueSentences[3] = "The drone is capable of flying and observing birds for over 40 minutes\n" +
            "I am impressed considering other drones usualy die out after about 30 minutes, also its very easy to drive and stable" + 
            "probably due to its weight being slightly above that of the average drone"; 
        }else {
            finalOutcomeDialogueSentences[3] = "This drone is capable of flying and observing birds for about 30 minutes, "+
            "it is a slight improvement from our previous drone and the stability of the drone is about the same."; 
        }

        //

        //Camera

        //Size

        //at final summary (overall) eval paragraph based on all vars
        outcomeDialogue.sentences = finalOutcomeDialogueSentences;
        return outcomeDialogue;
    }
    
    /**
    Manages what happens any time a user lock's in a choice.
    This is triggered any time a user lock's in a choice by clicking the "enter" button.
    */
    public void lockInChoice() {
        
        //TODO cant lock in choice until finished previous choice.
        acceptedSubChoiceNumber = 0; //reset subChoice index
        //Get choice locked in choice id and text
        DragDrop lastChoice = slot.droppedChoice;
        latestChoiceId = lastChoice.choice_id;
        string choiceCardText = lastChoice.GetComponentInChildren<TextMeshProUGUI>().text;
        
        //reset choice card to its original location
        lastChoice.transform.position = lastChoice.original_position;

        locked_choices.Add(latestChoiceId); // Add to List of locked choices TODO CAN REMOVE THIS PROBABLY LATER

        tabController.spawnTab(latestChoiceId, choiceCardText, choiceFeedbackDialogues[latestChoiceId]);
        //Main Tab always displayed the to the right of all other tabs
        mainTab.transform.SetSiblingIndex(++mainTabIndexInLayout);

        //Update game paramaters and UI
        updateMainTabText(choiceFeedbackDialogues[latestChoiceId]); //update text in main tab

        //update available time and balance due to locking in this main choice
        updateAvailableBalanceAndTimeForMainChoice(latestChoiceId);

        //Check if game ended, then activate final scene.
        if(latestChoiceId == (int)choices.shipIt){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+ 1);
        }
    }

    /**
    Called by DialogueManager when subChoice is accepted or refused
    */
    public void incrementSubChoiceNum() {
        acceptedSubChoiceNumber++;
    }
    /**
    This method is called whenever a subChoice is accepted
    Updates drone ranges according to accepted subChoice
    Called whenever choice is accepted
    */
    public void updateDroneRangesAndResources(){
        //Debug.Log("Calls updateDronRanges");
        Debug.Log(latestChoiceId);
        //TODO here check if availableBalance and RemaningTime is sufficient for this subchoice!
        if(availableBalance <= 0 || remainingTime <= 0) {
            return;
        }

        if(latestChoiceId == (int)choices.DroneExpert){
            if(acceptedSubChoiceNumber == 0){
                for(int i = 0; i < colorList.Count; i++) {
                    if(colorList[i] == "blue"){
                        colorList.RemoveAt(i);
                    }
                }
                updateAvailableBalanceAndTimeForSubChoices((float)0.25, 25);
            }
        }
        if(latestChoiceId == (int)choices.BirdExpert){
            //Bird expert ultimately suggest not to make it white
            if(acceptedSubChoiceNumber == 0) {
                for(int i = 0; i < colorList.Count; i++) {
                    if(colorList[i] == "white"){
                    colorList.RemoveAt(i);
                    }
                }
                updateAvailableBalanceAndTimeForSubChoices((float)0.25, 25);
            } else if(acceptedSubChoiceNumber == 1) {
                droneSizeRange[1] -= 30;
                updateAvailableBalanceAndTimeForSubChoices((float)0.25, 25);
            }
        }
        if(latestChoiceId == (int)choices.TestLocally){
            if(acceptedSubChoiceNumber == 0) {
                droneWeightRange[0] += 0.5;
                droneWeightRange[1] = 10;
                droneSizeRange[0] += 10;
                updateAvailableBalanceAndTimeForSubChoices((float)0.0, 0);
            }
        }
        if(latestChoiceId == (int)choices.OnFieldTesting){
            if(acceptedSubChoiceNumber == 0) {
                has_wetsuit = true;
                updateAvailableBalanceAndTimeForSubChoices((float)1.5, 100);
            } else if(acceptedSubChoiceNumber == 1) {
                droneWeightRange[0] += 0.5;
                droneSizeRange[0] += 10;
                updateAvailableBalanceAndTimeForSubChoices((float)0.0, 0);
            }
        }

        //display updates
        refreshDroneSpecs();
        
    }
    //Contains logic calculate the next text to display in scrollBar
    //Each choice locked in has a according display text
    private void updateMainTabText(Dialogue new_dialogue){
        EnterButton.dialogueTextBox = this.dialogueTextBox;
        EnterButton.dialogue = new_dialogue;
    }

    //updates the interface showing teh drone specs 
    private void refreshDroneSpecs(){
        StringBuilder sb = new StringBuilder("", 400);
        sb.AppendFormat("Potential Drone specs: \n");
        
        //COLOR RANGE
        sb.AppendFormat("Color: [");
        for(int i = 0; i < colorList.Count; i++) {
            sb.AppendFormat(colorList[i]);
            if(i < colorList.Count -1) {
                sb.AppendFormat(", ");
            }
        }
        sb.AppendFormat("]\n");

        //SIZE RANGE
        sb.AppendFormat("Size [cm]: [" + droneSizeRange[0].ToString() + " " + droneSizeRange[1].ToString() + "]\n");
        sb.AppendFormat("Weight [kg]: [" + string.Format("{0:F1}",droneWeightRange[0]) + " - " + 
                                        string.Format("{0:F1}",droneWeightRange[1]) + "]\n");
        sb.AppendFormat("Material: [");
        for(int i = 0; i < materialList.Count; i++) {
            sb.AppendFormat(materialList[i]);
            if(i < materialList.Count -1) {
                sb.AppendFormat(", ");
            }
        }
        sb.AppendFormat("]\n");
        if(has_wetsuit){
            sb.AppendFormat("Wet suit available\n"); //Surpirse this taxed you an extra week or 2.
        }
       
        droneSpecsText.text = sb.ToString();
    }

    //Update available time and balance for subchoices
    private void updateAvailableBalanceAndTimeForSubChoices(float timeCost, int financialCost) {
        remainingTime-= timeCost;
        availableBalance -= financialCost;
        remainingTimeText.text = "Remaining time: " + remainingTime.ToString("F1") +" Weeks"; 
        availableBalanceText.text = "Balance: " + availableBalance.ToString() +"$"; 
        
    }
    //Update available balance and time for main choices
    private void updateAvailableBalanceAndTimeForMainChoice(int locked_choice_id) {
        Debug.Log("availableBalance choice" + locked_choice_id.ToString());
        Debug.Log("cost of choice" + mainChoiceFinancialCosts[locked_choice_id].ToString());
        remainingTime-= mainChoiceTimeCosts[locked_choice_id];
        availableBalance -= mainChoiceFinancialCosts[locked_choice_id];
        remainingTimeText.text = "Remaining time: " + remainingTime.ToString("F1") +" Weeks"; 
        availableBalanceText.text = "Balance: " + availableBalance.ToString() +"$"; 
        
    }


}
