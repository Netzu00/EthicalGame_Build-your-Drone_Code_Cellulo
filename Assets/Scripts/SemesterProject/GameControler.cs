using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

//TODO
/*
2) work on the drone info interface, probably will work
same was the scrollbar but with image and smaller changes
(more modular)

3)Then pick together a selection method, if use cellulo or not ect..
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

    private string[] choiceFeedbackTexts = {"None", "Locked in 1", 
    "locked in 2", "Locked in 3", "Locked in 4", "Locked in 5", "Locked in 6"};

    void Start()
    {
        refreshDroneSpecs();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void lockInChoice() {
        Debug.Log("lockInChoice");
        int locked_choice = slot.current_choice;
        locked_choices.Add(locked_choice);
        //Check if valid choice i.e first must be investor
        //Last must be "go foward with production/finalize"


        string result = "List contents: ";
        foreach (var item in locked_choices)
        {
            result += item.ToString() + ", ";
        }
        Debug.Log(result);
    
        updateScrollBarText(locked_choice);
        refreshDroneSpecs();
        //TODO launch text belonging to that choice
        //Debit cost of choice
        //IF choice is final choice then launch logic
        //to calculate final outcome.
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
