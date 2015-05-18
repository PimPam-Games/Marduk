using UnityEngine;
using System.Collections;

public class InputControllerGui : MonoBehaviour {

	private MyGUI gui;
	// Use this for initialization
	void Awake () {
		gui = GetComponent<MyGUI>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonUp ("ToggleInventory")) {
			gui.ToggleInventoryWindow();
		}
		if (Input.GetButtonUp ("ToggleCharacterWindow")) {
			gui.ToggleCharacterWindow();
		}
	}
}
