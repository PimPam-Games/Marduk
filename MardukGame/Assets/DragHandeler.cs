using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class DragHandeler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler  {

	public static GameObject itemBeingDragged;
	Vector3 startPosition;

	#region IBeginDragHandler implementation

	public void OnBeginDrag (PointerEventData eventData)
	{
		Debug.Log ("asdasdasdasdasdasdasd");
		itemBeingDragged = gameObject;
		startPosition = transform.position;
	}

	#endregion

	#region IDragHandler implementation

	public void OnDrag (PointerEventData eventData)
	{
		Debug.Log ("asdgggggggggggggasdasdasd");
		transform.position = Input.mousePosition;
	}

	#endregion

	#region IEndDragHandler implementation

	public void OnEndDrag (PointerEventData eventData)
	{
		itemBeingDragged = null;
		transform.position = startPosition;
	}

	#endregion
}
