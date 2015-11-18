using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using pitems = PlayerItems;
using UnityEngine.UI;
using System.Collections.Generic;

public class InventorySlotsPanel : MonoBehaviour, IHasChanged {

	public Transform[] slotsPanels;
	public Transform weaponSlot = null;
	public Transform armourSlot = null;
	public Transform shieldSlot = null;
	public Transform headSlot = null;
	public Transform beltSlot = null;
	public Transform ringLSlot = null;
	public Transform ringRSlot = null;
	public Transform amuletSlot = null;
	public GameObject itemUI;
	public GameObject invTooltipGO;
	public static GameObject invTooltip;
	public SpellsPanel spellsPanel;
	//private bool itemsLoaded = false;
	
	
	//public GameObject itemUI;
	// Use this for initialization
	void Start () {
		invTooltip = invTooltipGO;
	}
	
	// Update is called once per frame
	void Update () {
		/*if(GameController.levelLoaded && !itemsLoaded){
			itemsLoaded = true;
			LoadItems();	
		}*/
	}

	/*Se invoca desde SpellsPanel */
	public void LoadSkillAt(GameObject newSkill, int posX, int posY){
		if(slotsPanels[posX].GetChild(posY).GetComponent<InventorySlot>().item == null){
			newSkill.transform.SetParent(slotsPanels[posX].GetChild(posY));
			newSkill.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);					
		}
		HasChanged();
	}

	/* Se invoca desde GameController*/	
	public void LoadItems(){
		List<Item> ilistAux = new List<Item>();
		foreach(Item item in PlayerItems.Inventory){
			GameObject it = (GameObject)Instantiate (itemUI, itemUI.transform.position, itemUI.transform.rotation);
			it = ItemCopy(item,it);			
			if(!item.IsEquipped){
				if(item != null){
					if(slotsPanels[item.InventoryPositionX].GetChild(item.InventoryPositionY).GetComponent<InventorySlot>().item == null){
						it.transform.SetParent(slotsPanels[item.InventoryPositionX].GetChild(item.InventoryPositionY));
						it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);					
					}
				}
			}
			else{
				switch(item.type){
					case ItemTypes.Weapon:
						if(weaponSlot.GetComponent<InventorySlot>().item == null){
							it.transform.SetParent(weaponSlot);
							it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
							PlayerItems.EquipedWeapon = it.GetComponent<Item>();
						}
						break;
					case ItemTypes.RangedWeapon:
						if(weaponSlot.GetComponent<InventorySlot>().item == null){
							it.transform.SetParent(weaponSlot);
							it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
							PlayerItems.EquipedWeapon = it.GetComponent<Item>();
						}
						break;
					case ItemTypes.TwoHandedWeapon:
						if(weaponSlot.GetComponent<InventorySlot>().item == null){
							it.transform.SetParent(weaponSlot);
							it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
							PlayerItems.EquipedWeapon = it.GetComponent<Item>();
						}
						break;
					case ItemTypes.Armour:
						if(armourSlot.GetComponent<InventorySlot>().item == null){
							it.transform.SetParent(armourSlot);
							it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
							PlayerItems.EquipedArmour = it.GetComponent<Item>();
						}
						break;
					case ItemTypes.Shield:
						if(shieldSlot.GetComponent<InventorySlot>().item == null){
							it.transform.SetParent(shieldSlot);
							it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
							PlayerItems.EquipedShield = it.GetComponent<Item>();
						}
						break;
					case ItemTypes.Helmet:
						if(headSlot.GetComponent<InventorySlot>().item == null){
							it.transform.SetParent(headSlot);
							it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
							PlayerItems.EquipedHelmet = it.GetComponent<Item>();
						}
						break;
					case ItemTypes.Belt:
						if(beltSlot.GetComponent<InventorySlot>().item == null){
							it.transform.SetParent(beltSlot);
							it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
							PlayerItems.EquipedBelt = it.GetComponent<Item>();
						}
						break;
					case ItemTypes.Amulet:
						if(amuletSlot.GetComponent<InventorySlot>().item == null){
						it.transform.SetParent(amuletSlot);
							it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
						}
						break;
					case ItemTypes.Ring:
						if(ringLSlot.GetComponent<InventorySlot>().item == null){
							it.transform.SetParent(ringLSlot);
							it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
							PlayerItems.EquipedRingL = it.GetComponent<Item>();
						}
						else{
							if(ringRSlot.GetComponent<InventorySlot>().item == null){
								it.transform.SetParent(ringRSlot);
								it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
								PlayerItems.EquipedRingR = it.GetComponent<Item>();						
							}
						}
						break;
				}
			}
			ilistAux.Add(it.GetComponent<Item>());
		}
		PlayerItems.Inventory = ilistAux;
		HasChanged();
	}	

	private GameObject ItemCopy(Item itemOrigin, GameObject itemDestiny){
		/*--- copia todo lo del item que estaba en el suelo al item que se ve en la ui del inventario ---*/
		Item itComponent = itemDestiny.GetComponent<Item>(); 
		itemDestiny.GetComponent<Image>().sprite = itemOrigin.sprite;
		
		itComponent.Atributes = itemOrigin.Atributes;
		itComponent.Offensives = itemOrigin.Offensives;		
		itComponent.Defensives = itemOrigin.Defensives;
		itComponent.Utils = itemOrigin.Utils;
		itComponent.Name = itemOrigin.Name;
		itComponent.sprite = itemOrigin.sprite;
		itComponent.Icon = itemOrigin.Icon;
		itComponent.Rarity = itemOrigin.Rarity;
		itComponent.Type = itemOrigin.Type;
		itComponent.InventoryPositionX = -1;
		itComponent.InventoryPositionY = -1;
		itComponent.IsEquipped = true;
		return itemDestiny;
	}

	/* se llama cada vez que hubo un cambio en algun slot */
	public void HasChanged ()
	{	
		int cantItems = 0;		
		for(int j = 0; j < slotsPanels.Length;j++) { //recorre el inventario y se fija los objetos y sus posiciones
			int i = 0;
			foreach(Transform slot in slotsPanels[j]){	
				InventorySlot sl = slot.GetComponent<InventorySlot>();
				GameObject item = sl.item;
				if(item != null){
					cantItems++;
					Item it = item.GetComponent<Item>();
					if(it != null){
						it.IsEquipped = false;
						it.InventoryPositionX = j;
						it.InventoryPositionY = i;
					}				
					else{
						SpellStats skill = item.GetComponent<SpellStats>();
						skill.InventoryPositionX = j;
						skill.InventoryPositionY = i;
						skill.IdSlotEquipped = -1;
					}	
				}
				i++;
			}
		}
		spellsPanel.HasChanged();
		PlayerItems.inventoryCantItems = cantItems;
		/* desequipar objetos */
		if(weaponSlot != null && weaponSlot.GetComponent<InventorySlot>().item == null)
			PlayerItems.EquipedWeapon = null;
		if(armourSlot != null && armourSlot.GetComponent<InventorySlot>().item == null)
			PlayerItems.EquipedArmour = null;
		if(shieldSlot != null && shieldSlot.GetComponent<InventorySlot>().item == null)
			PlayerItems.EquipedShield = null;
		if(headSlot != null && headSlot.GetComponent<InventorySlot>().item == null)
			PlayerItems.EquipedHelmet = null;
		if(beltSlot != null && beltSlot.GetComponent<InventorySlot>().item == null)
			PlayerItems.EquipedBelt = null;
		if(ringLSlot != null && ringLSlot.GetComponent<InventorySlot>().item == null)
			PlayerItems.EquipedRingL = null;
		if(ringRSlot != null && ringRSlot.GetComponent<InventorySlot>().item == null)
			PlayerItems.EquipedRingR = null;
		if(amuletSlot != null && amuletSlot.GetComponent<InventorySlot>().item == null)
			PlayerItems.EquipedAmulet = null;
	}

	/*Se invoca desde SpellsPanel para agregar un skill al inventario*/
	public bool AddSkill(GameObject skill){		
		for(int j = 0; j < slotsPanels.Length;j++) {
			int i = 0;
			foreach(Transform slot in slotsPanels[j]){
				GameObject item = slot.GetComponent<InventorySlot>().item;
				if(item == null){	
					skill.transform.SetParent(slot);					
					SpellStats skillStats = skill.GetComponent<SpellStats>();
					skillStats.InventoryPositionX = j;
					skillStats.InventoryPositionY = i;
					skillStats.IdSlotEquipped = -1; //no esta equipado
					skill.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
					PlayerItems.SpellsInvetory.Add(skillStats);
					PlayerItems.inventoryCantItems++;
					HasChanged();
					i++;
					return true;
				}	
			}
		}
		return false;	
	}

	/* Este metodo se llama desde PlatformerCharacter2D cuando se agarra un item*/
	public bool AddItem(Item newItem){
		GameObject it = (GameObject)Instantiate (itemUI, itemUI.transform.position, itemUI.transform.rotation);

		/*--- copia todo lo del item que estaba en el suelo al item que se ve en la ui del inventario ---*/
		it = ItemCopy(newItem,it);
		Item itComponent = it.GetComponent<Item>();

		/* se fija si el slot que corresponde al tipo del item no esta ocupado, para que el item se coloque en dicho slot */
		if(PlayerItems.EquipedWeapon == null && itComponent.Type == ItemTypes.Weapon){ //si el slot del arma no esta ocupado pongo ahi el nuevo item
			it.transform.SetParent(weaponSlot);
			it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
			PlayerItems.EquipedWeapon = itComponent;
			PlayerItems.Inventory.Add(itComponent);
			return true;
		}
		if(PlayerItems.EquipedArmour == null && itComponent.Type == ItemTypes.Armour){
			it.transform.SetParent(armourSlot);
			it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
			PlayerItems.EquipedArmour = itComponent;
			PlayerItems.Inventory.Add(itComponent);
			return true;
		}
		if(PlayerItems.EquipedShield == null && itComponent.Type == ItemTypes.Shield){
			if(!(PlayerItems.EquipedWeapon == null) && PlayerItems.EquipedWeapon.Type == ItemTypes.RangedWeapon){} //si hay un arco equipado no hago nada
			else{
				it.transform.SetParent(shieldSlot);
				it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
				PlayerItems.EquipedShield = itComponent;
				PlayerItems.Inventory.Add(itComponent);
				return true;
			}						
		}
		if(PlayerItems.EquipedHelmet == null && itComponent.Type == ItemTypes.Helmet){
			it.transform.SetParent(headSlot);
			it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
			PlayerItems.EquipedHelmet = itComponent;
			PlayerItems.Inventory.Add(itComponent);
			return true;
		}
		if(PlayerItems.EquipedBelt == null && itComponent.Type == ItemTypes.Belt){
			it.transform.SetParent(beltSlot);
			it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
			PlayerItems.EquipedBelt = itComponent;
			PlayerItems.Inventory.Add(itComponent);
			return true;
		}
		if(PlayerItems.EquipedAmulet == null && itComponent.Type == ItemTypes.Amulet){
			it.transform.SetParent(amuletSlot);
			it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
			PlayerItems.EquipedAmulet = itComponent;
			PlayerItems.Inventory.Add(itComponent);
			return true;
		}
		if(PlayerItems.EquipedRingL == null && itComponent.Type == ItemTypes.Ring){
			it.transform.SetParent(ringLSlot);
			it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
			PlayerItems.EquipedRingL = itComponent;
			PlayerItems.Inventory.Add(itComponent);
			return true;
		}
		if(PlayerItems.EquipedRingR == null && itComponent.Type == ItemTypes.Ring){
			it.transform.SetParent(ringRSlot);
			it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
			PlayerItems.EquipedRingR = itComponent;
			PlayerItems.Inventory.Add(itComponent);
			return true;
		}	
		/*--------------------------------------------------------------------*/	

		/*si el slot estaba ocupado, el item se ubica en un slot vacio del inventario*/
		for(int j = 0; j < slotsPanels.Length;j++) {
			int i = 0;
			foreach(Transform slot in slotsPanels[j]){
				GameObject item = slot.GetComponent<InventorySlot>().item;
				if(item == null){	
					it.transform.SetParent(slot);
					
					itComponent.InventoryPositionX = j;
					itComponent.InventoryPositionY = i;
					itComponent.IsEquipped = false;
					it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
					PlayerItems.Inventory.Add(itComponent);
					PlayerItems.inventoryCantItems++;
					HasChanged();
					i++;
					return true;
				}	
			}
		}
		return false;
	}

}
