using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using p = PlayerStats;
public class AttrInfo : MonoBehaviour,  IPointerEnterHandler, IPointerExitHandler{

	public GameObject characterTooltip;
	public int id  = -1;

	public void OnPointerEnter(PointerEventData eventData){ //muestro el tooltip
		if(!characterTooltip.activeSelf){
			characterTooltip.SetActive(true);
			if(id == 1)
				characterTooltip.GetComponentInChildren<Text>().text = "1 Strength \n ---------------------------------- \n +" +p.DmgPerStrengthP +" of base physical damage";
			if(id == 3)
				characterTooltip.GetComponentInChildren<Text>().text = "1 Vitality \n ---------------------------------- \n +"+p.HealthPerVitalityP +" Maximum HP";
			if(id == 2)
				characterTooltip.GetComponentInChildren<Text>().text = "1 Dexterity \n ---------------------------------- \n +" + p.CritMultPerDexterityP + " Crit Dmg Multiplier \n +" + System.Math.Round(p.CritChancePerDexterityP * 100,1) + "% of Critical Chance";
			if(id == 4)
				characterTooltip.GetComponentInChildren<Text>().text = "1 Spirit \n ---------------------------------- \n +"+ p.MaxManaPerSpiritP +" Maximum Mana \n +" + p.ManaRegenPerSpiritP +" Mana Regeneration per second \n +"+p.MgDmgPerSpiritP+" Magic Damage";
		}
	}
	
	public void OnPointerExit(PointerEventData eventData){
		if(characterTooltip.activeSelf){
			characterTooltip.GetComponentInChildren<Text>().text = "";
			characterTooltip.SetActive(false);		
		}
	}
}
