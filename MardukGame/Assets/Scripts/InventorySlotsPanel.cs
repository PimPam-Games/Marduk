using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using pitems = PlayerItems;

public class InventorySlotsPanel : MonoBehaviour, IHasChanged {

	public Transform[] slotsPanels;

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
				GameObject item = null;
				if(sl != null)
					item = sl.item;
				if(item != null)
					Debug.Log("item en posicion " + i);
				i++;
			}
		}
	}

	public bool AddItem(string itName){
		Debug.Log(itName);
		GameObject newItemPrefab = (GameObject)Resources.Load ("ItemsUI/" + itName);
		if(newItemPrefab == null){
			Debug.LogError("prefab del item no encontrado en ItemsUI");
			return false;	
		}
		GameObject newItem = (GameObject)Instantiate (newItemPrefab, newItemPrefab.transform.position, newItemPrefab.transform.rotation);
		for(int j = 0; j < slotsPanels.Length;j++) {
			foreach(Transform slot in slotsPanels[j]){
				GameObject item = slot.GetComponent<InventorySlot>().item;
				if(item == null){
					newItem.transform.SetParent(slot);
					newItem.GetComponent<RectTransform>().localScale = new Vector3(2.5f,2.5f,1);
					PlayerItems.Inventory.Add (newItem.GetComponent<Item>());
					PlayerItems.inventoryCantItems++;
					HasChanged();
					return true;
				}	
				
			}
		}
		return true;
	}

}
