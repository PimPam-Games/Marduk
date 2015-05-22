using UnityEngine;
using System.Collections;

public class CreateObject : MonoBehaviour {

	public GameObject objectToCreate;
	public bool loop;
	public float delay;
	
	// Use this for initialization
	void Start () {
		StartCoroutine (InstantiateObject());
	}

	IEnumerator InstantiateObject () {
		yield return new WaitForSeconds (delay);
		Instantiate (objectToCreate, transform.position, transform.rotation);
		while (loop) {
			yield return new WaitForSeconds (delay);
			Instantiate (objectToCreate, transform.position, transform.rotation);
		}
	}

}
