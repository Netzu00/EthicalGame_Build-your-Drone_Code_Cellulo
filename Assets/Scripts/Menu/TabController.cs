using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabController : MonoBehaviour
{
    [SerializeField] private GameObject[] tabs;
    public GameObject spawnPrefab;
    public GameObject parentOfSpawn;
    private GameObject spawnedtab;
    public void Start()
    {
        spawnedtab = GameObject.Instantiate(spawnPrefab);
        Debug.Log(spawnedtab);
        spawnedtab.transform.SetParent(parentOfSpawn.transform, false);
        //spawnedtab.SetActive(true); NICE
    }

    public void onTabSwitch(GameObject tab) {
        tab.SetActive(true);

        for(int i = 0; i < tabs.Length; i++) {
            if(tabs[i]!= tab){
                tabs[i].SetActive(false);
            }
        }
    }
}
