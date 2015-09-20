using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using p = PlayerStats;

public class Slot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

	public GameObject tooltip;

	public GameObject spell{
		get{
			if(transform.childCount > 0)
				return transform.GetChild(0).gameObject;
			return null;
		}
		set{
			this.spell = value;
		}
	}

	public void OnDrop(PointerEventData eventData){
		if (!spell)
			DragHandeler.itemBeingDragged.transform.SetParent (transform);
		else {
			spell.transform.SetParent(DragHandeler.startParent);
			DragHandeler.itemBeingDragged.transform.SetParent (transform);
		}
		ExecuteEvents.ExecuteHierarchy<IHasChanged> (gameObject, null, (x,y) => x.HasChanged ());
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right) {
			if(spell != null)
				spell.GetComponent<SpellStats>().RemoveSkill();
			ExecuteEvents.ExecuteHierarchy<IHasChanged> (gameObject, null, (x,y) => x.HasChanged ());
		}
		/*else if (eventData.button == PointerEventData.InputButton.Middle)
			Debug.Log("Middle click");
		else if (eventData.button == PointerEventData.InputButton.Left)
			Debug.Log("Right click");*/
	}
		
	public void OnPointerEnter(PointerEventData eventData){ //muestro el tooltip
		showTooltip ();
	}

	public void OnPointerExit(PointerEventData eventData){

		tooltip.SetActive (false);
	}

	private void showTooltip(){
		if (this.spell == null)
			return;
		SpellStats spellStats = spell.GetComponent<SpellStats> ();
		tooltip.transform.GetChild (0).GetComponent<Text> ().text = spellStats.spellName;
		switch(spellStats.type){
		case Types.SkillsTypes.Spell:
			PlayerProjStats pps = spell.GetComponent<SpellStats> ().projectile.GetComponent<PlayerProjStats> ();
			switch (pps.elem) {
			case Types.Element.Cold:
				tooltip.transform.GetChild (2).GetComponent<Text> ().color = Color.cyan;	
				break;
			case Types.Element.Fire:
				tooltip.transform.GetChild (2).GetComponent<Text> ().color = Color.red;	
				break;
			case Types.Element.Lightning:
				tooltip.transform.GetChild (2).GetComponent<Text> ().color = Color.yellow;	
				break;
			case Types.Element.Poison:
				tooltip.transform.GetChild (2).GetComponent<Text> ().color = Color.green;	
				break;
			default:
				tooltip.transform.GetChild (2).GetComponent<Text> ().color = Color.white;	
				break;
			}
			tooltip.transform.GetChild (1).GetComponent<Text> ().text = spellStats.type.ToString();
			tooltip.transform.GetChild (2).GetComponent<Text> ().text = pps.elem.ToString ();
			tooltip.transform.GetChild (3).GetComponent<Text> ().text = "Mana cost: " + spellStats.manaCost.ToString () + "\n";
			float totalMinDmg = pps.minDmg + pps.minDmg * p.offensives[p.IncreasedMgDmg]/100;
			float totalMaxDmg = pps.maxDmg + pps.maxDmg * p.offensives[p.IncreasedMgDmg]/100;
			tooltip.transform.GetChild (3).GetComponent<Text> ().text += "Damage " + System.Math.Round (totalMinDmg, 1).ToString () + " - " + System.Math.Round (totalMaxDmg, 1).ToString ();
			break;
		case Types.SkillsTypes.Aura:
			//tooltip.transform.GetChild (1).GetComponent<Text> ().color = Color.magenta;	
			tooltip.transform.GetChild (1).GetComponent<Text> ().text = "Aura";
			tooltip.transform.GetChild (2).GetComponent<Text> ().text = "";
			tooltip.transform.GetChild (3).GetComponent<Text> ().text = "Mana Reserved: " + spellStats.manaReserved.ToString()+ "\n";
			tooltip.transform.GetChild (3).GetComponent<Text> ().text += "Life Regen per Second: " + spellStats.lifeRegenPerSecond;
			break;
		case Types.SkillsTypes.Movement:
			tooltip.transform.GetChild (1).GetComponent<Text> ().text = "Movement";
			tooltip.transform.GetChild (2).GetComponent<Text> ().text = "";
			tooltip.transform.GetChild (3).GetComponent<Text> ().text = "Mana Reserved: " + spellStats.manaCost.ToString()+ "\n";
			
			break;
		}
		tooltip.SetActive (true);	
	}

}
