using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameControler : MonoBehaviour
{
    // Start is called before the first frame update
    public int max_num_choices = 5; //TODO MAX WILL BE DETERMINED BY BUDGET
    public int budget = 10000;
    public int locked_choice = -1;

    List<int> locked_choices = new List<int>();
    public int choice_index = 0;
    public DropSlot slot;


    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void lockInChoice() {
        Debug.Log("lockInChoice");
        int locked_choice = slot.current_choice;
        Debug.Log(locked_choice);
        locked_choices.Add(locked_choice);
        //TODO launch text belonging to that choice
        //IF choice is final choice then launch logic
        //to calculate final outcome.
    }
}
