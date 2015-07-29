using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class Slot : MonoBehaviour, IDropHandler {

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
	
}
