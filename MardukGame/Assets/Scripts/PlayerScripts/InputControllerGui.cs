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
			SetMouseVisible();
		}
		if (Input.GetButtonUp ("ToggleCharacterWindow")) {
			gui.ToggleCharacterWindow();
			SetMouseVisible();
		}
		if(Input.GetButtonUp ("Save")){
			Persistence.Save();
			Debug.Log("Save Data");
		}
		if (Input.GetButtonUp ("Load")) {
			Persistence.Load();
			Debug.Log("Load Data");
		}
	}

	private void SetMouseVisible(){
		if(MyGUI.InventoryOpen() || MyGUI.CharacterWindowOpen())
			Cursor.visible = true;
		else
			Cursor.visible = false;
	}
}
