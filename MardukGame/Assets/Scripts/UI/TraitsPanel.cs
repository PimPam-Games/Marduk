using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using g = GameController;
using p = PlayerStats;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TraitsPanel : MonoBehaviour{

	private static List<GameObject> buttons;

	// Use this for initialization
	void Start () {
		buttons = new List<GameObject> ();
		foreach (Transform child in this.transform) {
			buttons.Add(child.gameObject);
		}
	}

	// Update is called once per frame
	void Update () {
		
	}
	
	public void clickOnTrait(int tName) {
		if (!Traits.traits [tName].isActive ()) {
			Traits.activate(tName);
		} else {
			Traits.deactivate(tName);
		}
	}

	public void clickOnReset () {
		Traits.reset();
	}


}
