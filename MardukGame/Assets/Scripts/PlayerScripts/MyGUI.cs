﻿using UnityEngine;
using System.Collections;
using p = PlayerStats;

public class MyGUI : MonoBehaviour {

	public float buttonWidth = 35;
	public float buttonHeight = 35;
	public GUIStyle inventoryStyle;
	public GUIStyle characterStyle;
	private float dif = 1;

	/*Variables del inventario */
	private static bool  displayInventoryWindow = false;
	private const int INVENTORY_WINDOW_ID = 1;
	private Rect inventoryWindowRect =  new Rect(1000,150,252,400);
	private int inventoryRows = 4;
	private int inventoryCols = 7;


	/*variables de la ventana del personaje*/
	private static bool  displayCharacterWindow = false;
	private const int CHARACTER_WINDOW_ID = 2;
	private Rect characterWindowRect =  new Rect(100,150,252,400);
	private int characterPanel = 0;
	private string[] characterPanelNames = new string[] {" "," "," "};

	//private float doubleClickTimer = 0;
//	private const float DBCLICK_TIMER_THRESHOLD =  0.5f;
	private Item selectedItem;
	private string toolTip = "";
	//private Vector2 inventoryWindowSlider = Vector2.zero

	public static bool InventoryOpen(){
		return displayInventoryWindow;
	}

	public static bool CharacterWindowOpen(){
		return displayCharacterWindow;
	}

	void OnGUI(){
		//GUI.skin = mySkin;
		if (displayCharacterWindow) {
			characterWindowRect.Set(100*dif,1*dif,360*dif,600*dif);
			characterWindowRect = GUI.Window (CHARACTER_WINDOW_ID, characterWindowRect, CharacterWindow,"",characterStyle);
			
		}
		if (displayInventoryWindow) {
			inventoryWindowRect.Set(900*dif,1*dif,360*dif,600*dif);
			inventoryWindowRect = GUI.Window (INVENTORY_WINDOW_ID, inventoryWindowRect, InventoryWindow,"",inventoryStyle );

		}
		DisplayTooltip ();
	}


	public void CharacterWindow(int id){
		GUIStyle toolbarStyle = new GUIStyle();

		toolbarStyle.margin = new RectOffset(25, 25, 10, 10);
		characterPanel = GUI.Toolbar (new Rect(50*dif,250*dif,250*dif,20*dif),characterPanel,characterPanelNames,toolbarStyle);

		GUI.Label (new Rect (180*dif, 127*dif, 100*dif, 20*dif), p.atributes[p.Strength].ToString());
		GUI.Label (new Rect (180*dif, 155*dif, 100*dif, 20*dif),  p.atributes[p.Dextery].ToString());
		GUI.Label (new Rect (180*dif, 184*dif, 100*dif, 20*dif),p.atributes[p.Vitality].ToString());
		GUI.Label (new Rect (180*dif, 210*dif, 100*dif, 20*dif), p.atributes[p.Spirit].ToString());
		switch (characterPanel) {
		case 0:
			DisplayOffensives();
			break;
		case 1:
			DisplayDfensives();
			break;
		case 2:
			DisplayMisc();
			break;
		}
		GUI.DragWindow ();
	}

	private void DisplayOffensives(){
		GUI.Label (new Rect (65*dif, 270*dif, 100*dif, 20*dif), "Physical Damage");
		GUI.Label (new Rect (250*dif, 270*dif, 100*dif, 20*dif), p.offensives[p.MinDmg]+" - "+p.offensives[p.MaxDamge]);
		GUI.Label (new Rect (65*dif, 285*dif, 100*dif, 20*dif), "Thorns");
		GUI.Label (new Rect (250*dif, 285*dif, 100*dif, 20*dif), p.defensives[p.Thorns].ToString());


	}

	private void DisplayDfensives(){
		GUI.Label (new Rect (65*dif, 270*dif, 200*dif, 20*dif), "Defense");
		GUI.Label (new Rect (250*dif, 270*dif, 100*dif, 20*dif), p.defensives[p.Defense].ToString());
		GUI.Label (new Rect (65*dif, 285*dif, 200*dif, 20*dif), "Life Regenerated Per Second");
		GUI.Label (new Rect (250*dif, 285*dif, 100*dif, 20*dif), p.defensives[p.LifePerSecond].ToString());
		GUI.Label (new Rect (65*dif, 300*dif, 200*dif, 20*dif), "Cold Resistance");
		GUI.Label (new Rect (250*dif, 300*dif, 100*dif, 20*dif), p.defensives[p.ColdRes].ToString());
		GUI.Label (new Rect (65*dif, 315*dif, 200*dif, 20*dif), "Fire Resistance");
		GUI.Label (new Rect (250*dif, 315*dif, 100*dif, 20*dif), p.defensives[p.FireRes].ToString());
		GUI.Label (new Rect (65*dif, 330*dif, 200*dif, 20*dif), "Lightning Resistance");
		GUI.Label (new Rect (250*dif, 330*dif, 100*dif, 20*dif), p.defensives[p.LightRes].ToString());
		GUI.Label (new Rect (65*dif, 345*dif, 200*dif, 20*dif), "Poison Resistance");
		GUI.Label (new Rect (250*dif, 345*dif, 100*dif, 20*dif), p.defensives[p.PoisonRes].ToString());
		GUI.Label (new Rect (65*dif, 360*dif, 200*dif, 20*dif), "Block chance");
		GUI.Label (new Rect (250*dif, 360*dif, 100*dif, 20*dif), p.defensives[p.BlockChance].ToString());
		GUI.Label (new Rect (65*dif, 375*dif, 200*dif, 20*dif), "Evasiveness");
		GUI.Label (new Rect (250*dif, 375*dif, 100*dif, 20*dif), p.defensives[p.Evasiveness].ToString());
	}

	private void DisplayMisc(){
		GUI.Label (new Rect (65*dif, 270*dif, 200*dif, 20*dif), "Movement Speed");
		GUI.Label (new Rect (250*dif, 270*dif, 100*dif, 20*dif), p.utils[p.MovementSpeed].ToString());
		GUI.Label (new Rect (65*dif, 285*dif, 200*dif, 20*dif), "Magic Find");
		GUI.Label (new Rect (250*dif, 285*dif, 100*dif, 20*dif), p.utils[p.MagicFind].ToString());
	}

	public void InventoryWindow(int id){
		int cnt = 0;
		if (PlayerItems.EquipedWeapon != null) { //Slot del arma
			if (GUI.Button (new Rect (59 *dif , 181 * dif, 60 * dif, 129 * dif), new GUIContent(PlayerItems.EquipedWeapon.Icon,PlayerItems.EquipedWeapon.ToolTip()))) {
				if(Input.GetMouseButtonUp(0)) {
					if(PlayerItems.InventoryMaxSize > PlayerItems.inventoryCantItems){
						PlayerItems.Inventory.Add(PlayerItems.EquipedWeapon);
						PlayerItems.EquipedWeapon = null;
						PlayerItems.inventoryCantItems++;

					}
				}
			}
			SetTooltip();
		}
		if (PlayerItems.EquipedArmour != null) { //Slot de la armadura
			if (GUI.Button (new Rect (150 *dif , 181 * dif, 60 * dif, 129 * dif), new GUIContent(PlayerItems.EquipedArmour.Icon,PlayerItems.EquipedArmour.ToolTip()))) {
				if(Input.GetMouseButtonUp(0)) {
					if(PlayerItems.InventoryMaxSize > PlayerItems.inventoryCantItems){
						PlayerItems.Inventory.Add(PlayerItems.EquipedArmour);
						PlayerItems.EquipedArmour = null;
						PlayerItems.inventoryCantItems++;

					}
				}
			}
			SetTooltip();
		}
		if (PlayerItems.EquipedHelmet != null) { //Slot del casco
			if (GUI.Button (new Rect (159 *dif , 93 * dif, 41 * dif, 58 * dif), new GUIContent(PlayerItems.EquipedHelmet.Icon,PlayerItems.EquipedHelmet.ToolTip()))) {
				if(Input.GetMouseButtonUp(0)) {
					if(PlayerItems.InventoryMaxSize > PlayerItems.inventoryCantItems){
						PlayerItems.Inventory.Add(PlayerItems.EquipedHelmet);
						PlayerItems.EquipedHelmet = null;
						PlayerItems.inventoryCantItems++;
						Debug.Log(PlayerItems.inventoryCantItems);
					}
				}
			}
			SetTooltip();
		}
		if (PlayerItems.EquipedShield != null) { //Slot del escudo
			if (GUI.Button (new Rect (240 *dif , 181 * dif, 60 * dif, 129 * dif), new GUIContent(PlayerItems.EquipedShield.Icon,PlayerItems.EquipedShield.ToolTip()))) {
				if(Input.GetMouseButtonUp(0)) {
					if(PlayerItems.InventoryMaxSize > PlayerItems.inventoryCantItems){
						PlayerItems.Inventory.Add(PlayerItems.EquipedShield);
						PlayerItems.EquipedShield = null;
						PlayerItems.inventoryCantItems++;
					}
				}
			}
			SetTooltip();
		}
		GUI.Button (new Rect (159 *dif , 93 * dif, 41 * dif, 58 * dif), "", "box"); // casco
		GUI.Button (new Rect (59 *dif , 181 * dif, 60 * dif, 129 * dif), "", "box"); //arma
		GUI.Button (new Rect (240 *dif , 181 * dif, 60 * dif, 129 * dif), "", "box"); // escudo
		GUI.Button (new Rect (150 *dif , 181 * dif, 60 * dif, 129 * dif), "", "box"); //armadura
		for (int y =0; y<inventoryRows; y++) {
			for(int x=0; x <inventoryCols; x++){
				if(cnt < PlayerItems.Inventory.Count){
					if(GUI.Button(new Rect((60+(x*buttonWidth))*dif,(348+(y*buttonHeight))*dif, buttonWidth*dif,buttonHeight*dif), new GUIContent(PlayerItems.Inventory[cnt].Icon,PlayerItems.Inventory[cnt].ToolTip()))){
						//if(doubleClickTimer != 0 && selectedItem != null){
						//	if(Time.time - doubleClickTimer < DBCLICK_TIMER_THRESHOLD){
						if(Input.GetMouseButtonUp(0)) {

							if(PlayerItems.Inventory[cnt].Type == ItemTypes.Weapon){ // arma equipada
								if(PlayerItems.EquipedWeapon == null){
									PlayerItems.EquipedWeapon = PlayerItems.Inventory[cnt];
									PlayerItems.Inventory.RemoveAt(cnt);
									PlayerItems.inventoryCantItems--;
								}
								else{
									Item temp = PlayerItems.EquipedWeapon;
									PlayerItems.EquipedWeapon = PlayerItems.Inventory[cnt];
									PlayerItems.Inventory[cnt] = temp;
								}
							}
							else{
								if(PlayerItems.Inventory[cnt].Type == ItemTypes.Armour){ // armadura equipada
									if(PlayerItems.EquipedArmour == null){
										PlayerItems.EquipedArmour = PlayerItems.Inventory[cnt];
										PlayerItems.Inventory.RemoveAt(cnt);
										PlayerItems.inventoryCantItems--;
									}
									else{
										Item temp = PlayerItems.EquipedArmour;
										PlayerItems.EquipedArmour = PlayerItems.Inventory[cnt];
										PlayerItems.Inventory[cnt] = temp;
									}
								}
								else{
									if(PlayerItems.Inventory[cnt].Type == ItemTypes.Helmet){ // casco equipado
										if(PlayerItems.EquipedHelmet == null){  
											PlayerItems.EquipedHelmet = PlayerItems.Inventory[cnt];
											PlayerItems.Inventory.RemoveAt(cnt);
											PlayerItems.inventoryCantItems--;
										}
										else{
											Item temp = PlayerItems.EquipedHelmet;
											PlayerItems.EquipedHelmet = PlayerItems.Inventory[cnt];
											PlayerItems.Inventory[cnt] = temp;
										}
									}
									else{
										if(PlayerItems.Inventory[cnt].Type == ItemTypes.Shield){ // escudo equipado
											if(PlayerItems.EquipedShield == null){  
												PlayerItems.EquipedShield = PlayerItems.Inventory[cnt];
												PlayerItems.Inventory.RemoveAt(cnt);
												PlayerItems.inventoryCantItems--;
											}
											else{
												Item temp = PlayerItems.EquipedShield;
												PlayerItems.EquipedShield = PlayerItems.Inventory[cnt];
												PlayerItems.Inventory[cnt] = temp;
											}
										}
									}
								}

							}

						}

						if(Input.GetMouseButtonUp(1)) {
							PlayerItems.Inventory.RemoveAt(cnt);
							PlayerItems.inventoryCantItems--;

						}
								//doubleClickTimer = 0;
								//selectedItem = null;
							//}else
							//	doubleClickTimer = Time.time;
						//}else{
						//	doubleClickTimer = Time.time;
						//	selectedItem = PlayerItems.Inventory[cnt];
					}
					SetTooltip();
				}
				else{
					GUI.Button(new Rect((60+(x*buttonWidth))*dif,(348+(y*buttonHeight))*dif, buttonWidth*dif,buttonHeight*dif),"","box");//(x+y*inventoryCols).ToString());
				}
				//Debug.Log(cnt);
				cnt++;
			}
		}

		GUI.DragWindow ();
	}

	public void ToggleInventoryWindow(){
		displayInventoryWindow = !displayInventoryWindow;
	}

	public void ToggleCharacterWindow(){
		displayCharacterWindow = !displayCharacterWindow;
	}



	private void SetTooltip(){
		if (Event.current.type == EventType.Repaint && GUI.tooltip != toolTip) {
			if(toolTip != "")
				toolTip = "";
			if(GUI.tooltip != "")
				toolTip = GUI.tooltip;
		}
	}

	private void DisplayTooltip(){
		if(toolTip != "" && displayInventoryWindow)
			GUI.Box (new Rect (735*dif,200*dif,185*dif,180*dif),toolTip);
	}
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		dif = (Screen.width/12.8f) / 100;
	}
}
