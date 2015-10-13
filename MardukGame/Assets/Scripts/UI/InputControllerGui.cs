using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputControllerGui : MonoBehaviour {

	//este script esta en el gameobject GUI, 

	private MyGUI gui;
	public GameObject traitsPanel;
	public GameObject teleporterPanel;
	public static bool tpOpen = false;
	public static bool toggleTeleporterPanel;
	//private bool gamePaused = false;

	public Image menuInGame;
	// Use this for initialization
	void Awake () {


		gui = GetComponent<MyGUI>();
	}



	// Update is called once per frame
	void Update () {
		if (Input.GetButtonUp ("ToggleInventory") && !menuInGame.IsActive()) {
			gui.ToggleInventoryWindow();
			SetMouseVisible();
		}
		if (Input.GetButtonUp ("ToggleCharacterWindow") && !menuInGame.IsActive()) {
			gui.ToggleCharacterWindow();
			SetMouseVisible();
		}
		if (Input.GetButtonUp ("ToggleTraits") && !menuInGame.IsActive()) {
			if(traitsPanel.activeSelf)
				traitsPanel.SetActive(false);
			else
				traitsPanel.SetActive(true);
			SetMouseVisible();
		}
		if(Input.GetButtonUp ("Save")){
			Persistence.Save();
			Debug.Log("Save Data");
		}
		if (Input.GetButtonUp ("Load")) {
			Persistence.Load("Hola");
			Debug.Log("Load Data");
		}
		if (Input.GetButtonUp ("Escape")) {
			Debug.Log("Pause");
			if(Time.timeScale == 1.0f){
				Time.timeScale = 0;
				//gamePaused = true;
				menuInGame.gameObject.SetActive(true);
				SetMouseVisible();
			}
			else{
				Time.timeScale = 1.0f;
				//gamePaused = false;
				menuInGame.gameObject.SetActive(false);
				SetMouseVisible();
			}
		}
		if (toggleTeleporterPanel) {
			toggleTeleporterPanel = false;
			if(tpOpen)
				teleporterPanel.SetActive(true);
			else
				teleporterPanel.SetActive(false);
			SetMouseVisible();
		}
	}

	public static void OpenTeleporterPanel(){
		if (!tpOpen) {
			toggleTeleporterPanel = true;
			tpOpen = true;
		}
	}

	public static void CloseTeleporterPanel(){
		if (tpOpen) {
			toggleTeleporterPanel = true;
			tpOpen = false;
		}
	}

	private void SetMouseVisible(){
		if(MyGUI.InventoryOpen() || MyGUI.CharacterWindowOpen() || menuInGame.IsActive() || traitsPanel.activeSelf || tpOpen)
			Cursor.visible = true;
		else
			Cursor.visible = false;
	}
}
