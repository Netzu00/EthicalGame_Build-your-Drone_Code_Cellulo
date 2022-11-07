using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

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
public enum Choices
{
	None, 
	DroneExpert,
    BirdExpert, //expert on specific bird
    OnSiteVisit, //nothern ireland lots of rain and WIND! in winter for example
    OnFieldTesting, 
    macroTesting,
}
public class GameControler : MonoBehaviour
{
    // Start is called before the first frame update
    public int max_num_choices = 5; //TODO MAX WILL BE DETERMINED BY BUDGET
    public int budget = 10000; 
    private const int NUM_CARDS = 5;
    public TextMeshProUGUI droneSpecsText;
    public string droneColor = "white";
    public string droneSize = "30cm";
    public string droneWeight = "1kg";
    public int droneDevelopmentCost = 0;
    public int droneManufacturingCost = 0;
    public int droneProductionCost = 0;
    public int dronePrice = 0;
    public string droneMaterial = "carbon fiber composites";
    public TextMeshProUGUI scrollBarText; //Contains text currently displayed in scrollBar
    List<int> locked_choices = new List<int>(); //List of choices locked in by the players
    public int choice_index = 0; //how many choices have been made
    public DropSlot slot; //slot where choice is dropped into

    /* Tabs and dialogues -------------------------------------------------------*/
    public TabController tabController;
    //Array of dialogues 
    [SerializeField] private List<Dialogue> choiceFeedbackDialogues;//set in unity directly
    
    //TODO modify main tab to use dialogues also!!!
    private string[] choiceFeedbackTexts = {"None", "Locked in 1", 
    "locked in 2", "Locked in 3", "Locked in 4", "Locked in 5", "Locked in 6"};

    /*Money System ---------------------------------------------------------------*/
    public TextMeshProUGUI availableBalanceText;
    public int availableBalance; //set in unity and updated through code
    public int[] costs; //Costs of each choice, set in unity

    // -----------------------------------------------------------------------------
    void Start()
    {   
        availableBalanceText.text = "Balance: " + availableBalance.ToString() +"$"; 
        refreshDroneSpecs();
    }
    
    public void lockInChoice() {
        //reset choice to its original location
         

        DragDrop lastChoice = slot.droppedChoice;
        int locked_choice_id = lastChoice.choice_id;
        string choiceText = lastChoice.GetComponentInChildren<TextMeshProUGUI>().text;
        
        //reset choice card to its original location
        lastChoice.transform.position = lastChoice.original_position;

        locked_choices.Add(locked_choice_id); // Add to List of locked choices 
        tabController.spawnTab(choiceText, choiceFeedbackDialogues[locked_choice_id]);
        //debug_print_list_content();
        
        //Update game paramaters and UI
        updateScrollBarText(locked_choice_id); //update text in main tab
        refreshDroneSpecs();
        updateAvailableBalance(locked_choice_id);

        //Check if game ended and calculate final outcome
        //IF choice is final choice then launch logic
        //to calculate final outcome.
    }

    private void updateAvailableBalance(int locked_choice_id) {
        Debug.Log("avaialableBalance choice" + locked_choice_id.ToString());
        Debug.Log("cost of choice" + costs[locked_choice_id].ToString());
        availableBalance -= costs[locked_choice_id];
        availableBalanceText.text = "Balance: " + availableBalance.ToString() +"$"; 
        //put a END button with no cost.
    }

    private void debug_print_list_content() {
        //Print list contents for debugging
        string result = "List contents: ";
        foreach (var item in locked_choices)
        {
            result += item.ToString() + ", ";
        }
        Debug.Log(result);
    }

    //Contains logic calculate the next text to display in scrollBar
    //Each choice locked in has a according display text
    private void updateScrollBarText(int locked_choice){
        Debug.Log(locked_choice);
        scrollBarText.text = choiceFeedbackTexts[locked_choice];
    }

    //updates the interface showing teh drone specs 
    private void refreshDroneSpecs(){
        StringBuilder sb = new StringBuilder("", 200);
        sb.AppendFormat("Color: {0}\nWeight :{1}\nSize: {2}\nMaterial: {3}\nPrice: {4}\n" + 
            "Development Cost: {5}\nManufacturing Cost: {6}\nProduction Cost: {7}", 
            droneColor, droneWeight, droneSize, droneMaterial, dronePrice.ToString(),
            droneDevelopmentCost.ToString(), droneManufacturingCost.ToString(), droneProductionCost.ToString());

        droneSpecsText.text = sb.ToString();
    }
    //TODO private void endingOfGameLogic(){}
}
