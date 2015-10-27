using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using pitems = PlayerItems;
using UnityEngine.UI;

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
	
	//public GameObject itemUI;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void HasChanged ()
	{
		int i = 0;
		for(int j = 0; j < slotsPanels.Length;j++) {
			foreach(Transform slot in slotsPanels[j]){
				InventorySlot sl = slot.GetComponent<InventorySlot>();
				GameObject item = sl.item;
				if(item != null){
					item.GetComponent<Item>().IsEquipped = false;
					item.GetComponent<Item>().InventoryPosition = i;
				}
				i++;
			}
		}
		if(weaponSlot != null && weaponSlot.GetComponent<InventorySlot>().item == null)
			PlayerItems.EquipedWeapon = null;
		if(armourSlot != null && armourSlot.GetComponent<InventorySlot>().item == null)
			PlayerItems.EquipedArmour = null;
		if(shieldSlot != null && shieldSlot.GetComponent<InventorySlot>().item == null)
			PlayerItems.EquipedShield = null;
		if(weaponSlot != null && weaponSlot.GetComponent<InventorySlot>().item == null)
			PlayerItems.EquipedWeapon = null;
		if(weaponSlot != null && weaponSlot.GetComponent<InventorySlot>().item == null)
			PlayerItems.EquipedWeapon = null;
		if(weaponSlot != null && weaponSlot.GetComponent<InventorySlot>().item == null)
			PlayerItems.EquipedWeapon = null;
		if(weaponSlot != null && weaponSlot.GetComponent<InventorySlot>().item == null)
			PlayerItems.EquipedWeapon = null;
	}

	public bool AddItem(Item newItem){
		//Debug.Log(itName);
		GameObject itemUI = (GameObject)Resources.Load ("ItemsUI/ItemUI");
		if(itemUI == null){
			Debug.LogError("prefab del item no encontrado en ItemsUI");
			return false;	
		}
		GameObject it = (GameObject)Instantiate (itemUI, itemUI.transform.position, itemUI.transform.rotation);
		
		for(int j = 0; j < slotsPanels.Length;j++) {
			foreach(Transform slot in slotsPanels[j]){
				GameObject item = slot.GetComponent<InventorySlot>().item;
				if(item == null){
					Item itComponent = it.GetComponent<Item>(); //copia todo lo del item que estaba en el suelo al item que se ve en la ui del inventario
					itComponent.Atributes = newItem.Atributes;
					itComponent.Offensives = newItem.Offensives;		
					itComponent.Defensives = newItem.Defensives;
					itComponent.Utils = newItem.Utils;
					itComponent.Name = newItem.Name;
					itComponent.sprite = newItem.sprite;
					itComponent.Icon = newItem.Icon;
					itComponent.Rarity = newItem.Rarity;
					itComponent.Type = newItem.Type;
					it.GetComponent<Image>().sprite = newItem.sprite;
					it.transform.SetParent(slot);
					itComponent.InventoryPosition = j;
					it.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
					PlayerItems.Inventory.Add(itComponent);
					PlayerItems.inventoryCantItems++;
					HasChanged();
					return true;
				}	
			}
		}
		return false;
	}

}
