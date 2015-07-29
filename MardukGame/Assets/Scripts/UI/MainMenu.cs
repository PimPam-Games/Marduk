﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public Image playMenuImage;

	void Awake(){
		/*foreach (GameObject o in Object.FindObjectsOfType<GameObject>()) {
			if(!o.activeSelf){
				Destroy(o);
			}
		}*/
	}

	public void NewGame(){

		playMenuImage.gameObject.SetActive(true);
		this.gameObject.SetActive(false);
		//Application.LoadLevel ("level0");
	}

	public void ExitGame(){
		Application.Quit ();
	}
}
