using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropSlot : MonoBehaviour, IDropHandler
{
   public int current_choice = -1;
   public void OnDrop(PointerEventData eventData){
        Debug.Log("OnDrop");
        if(eventData.pointerDrag != null) {
            eventData.pointerDrag.GetComponent<RectTransform>().anchoredPosition = GetComponent<RectTransform>().anchoredPosition;
        }  
        
        DragDrop droppedObject = eventData.pointerDrag.GetComponent<DragDrop>();
        current_choice = droppedObject.choice_id;
   }
}
