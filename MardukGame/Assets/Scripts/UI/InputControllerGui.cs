using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class InputControllerGui : MonoBehaviour {

	//este script esta en el gameobject GUI, 

	//private MyGUI gui;
	public GameObject traitsPanel;
	public GameObject traitsTooltip;
	public GameObject teleporterPanel;
	public GameObject characterPanel;
	public GameObject inventory;
	public GameObject invTooltip;
	public GameObject[] SpellPanelSlots;
    public CameraController minimap;

	public static bool tpOpen = false;
	public static bool toggleTeleporterPanel;
	public static bool resumePressed = false;
	public static bool invOpen = false;
	public static bool closeInventory = false; // se usa para cerrar el inventario luego de que se hallan cargando las skills al iniciar el juego 
	//private bool gamePaused = false;

	public Image menuInGame;
	// Use this for initialization

	// Update is called once per frame
	void Update () {
		if(closeInventory){
			closeInventory = false;
			inventory.SetActive(false);
			for(int i = 0; i < SpellPanelSlots.Length; i++){
				SpellPanelSlots[i].GetComponent<Image>().enabled = false;
				
				if(SpellPanelSlots[i].transform.childCount > 0){
					SpellPanelSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
				}
			}
		}
		if(PlayerStats.isDead)
			return;
        if (Input.GetButtonUp("ToggleMinimap"))
        {
            minimap.ToggleMinimap();
        }
        
        if (Input.GetButtonUp ("ToggleInventory") && !menuInGame.IsActive()) {
			//gui.ToggleInventoryWindow();
			if(inventory.activeSelf){
				if(!Input.GetMouseButton(0)){
					inventory.SetActive(false);
					invTooltip.SetActive(false);
					invOpen = false;
					for(int i = 0; i < SpellPanelSlots.Length; i++){
						SpellPanelSlots[i].GetComponent<Image>().enabled = false;
						
						if(SpellPanelSlots[i].transform.childCount > 0){
							SpellPanelSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = false;
						}
					}
				}
			}
			else{
				inventory.SetActive(true);
				invOpen = true;
                TutorialController.inventoryTutorialOn = false;
                TutorialController.invTutorialShowed = true;
                for (int i = 0; i < SpellPanelSlots.Length; i++){
					SpellPanelSlots[i].GetComponent<Image>().enabled = true;
					if(SpellPanelSlots[i].transform.childCount > 0){
						SpellPanelSlots[i].transform.GetChild(0).GetComponent<Image>().enabled = true;
					}
				}
			}
			SetMouseVisible();
		}
        if (Input.GetButtonUp("ToggleCharacterWindow") && !menuInGame.IsActive()) {
            //gui.ToggleCharacterWindow();
            if (characterPanel.activeSelf)
                characterPanel.SetActive(false);
            else {
                characterPanel.SetActive(true);
                TutorialController.attributesTutorialOn = false;
                TutorialController.attributesTutorialShowed = true;
            }
            SetMouseVisible();
		}
		if (Input.GetButtonUp ("ToggleTraits") && !menuInGame.IsActive()) {
            if (traitsPanel.activeSelf)
            {
                traitsPanel.SetActive(false);
                traitsTooltip.SetActive(false);
            }
            else {
                traitsPanel.SetActive(true);
                TutorialController.traitsTutorialOn = false;
                TutorialController.traitsTutorialShowed = true;
            }
			SetMouseVisible();
		}
		/*if(Input.GetButtonUp ("Save")){
			//Persistence.Save();
			//Debug.Log("Save Data");
		}
		if (Input.GetButtonUp ("Load")) {
			//Persistence.Load("Hola");
			//Debug.Log("Load Data");
		}*/
		if (Input.GetButtonUp ("Escape") || resumePressed) {
			Debug.Log("Pause");
			resumePressed = false;
			if(inventory.activeSelf || characterPanel.activeSelf || traitsPanel.activeSelf){
				traitsPanel.SetActive(false);
				traitsTooltip.SetActive(false);
				characterPanel.SetActive(false);
				inventory.SetActive(false);
				invTooltip.SetActive(false);
				invOpen = false;
				Cursor.visible = false;
                closeInventory = true;
				return;
			}
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
		if(inventory.activeSelf || characterPanel.activeSelf || menuInGame.IsActive() || traitsPanel.activeSelf || tpOpen)
			Cursor.visible = true;
		else
			Cursor.visible = false;
	}
}
