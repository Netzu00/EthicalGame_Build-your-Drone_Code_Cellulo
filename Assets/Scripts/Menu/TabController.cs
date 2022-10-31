using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Text;

public class TabController : MonoBehaviour
{
    [SerializeField] private GameObject[] tabs;
    public GameObject spawnButtonPrefab;
    public GameObject parentOfSpawn;
    private GameObject spawnedTabButton;
    private TextMeshProUGUI spawnedText;
    private int choiceId = 0;

    public void onTabSwitch(GameObject tab) {
        tab.SetActive(true);

        for(int i = 0; i < tabs.Length; i++) {
            if(tabs[i]!= tab){
                tabs[i].SetActive(false);
            }
        }
    }

    public void spawnTab(string buttonText, string tabContent) {
        //Spawn a button for the tab
        spawnedTabButton = GameObject.Instantiate(spawnButtonPrefab);
        spawnedTabButton.GetComponentInChildren<TextMeshProUGUI>().text = buttonText;
        spawnedTabButton.name = "Tab_"+ choiceId.ToString();
        spawnedTabButton.transform.SetParent(parentOfSpawn.transform, false);

        //Spawn new tab with contents
        
    }
}
