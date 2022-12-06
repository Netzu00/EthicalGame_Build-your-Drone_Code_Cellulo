using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler {
   public DragDrop droppedChoice;
   public void OnDrop(PointerEventData eventData){
        //Debug.Log("OnDrop");
        
        if(eventData.pointerDrag != null) {
            eventData.pointerDrag.GetComponent<RectTransform>().position = GetComponent<RectTransform>().position;
            droppedChoice = eventData.pointerDrag.GetComponent<DragDrop>();
        }  
   }
}
