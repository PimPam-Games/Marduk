using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {

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

	public void OnDrop(PointerEventData eventData){
		if (!item)
			DragHandeler.itemBeingDragged.transform.SetParent (transform);
		else {
			item.transform.SetParent(DragHandeler.startParent);
			DragHandeler.itemBeingDragged.transform.SetParent (transform);
		}
	}
	
}
