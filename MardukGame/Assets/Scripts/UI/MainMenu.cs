using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public Image playMenuImage;
	public Image optionsMenu;

 
	void Awake(){
		//Debug.Log( Screen.currentResolution.);
		bool resolutionOk = false;
		/*resoluciones de pantalla compatibles con el juego*/
		/*if(Screen.currentResolution.width == 1280 &&  Screen.currentResolution.height == 720) 
			resolutionOk = true;
		if(Screen.currentResolution.width == 1360 &&  Screen.currentResolution.height == 768)
			resolutionOk = true;
		if(Screen.currentResolution.width == 1366 &&  Screen.currentResolution.height == 768)
			resolutionOk = true;
		if(Screen.currentResolution.width == 1600 &&  Screen.currentResolution.height == 900)
			resolutionOk = true;*/
		Screen.SetResolution(1600,900,true);
		//auxtext.text = "res " + Screen.currentResolution.width.ToString() + " x " + Screen.currentResolution.height.ToString();
	/*	if(!resolutionOk){
			Screen.SetResolution(1280,720,true);
		}*/
		
		//QualitySettings.SetQualityLevel(5); //por defecto se setea la calidad de grafricos mas alta
	}
	
	public void NewGame(){
		StartCoroutine(ButtonPush(1));
	}

	public void Options(){
		StartCoroutine(ButtonPush(2));
	}

	public void ExitGame(){
		StartCoroutine(ButtonPush(3));
	}

	IEnumerator ButtonPush(int btnId){
		yield return new WaitForSeconds (0.3f);
		if(btnId == 1){
			playMenuImage.gameObject.SetActive(true);
			this.gameObject.SetActive(false);
		}
		if(btnId == 2){
			optionsMenu.gameObject.SetActive(true);
			this.gameObject.SetActive(false);
		}
		if(btnId == 3){
			Application.Quit ();
		}
	}
}
