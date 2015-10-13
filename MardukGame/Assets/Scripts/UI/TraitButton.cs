using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using p = PlayerStats;

public class TraitButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler {
	
	public GameObject tooltip;
	public int traitIndex;

	public void OnPointerEnter(PointerEventData eventData){ //muestro el tooltip
		tooltip.SetActive (true);	
		showTooltip ();
	}
	
	public void OnPointerExit(PointerEventData eventData){		
		tooltip.SetActive (false);
	}
	
	private void showTooltip(){
		tooltip.transform.GetChild(0).GetComponent<Text> ().text = Traits.traits[traitIndex].getName() + "\n";
		tooltip.transform.GetChild(1).GetComponent<Text> ().text = Traits.traits[traitIndex].getDescription()  + "\n cost: " + Traits.traits[traitIndex].getCost()  + "\n";
	}
	
}

