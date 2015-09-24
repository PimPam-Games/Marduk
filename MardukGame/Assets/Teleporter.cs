﻿using UnityEngine;
using System.Collections;
using t = TeleporterPanel;

public class Teleporter : MonoBehaviour {


	public const int TZone1 = 0 , TCastle = 1, TSublevel = 2; 
	public int id;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Player" && !t.isOpen) {
			PlayerItems.playerTeleporters[id] = true;
			//Debug.Log("enter portal");
			t.OpenTeleporterPanel();
		}
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.tag == "Player" && !t.isOpen) {

			PlayerItems.playerTeleporters[id] = true;
			//Debug.Log("enter portal");
			t.OpenTeleporterPanel();
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.tag == "Player" && t.isOpen) {
			t.CloseTeleporterPanel();
		}
	}
}
