using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;
using UnityEngine.UI;

public class TabController : MonoBehaviour
{
    [SerializeField] private List<GameObject> tabs;
    //For spawning of tab button
    public DialogueTrigger spawnButtonPrefab;
    public GameObject parentOfSpawn;
    private DialogueTrigger spawnedTabButton;

    //For spawning for tab body
    public GameObject parentOfBodySpawn; //to know where to place the tab in the hierarchy
    public GameObject spawnTabBodyPrefab;
    private GameObject spawnedTabBody; //new obj for spawned body
    //--
    //Now need to map button text to an index here...
    private string[] choiceFeedbackTexts = {"None", "Locked in 1", 
    "locked in 2", "Locked in 3", "Locked in 4", "Locked in 5", "Locked in 6"};
    private int choiceId = 0; //used to give spawnedTab a name

    //Called by the pressed button(with onClick() method), using as argument "tabBody" the body associated with the button

    public void onTabSwitch(GameObject tabBody) {
        tabBody.SetActive(true);  

        for(int i = 0; i < tabs.Count; i++) {
            if(tabs[i]!= tabBody){
                tabs[i].SetActive(false);
            }
        }
    }

    public void spawnTab(string locked_choice_text, Dialogue spawned_dialogue) {
        //Spawn a button for the tab
        spawnedTabButton = GameObject.Instantiate(spawnButtonPrefab);
        spawnedTabButton.GetComponentInChildren<TextMeshProUGUI>().text = locked_choice_text;
        spawnedTabButton.name = "spawnedTabButton_"+ choiceId.ToString();
        spawnedTabButton.transform.SetParent(parentOfSpawn.transform, false);
        spawnedTabButton.gameObject.SetActive(true);

        //Spawn new tab with 
        //Basically for custom content need to make a prefab, with a text field i can modify.. thats it. not soo hard.
        spawnedTabBody = GameObject.Instantiate(spawnTabBodyPrefab);
        spawnedTabBody.name = "spawnedTabBody_"+ choiceId.ToString();
        spawnedTabBody.transform.SetParent(parentOfBodySpawn.transform, false);


        //Set dialogue triggered by this button with tabContent
        /*Dialogue tabDialogue = new Dialogue();
        string[] spawned_sentences = {tabContent};
        Dialogue spawned_dialogue = new Dialogue();
        spawned_dialogue.sentences = spawned_sentences;*/
        spawnedTabButton.dialogue = spawned_dialogue;
        spawnedTabButton.dialogueTextBox = spawnedTabBody.GetComponentInChildren<TextMeshProUGUI>();

        //set spawnedButton to trigger dialogue upon click
        spawnedTabButton.onClick.AddListener(delegate { spawnedTabButton.TriggerDialogue(); });
        //Modify Button to call correct tab body
        spawnedTabButton.onClick.AddListener(delegate { onTabSwitch(spawnedTabBody); });

        //Add new spawned tab to tabs array
        tabs.Add(spawnedTabBody);
        
    }
}
