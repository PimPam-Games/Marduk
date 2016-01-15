using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySlot :  MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler{

	private const int WeaponSlotID = 1, ArmourSlotID = 2, ShieldSlotID = 3, HeadSlotID = 4, BeltSlotID = 5, RingLSlotID = 6, RingRSlotID = 7, AmuletSlotID = 8;

	public int slotId = 0;
	private float doubleClickTimer = 0;
	private float doubleClickDelay = 0.5f;
	private int clickCount = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	/*void Update () {
		doubleClickTimer -= Time.deltaTime;
		if(doubleClickTimer <= 0)
			clickCount = 0;
	}*/

	public GameObject item{
		get{
			if(transform.childCount > 0)
				return transform.GetChild(0).gameObject;
			return null;
		}
		set{
			this.item = value;
		}
	}


	public void OnDrop (PointerEventData eventData)
	{
		Item draggedItem = DragHandeler.itemBeingDragged.GetComponent<Item>();
		if(draggedItem != null){
			switch(slotId){
				case WeaponSlotID:
				if(draggedItem.type != ItemTypes.Weapon && draggedItem.type != ItemTypes.TwoHandedWeapon && draggedItem.type != ItemTypes.RangedWeapon)
						return;
				if((draggedItem.type == ItemTypes.RangedWeapon || draggedItem.type == ItemTypes.TwoHandedWeapon) && PlayerItems.EquipedShield != null) //no deja equipar al arco si hay un escudo equipado
						return;
					draggedItem.IsEquipped = true;
					draggedItem.InventoryPositionX = -1; //-1 significa que no esta en el inventario
					PlayerItems.EquipedWeapon = draggedItem;
					break;
				case ArmourSlotID:
					if(draggedItem.type != ItemTypes.Armour)
						return;
					draggedItem.IsEquipped = true;
					draggedItem.InventoryPositionX = -1; //-1 significa que no esta en el inventario
					PlayerItems.EquipedArmour = draggedItem;
					break;
				case ShieldSlotID:
					if(draggedItem.type != ItemTypes.Shield)
						return;
				if(PlayerItems.EquipedWeapon != null && (PlayerItems.EquipedWeapon.type == ItemTypes.RangedWeapon || PlayerItems.EquipedWeapon.type == ItemTypes.TwoHandedWeapon)) //no deja equipar al escudo si hay un arco equipado
						return;
					draggedItem.IsEquipped = true;
					draggedItem.InventoryPositionX = -1; //-1 significa que no esta en el inventario
					PlayerItems.EquipedShield = draggedItem;
					break;
				case HeadSlotID:
					if(draggedItem.type != ItemTypes.Helmet)
						return;
					draggedItem.IsEquipped = true;
					draggedItem.InventoryPositionX = -1; //-1 significa que no esta en el inventario
					PlayerItems.EquipedHelmet = draggedItem;
					break;
				case BeltSlotID:
					if(draggedItem.type != ItemTypes.Belt)
						return;
					draggedItem.IsEquipped = true;
					draggedItem.InventoryPositionX = -1; //-1 significa que no esta en el inventario
					PlayerItems.EquipedBelt = draggedItem;
					break;
				case RingLSlotID:
					if(draggedItem.type != ItemTypes.Ring)
						return;
					draggedItem.IsEquipped = true;
					draggedItem.InventoryPositionX = -1; //-1 significa que no esta en el inventario
					PlayerItems.EquipedRingL = draggedItem;
					break;
				case RingRSlotID:
					if(draggedItem.type != ItemTypes.Ring)
						return;
					draggedItem.IsEquipped = true;
					draggedItem.InventoryPositionX = -1; //-1 significa que no esta en el inventario
					PlayerItems.EquipedRingR= draggedItem;
					break;
				case AmuletSlotID:
					if(draggedItem.type != ItemTypes.Amulet)
						return;
					draggedItem.IsEquipped = true;
					draggedItem.InventoryPositionX = -1; //-1 significa que no esta en el inventario
					PlayerItems.EquipedAmulet= draggedItem;
					break;
			}
		}
		if (!item)
			DragHandeler.itemBeingDragged.transform.SetParent (transform);
		else {
			InventorySlot invSlot = DragHandeler.startParent.gameObject.GetComponent<InventorySlot>();
			
			if(invSlot != null && invSlot.slotId < 1){
				item.transform.SetParent(DragHandeler.startParent);
				DragHandeler.itemBeingDragged.transform.SetParent (transform);
			}
		}
		ExecuteEvents.ExecuteHierarchy<IHasChanged> (gameObject, null, (x,y) => x.HasChanged ());
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		
		if(item!=null){
			
			InventorySlotsPanel.invTooltip.SetActive(true);
			Item it = item.GetComponent<Item>();
			if(it != null)
				InventorySlotsPanel.invTooltip.GetComponentInChildren<Text>().text = it.ToolTip();
			else{
				SpellStats skill = item.GetComponent<SpellStats>();
				InventorySlotsPanel.invTooltip.GetComponentInChildren<Text>().text = skill.ToolTip();
			}
		}
	}


	public void OnPointerExit (PointerEventData eventData)
	{
		if(item!=null){
			InventorySlotsPanel.invTooltip.SetActive(false);
			//InventorySlotsPanel.invTooltip.GetComponentInChildren<Text>().text = item.GetComponent<Item>().ToolTip();
		}
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		switch(eventData.button)
		{
		case PointerEventData.InputButton.Left:
			clickCount++;
			doubleClickTimer = doubleClickDelay;
			if(clickCount == 2){
				clickCount = 0;
				if(item == null)
					return;				
			}
			break;
		case PointerEventData.InputButton.Right:
			if(item!=null){
				Item it = item.GetComponent<Item>();
				if(it != null ){
					if(it.IsEquipped){
						return;					
					}
					PlayerItems.Inventory.Remove(it);
				}
				else{
					SpellStats sp = item.GetComponent<SpellStats>();
					PlayerItems.SpellsInvetory.Remove(sp);
				}
				Destroy(item.gameObject);
				ExecuteEvents.ExecuteHierarchy<IHasChanged> (gameObject, null, (x,y) => x.HasChanged ());
			}
			break;
		}
		
	}

}
