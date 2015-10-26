using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class InventorySlot :  MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler{

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

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
		if (!item)
			DragHandeler.itemBeingDragged.transform.SetParent (transform);
		else {
			item.transform.SetParent(DragHandeler.startParent);
			DragHandeler.itemBeingDragged.transform.SetParent (transform);
		}
		ExecuteEvents.ExecuteHierarchy<IHasChanged> (gameObject, null, (x,y) => x.HasChanged ());
	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		Debug.Log("pointer enter");
	}


	public void OnPointerExit (PointerEventData eventData)
	{
		Debug.Log("pointer exit");
	}

	public void OnPointerClick (PointerEventData eventData)
	{
		Debug.Log("pointer click");
	}

}
