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
				characterTooltip.GetComponentInChildren<Text>().text = "1 Strength \n ---------------------------------- \n +0.25 of base physical damage";
			if(id == 3)
				characterTooltip.GetComponentInChildren<Text>().text = "1 Vitality \n ---------------------------------- \n +3 Maximum HP";
			if(id == 2)
				characterTooltip.GetComponentInChildren<Text>().text = "1 Dexterity \n ---------------------------------- \n +0.1 Crit Dmg Multiplier";
			if(id == 4)
				characterTooltip.GetComponentInChildren<Text>().text = "1 Spirit \n ---------------------------------- \n +3 Maximum Mana \n +0.1 Mana Regeneration per second \n +0.2 Magic Damage";
		}
	}
	
	public void OnPointerExit(PointerEventData eventData){
		if(characterTooltip.activeSelf){
			characterTooltip.GetComponentInChildren<Text>().text = "";
			characterTooltip.SetActive(false);		
		}
	}
}
