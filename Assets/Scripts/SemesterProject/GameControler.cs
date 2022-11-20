using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//TODO

/*
4)Work on Story and make interface much nicer. Thats it..
5)Writte report and document code
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
    public static int[] droneWeightRange = {500, 5000};
    //public int droneDevelopmentCost = 0;
    //public int droneManufacturingCost = 0;
    //public int droneProductionCost = 0;
    //public int dronePrice = 0;

    //Main Tab Feedback text
    public DialogueTrigger EnterButton;
    public TextMeshProUGUI dialogueTextBox;
    public TextMeshProUGUI scrollBarText; //Contains text currently displayed in scrollBar

    //Array of locked choice and choice selection objects
    List<int> locked_choices = new List<int>(); //List of choices locked in by the players
    public int choice_index = 0; //how many choices have been made
    public DropSlot slot; //slot where choice is dropped into

    /* Tabs and dialogues -------------------------------------------------------*/
    public Button mainTab;
    private int mainTabIndexInLayout;
    public TabController tabController;
    //Array of dialogues 
    [SerializeField] private List<Dialogue> choiceFeedbackDialogues;//set in unity directly
    
    //TODO modify main tab to use dialogues also!!!
    private string[] choiceFeedbackTexts = {"None", "Locked in 1", 
    "locked in 2", "Locked in 3", "Locked in 4", "Locked in 5", "Locked in 6"};

    /*Money System ---------------------------------------------------------------*/
    public TextMeshProUGUI availableBalanceText;
    static public int availableBalance = 10000; //set in unity and updated through code
    public int[] costs; //Costs of each choice, set in unity

    // -----------------------------------------------------------------------------
    void Start()
    {   //Set MissionStatementDialogue
        if(MissionStatementTriggerButton!= null) {
            MissionStatementTriggerButton.dialogueTextBox = MissionStatementDialogueTextBox;
            MissionStatementTriggerButton.dialogue = MissionStatementDialogue;
            MissionStatementTriggerButton.allowRestart = false;
        }

        //If on final scene setup final trigger with its dialogue
        if(finalDialogueTriggerButton!= null) {
            finalDialogueTriggerButton.dialogueTextBox = finalOutcomeDialogueTextBox;
            //TODO: Compute final dialogue in functions here!!!!!!!!!
            finalDialogueTriggerButton.dialogue = finalOutcomeDialogue;
            finalDialogueTriggerButton.allowRestart = false;
        }
      
        //Set button to trigger mission statement dialogue
        //Find a way to load mission statement dialogue open scene loading
        //Trigger intro dialogue, 
        mainTabIndexInLayout = 1;
        availableBalanceText.text = "Balance: " + availableBalance.ToString() +"$"; 
        refreshDroneSpecs();
    }
    
    public void lockInChoice() {
        //reset choice to its original location
        DragDrop lastChoice = slot.droppedChoice;
        int locked_choice_id = lastChoice.choice_id;
        string choiceCardText = lastChoice.GetComponentInChildren<TextMeshProUGUI>().text;
        
        //reset choice card to its original location
        lastChoice.transform.position = lastChoice.original_position;

        Debug.Log(locked_choice_id);

        locked_choices.Add(locked_choice_id); // Add to List of locked choices 
        tabController.spawnTab(locked_choice_id, choiceCardText, choiceFeedbackDialogues[locked_choice_id]);
        //Main Tab always displayed the to the right of all other tabs
        mainTab.transform.SetSiblingIndex(++mainTabIndexInLayout);

        //Update game paramaters and UI
        updateMainTabText(choiceFeedbackDialogues[locked_choice_id]); //update text in main tab
        updateDroneRanges(locked_choice_id);
        refreshDroneSpecs();
        updateAvailableBalance(locked_choice_id);

        //Check if game ended, then activate final scene.
        if(locked_choice_id == (int)choices.shipIt){
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+ 1);
        }
    }
    //Contains all game loics
    //current ranges: Color/Material, Weight/Size/ Camera
    private void updateDroneRanges(int choice_id){
        //TODO will need to code logic for different "orderings" aswell.... defo need to brainstorm this.
        if(choice_id == (int)choices.BirdExpert){
            //Bird expert ultimately suggest not to make it white
            for(int i = 0; i < colorList.Count; i++) {
                if(colorList[i] == "white"){
                    colorList.RemoveAt(i);
                }
            }
        }
        if(choice_id == (int)choices.DroneExpert){
             for(int i = 0; i < colorList.Count; i++) {
                if(colorList[i] == "blue"){
                    colorList.RemoveAt(i);
                }
            }
        }
        if(choice_id == (int)choices.OnFieldTesting){
            
        }
        if(choice_id == (int)choices.OnFieldTesting){
            droneSizeRange[0] = 30;
            droneSizeRange[1] = 60;
            has_wetsuit = true;
        }
        
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
        sb.AppendFormat("Weight [g]: [" + droneWeightRange[0].ToString() + " " + droneWeightRange[1].ToString() + "]\n");
        sb.AppendFormat("Material: ");
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

    private void updateAvailableBalance(int locked_choice_id) {
        Debug.Log("availableBalance choice" + locked_choice_id.ToString());
        Debug.Log("cost of choice" + costs[locked_choice_id].ToString());
        availableBalance -= costs[locked_choice_id];
        availableBalanceText.text = "Balance: " + availableBalance.ToString() +"$"; 
        //put a END button with no cost.
    }

}
