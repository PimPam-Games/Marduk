using UnityEngine;
using System.Collections;
using g = GameController;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;
using System;

public class TeleporterPanel : MonoBehaviour {

	public static bool updateTeleporterUI = false;
	public Image[] buttons;
	public Text[] btnTexts;

	void Start(){
		UpdateUi();
	}

	void Update(){
		if(updateTeleporterUI){
			updateTeleporterUI = false;
			UpdateUi();
		}
	}

	private void UpdateUi(){
		for(int i = 0; i < buttons.Length; i++){
			
			if(!PlayerItems.playerTeleporters[i]){
				btnTexts[i].color = new Color(0,0,0,0.4f);
			}
			else{
				btnTexts[i].color = new Color(0,0,0,1);
				buttons[i].color = new Color(1,1,1,0.15f);
			}
		}
	}

	public void UseTeleporter(string levelAndTp){
		string levelToLoad = levelAndTp.Split(' ')[0]; //el parametro se divide en 2, ej: "level5 3"
		int index = Int32.Parse(levelAndTp.Split (' ') [1]); //index es el numero del transportador en el arreglo de los tps
		if (string.Compare(g.currLevelName,levelToLoad) == 0)
			return;
		if (!PlayerItems.playerTeleporters[index]) //si el jugador no tiene este transportador no puede ir a esa zona
			return;
		g.previousExit = 7; //por convencion la salida 7 es para entrar en algun otro transportador
		g.jumpOnLoad = false;
		Fading.BeginFadeIn(levelToLoad);
		Debug.Log (levelToLoad);
	}
}