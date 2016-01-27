using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using p = PlayerStats;

public class Slot : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler {

	public GameObject tooltip;
	public int id;
	public bool supportSlot;
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

	void Awake(){
		if(transform.childCount > 0)
			Destroy(transform.GetChild(0).gameObject);
	}

	public void OnDrop(PointerEventData eventData){	
		SpellStats draggedSpell = DragHandeler.itemBeingDragged.GetComponent<SpellStats>();
		if(draggedSpell == null)
			return;
		if(!spell && draggedSpell.type == Types.SkillsTypes.Support) //si es un support y no hay skill en este slot no hace nada
			return;
		if(!supportSlot){ //si es suppport se pueden poner 2 iguales
			for(int i = 0; i< PlatformerCharacter2D.playerSkills.Length; i++){ // si hay otro skill igual equipado
				if(PlatformerCharacter2D.playerSkills[i] != null && PlatformerCharacter2D.playerSkills[i].IdSlotEquipped != draggedSpell.IdSlotEquipped  && string.Compare(PlatformerCharacter2D.playerSkills[i].nameForSave,draggedSpell.nameForSave)==0)
					return;
			}
		}
		draggedSpell.IdSlotEquipped = this.id;
		if (!spell)
			DragHandeler.itemBeingDragged.transform.SetParent (transform);
		else {
			SpellStats sp = spell.GetComponent<SpellStats>();
			if(draggedSpell.type == Types.SkillsTypes.Support){ //si solte un support en un slot donde hay otro skill
				if(spell.GetComponent<SpellStats>().SupportSkill != null)
					return;
				sp.SupportSkill = (Support)draggedSpell; //le asigno el support a este skill
				DragHandeler.itemBeingDragged.transform.SetParent(null); // no lo puedo destruir por que se puierde la referencia al support del skill
				DragHandeler.itemBeingDragged.gameObject.SetActive(false);
			}	
			else{
				if(DragHandeler.startParent.GetComponent<Slot>() != null)
					sp.IdSlotEquipped = DragHandeler.startParent.GetComponent<Slot>().id;
				else{
					sp.IdSlotEquipped = -1;
					sp.InventoryPositionX = draggedSpell.InventoryPositionX;
					sp.InventoryPositionY = draggedSpell.InventoryPositionY;					
				}
				spell.transform.SetParent(DragHandeler.startParent);
				DragHandeler.itemBeingDragged.transform.SetParent (transform);
			}
		}
		ExecuteEvents.ExecuteHierarchy<IHasChanged> (gameObject, null, (x,y) => x.HasChanged ());
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (eventData.button == PointerEventData.InputButton.Right) {
			/*if(spell != null){
				SpellStats sp = spell.GetComponent<SpellStats>();
				PlayerItems.SpellsInvetory.Remove(sp);
				sp.RemoveSkill();
				
			}
			ExecuteEvents.ExecuteHierarchy<IHasChanged> (gameObject, null, (x,y) => x.HasChanged ());*/
		}
		/*else if (eventData.button == PointerEventData.InputButton.Middle)
			Debug.Log("Middle click");
		else if (eventData.button == PointerEventData.InputButton.Left)
			Debug.Log("Right click");*/
	}
		
	public void OnPointerEnter(PointerEventData eventData){ //muestro el tooltip
		if(spell!=null && InputControllerGui.invOpen){			
			InventorySlotsPanel.invTooltip.SetActive(true);
			SpellStats skill = spell.GetComponent<SpellStats>();
			InventorySlotsPanel.invTooltip.GetComponentInChildren<Text>().text = skill.ToolTip();
		}
	}

	public void OnPointerExit(PointerEventData eventData){
		if(InputControllerGui.invOpen){
			InventorySlotsPanel.invTooltip.SetActive(false);
		}
	}

}
