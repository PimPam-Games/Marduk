using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AttrInfo : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler{

	public GameObject characterTooltip;
	public int id  = -1;

	public void OnPointerEnter(PointerEventData eventData){ //muestro el tooltip
		if(!characterTooltip.activeSelf){
			characterTooltip.SetActive(true);
			if(id == 1)
				characterTooltip.GetComponentInChildren<Text>().text = "1 Strength , 0.25 of base physical damage";
			if(id == 3)
				characterTooltip.GetComponentInChildren<Text>().text = "1 Vitality , 3 HP";
		}
	}
	
	public void OnPointerExit(PointerEventData eventData){
		if(characterTooltip.activeSelf){
			characterTooltip.GetComponentInChildren<Text>().text = "";
			characterTooltip.SetActive(false);		
		}
	}
}
