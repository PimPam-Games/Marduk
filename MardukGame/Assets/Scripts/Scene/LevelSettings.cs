using UnityEngine;
using System.Collections;

public class LevelSettings : MonoBehaviour {

	public int enemiesLvl = 1;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonUp ("LvlDown") && enemiesLvl > 1) {
			enemiesLvl--;
			Debug.Log("Enemies lvl Down");
		}
		if (Input.GetButtonUp ("LvlUp")) {
			enemiesLvl++;
			Debug.Log ("Enemies lvl Up");
		}
	}
}
