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
		tooltip.transform.GetChild (1).GetComponent<Text> ().text = spellStats.type.ToString();
		Text reqText = tooltip.transform.GetChild (2).GetComponent<Text> ();
		reqText.text = "";
		for (int i = 0; i<spellStats.requeriments.Length; i++) {
			reqText.text += spellStats.requeriments[i].ToString() + " ";
			if(spellStats.requeriments[i] == Types.SkillsRequirements.None)
				reqText.text = "No requirements";
		}
		switch(spellStats.type){
		case Types.SkillsTypes.Ranged:
			RangedSkill rskill = (RangedSkill)spell.GetComponent<SpellStats> ();
			PlayerProjStats pps = rskill.projectile.GetComponent<PlayerProjStats> ();

			tooltip.transform.GetChild (3).GetComponent<Text> ().text = "Mana cost: " + spellStats.manaCost.ToString () + "\n";
			float totalMinDmg = pps.minDmg + pps.minDmg * p.offensives[p.IncreasedMgDmg]/100;
			float totalMaxDmg = pps.maxDmg + pps.maxDmg * p.offensives[p.IncreasedMgDmg]/100;
			tooltip.transform.GetChild (3).GetComponent<Text> ().text += "Damage " + System.Math.Round (totalMinDmg, 1).ToString () + " - " + System.Math.Round (totalMaxDmg, 1).ToString () + "\n";
			tooltip.transform.GetChild (3).GetComponent<Text> ().text += "level: " + spellStats.Lvl.ToString() +  "\n";
			break;
		case Types.SkillsTypes.Aura:

			tooltip.transform.GetChild (3).GetComponent<Text> ().text = "Mana Reserved: " + spellStats.manaReserved.ToString()+ "\n";
			//tooltip.transform.GetChild (3).GetComponent<Text> ().text += spellStats.lifeRegenPerSecond + "% Life Regen per Second" ;
			break;
		case Types.SkillsTypes.Utility:

			tooltip.transform.GetChild (3).GetComponent<Text> ().text = "Mana Reserved: " + spellStats.manaCost.ToString()+ "\n";
			
			break;
		case Types.SkillsTypes.Melee:
			RangedSkill rsskill = (RangedSkill)spell.GetComponent<SpellStats> ();
			PlayerProjStats proj = rsskill.projectile.GetComponent<PlayerProjStats> ();

			tooltip.transform.GetChild (3).GetComponent<Text> ().text = "Mana cost: " + spellStats.manaCost.ToString () + "\n"
				+ "damage multiplier: " + proj.physicalDmgMult +"% \n"
					+ "level: " + spellStats.Lvl.ToString() +  "\n";
			
			break;
		}
		tooltip.SetActive (true);	
	}

}
