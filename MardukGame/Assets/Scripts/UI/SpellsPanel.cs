﻿using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using p = PlayerStats;
using pc = PlatformerCharacter2D;
using System.Collections.Generic;

public class SpellsPanel : MonoBehaviour, IHasChanged {

	public static GameObject[] projectiles = new GameObject[4];
	public InventorySlotsPanel invPanel;
	public Transform slots;
	public Transform supportSlots;

	private GameObject player;
	public SpellStats[] playerSkills;
	public SpellStats[] playerSupportSkills;
	// Use this for initialization
	void Start () {
		player =  GameObject.Find ("Player");
		if (player != null) {
			playerSkills  = PlatformerCharacter2D.playerSkills;
			playerSupportSkills = pc.playerSupportSkills;
			HasChanged ();
		}
	}

	void Update(){
		if (player == null ){
			player = GameObject.FindGameObjectWithTag("Player");
			if (player != null){
				playerSkills  = PlatformerCharacter2D.playerSkills; 
				playerSupportSkills = pc.playerSupportSkills;
				HasChanged ();
			}
		}
	}

	public void HasChanged(){ //se llama cuando hubo algun cambio en el spell panel, ej: se intridujo un nuevo skill o se cambio de lugar uno
		if(playerSkills == null || pc.playerSupportSkills == null)
			return;
		int i = 0;
		foreach (Transform slot in slots) {
			GameObject spell = slot.GetComponent<Slot>().spell;
			if(spell != null){
				SpellStats stats =  spell.GetComponent<SpellStats>();
				stats.EquipSkill();
				pc.playerSkills[i] = stats;
			}
			else{
				pc.playerSkills[i] = null;
			}
			i++;
		}
		int j = 0;
		foreach (Transform slot in supportSlots) { // recorre los supoprt slots
			GameObject spell = slot.GetComponent<Slot>().spell;
			if(spell != null){
				SpellStats stats =  spell.GetComponent<SpellStats>();
				stats.EquipSkill();
				pc.playerSupportSkills[j] = stats;
			}
			else{
				pc.playerSupportSkills[j] = null;
			}
			j++;
		}
		foreach(SpellStats sp in PlayerItems.SpellsInvetory){
			if(sp.IdSlotEquipped < 0){
				sp.UnequipSkill();
			}
		}
	}

	public void LoadSkills(List<SerializableSpell> skillList){
		List<SpellStats> sInvAux = new List<SpellStats>();
		foreach(SerializableSpell skill in skillList){
			GameObject newSpell = InstantiateSkill(skill.spellName);
			SpellStats st = newSpell.GetComponent<SpellStats>();
			if(st != null){
				st.OldNextLevelExp = skill.oldNextLevelExp;
				st.Lvl = skill.lvl;	
				st.CurrentExp = skill.currentExp;
				st.NextLevelExp = st.SpellExpFormula();
				st.InventoryPositionX = skill.inventoryPositionX;
				st.InventoryPositionY = skill.inventoryPositionY;
				st.IdSlotEquipped = skill.idSlotEquipped;
				sInvAux.Add(st);
				if(st.IdSlotEquipped > -1){ // si es mayor a -1 esta equipado en esa posicion
					st.EquipSkill();
					if(st.type != Types.SkillsTypes.Support){
						newSpell.transform.SetParent(slots.GetChild(st.IdSlotEquipped));
						newSpell.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
					}
					else{
						newSpell.transform.SetParent(supportSlots.GetChild(st.IdSlotEquipped));
						newSpell.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
					}	
				}
				else
					invPanel.LoadSkillAt(newSpell,st.InventoryPositionX,st.InventoryPositionY);
			}
		}
		PlayerItems.SpellsInvetory = sInvAux;
		HasChanged();
	}
	
	/*Agrega un nuevo skill despues de que se agarra, se invoca desde PlatformerCharacter2D*/
	public bool AddSpell(string spellName){ 
		bool alreadyEquipped = false;
		for(int i = 0; i< PlatformerCharacter2D.playerSkills.Length; i++){
			if(PlatformerCharacter2D.playerSkills[i] != null && string.Compare(PlatformerCharacter2D.playerSkills[i].nameForSave,spellName)==0)
				alreadyEquipped = true;
		}
		GameObject newSpell = InstantiateSkill(spellName);
		SpellStats st = newSpell.GetComponent<SpellStats>();
		if(st.type == Types.SkillsTypes.Support) //si es un support lo agrega al inventario directamente
			alreadyEquipped = true;
		st.OldNextLevelExp = 1;
		st.Lvl = 0;	
		st.CurrentExp = 0;
		st.NextLevelExp = st.SpellExpFormula();
		if(!alreadyEquipped){
			foreach(Transform slot in slots){ //busca un slot vacio en spell panel
				GameObject spell = slot.GetComponent<Slot>().spell;
				if(spell == null){
					newSpell.transform.SetParent(slot);
					newSpell.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
					st.IdSlotEquipped = slot.GetComponent<Slot>().id;
					PlayerItems.SpellsInvetory.Add(st);
					st.EquipSkill();		
					HasChanged();
					return true;
				}
			}
		}
		/*Si no encuentra un slot disponible, lo intenta meter en el inventario*/
		bool invPanelResult = false;
		invPanelResult =  invPanel.AddSkill(newSpell);
		return invPanelResult;

	}
	
	private GameObject InstantiateSkill(string skillName){
		GameObject newSpellPrefab = (GameObject)Resources.Load ("PlayerSpells/" + skillName); //creo el prefab para meterlo en el slot
		if (newSpellPrefab == null) {
			Debug.LogError("prefab del skill no encontrado");
			return null;
		}
		return (GameObject)Instantiate (newSpellPrefab, newSpellPrefab.transform.position, newSpellPrefab.transform.rotation);
	}

}

namespace UnityEngine.EventSystems{
	public interface IHasChanged : IEventSystemHandler{
		void HasChanged();
	}
}