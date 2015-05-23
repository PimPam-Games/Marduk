using UnityEngine;
using System.Collections;

public class CreateObject : MonoBehaviour {

	public GameObject objectToCreate;
	public bool loop;
	public float delay;
	public Direction rotation;
	Quaternion rot;
	public enum Direction{Right, Left, Up ,Down};
	// Use this for initialization
	void Start () {

		rot = Quaternion.identity;
		switch (rotation)
		{
		case Direction.Down:
			rot.eulerAngles = new Vector3(90, 180, 0);
			break;
		case Direction.Up:
			rot.eulerAngles = new Vector3(-90, 0, 0);
			break;
		case Direction.Right:
			rot.eulerAngles = new Vector3(0, 90, 0);
			break;
		case Direction.Left:
			rot.eulerAngles = new Vector3(0, 270, 0);
			break;
		default:
			rot.eulerAngles = new Vector3(-90, 0, 0);
			break;
		}
		StartCoroutine (InstantiateObject());
	}

	IEnumerator InstantiateObject () {
		yield return new WaitForSeconds (delay);
	    Instantiate (objectToCreate, transform.position, rot);
		while (loop) {
			yield return new WaitForSeconds (delay);
			Instantiate (objectToCreate, transform.position, rot);
		}
	}

}
