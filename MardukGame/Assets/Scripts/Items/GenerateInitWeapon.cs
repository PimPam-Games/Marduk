using UnityEngine;
using System.Collections;

public class GenerateInitWeapon : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<ItemGenerator> ().createInitWeapon (transform.position,transform.rotation);
	}
	
}
