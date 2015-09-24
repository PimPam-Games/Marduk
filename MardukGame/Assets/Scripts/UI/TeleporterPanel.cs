using UnityEngine;
using System.Collections;
using g = GameController;
using System.Collections.Generic;
using System.Text.RegularExpressions;

public class TeleporterPanel : MonoBehaviour {

	//private static Canvas[] canvas;
	private static List<GameObject> buttons;
	public static bool isOpen;

	// Use this for initialization
	void Start () {
		//canvas = (Canvas)GetComponentsInChildren<Canvas>();
		buttons = new List<GameObject> ();
		foreach (Transform child in this.transform) {
			buttons.Add(child.gameObject);
		}
		foreach (GameObject b in buttons) {
			b.SetActive(false);
		}
		isOpen = false;
	}

	void Update(){
	/*	if (isOpen && !this.gameObject.activeSelf)
			this.gameObject.);
		if (!isOpen && this.gameObject.activeSelf)
			this.gameObject.SetActive (false);*/
	}

	public static void OpenTeleporterPanel(){
		//canvas[0].enabled = true;
		foreach (GameObject b in buttons) {
			b.SetActive(true);
		}
		isOpen = true;
	}

	public static void CloseTeleporterPanel(){
		//canvas[0].enabled = false;
		foreach (GameObject b in buttons) {
			b.SetActive(false);
		}
		isOpen = false;
	}

	public void goToCastle(){
		if (string.Compare(g.currLevelName,"level3") == 0)
			return;
		if (!PlayerItems.playerTeleporters[Teleporter.TCastle]) //si el jugador no tiene este transportador no puede ir a esa zona
			return;
		g.previousExit = 7; //por convencion la salida 7 es para entrar en algun otro transportador
		g.jumpOnLoad = false;
		/*foreach (GameObject b in buttons) {
			b.SetActive(false);
		}
		isOpen = false;*/
		Fading.BeginFadeIn("level3");
		Debug.Log ("Castle");
	}

	public void goToZone1(){
		if (string.Compare(g.currLevelName,"level1") == 0)
			return;
		if (!PlayerItems.playerTeleporters[Teleporter.TZone1]) //si el jugador no tiene este transportador no puede ir a esa zona
			return;
		g.previousExit = 7;
		g.jumpOnLoad = false;
		/*foreach (GameObject b in buttons) {
			b.SetActive(false);
		}
		isOpen = false;*/
		Fading.BeginFadeIn("level1");
		//Debug.Log ("Zone1");
	}
}