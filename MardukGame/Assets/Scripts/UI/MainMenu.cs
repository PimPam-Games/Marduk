using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public static int currentResolution = 0;
	public static int[] ResolutionsWidth = {1280,1360,1366,1600};
	public static int[] ResolutionsHeight = {720,768,768,900};

	public Image playMenuImage;
	public Image optionsMenu;

 
	void Awake(){
	//	bool resolutionOk = false;
		/*resoluciones de pantalla compatibles con el juego*/
		/*if(Screen.currentResolution.width == 1280 &&  Screen.currentResolution.height == 720) 
			resolutionOk = true;
		if(Screen.currentResolution.width == 1360 &&  Screen.currentResolution.height == 768)
			resolutionOk = true;
		if(Screen.currentResolution.width == 1366 &&  Screen.currentResolution.height == 768)
			resolutionOk = true;
		if(Screen.currentResolution.width == 1600 &&  Screen.currentResolution.height == 900)
			resolutionOk = true;*/
	//	Screen.SetResolution(ResolutionsWidth[currentResolution],ResolutionsHeight[currentResolution],true);
		//auxtext.text = "res " + Screen.currentResolution.width.ToString() + " x " + Screen.currentResolution.height.ToString();
	/*	if(!resolutionOk){
			Screen.SetResolution(1280,720,true);
		}*/
		PreferencesData prefs = null;
		prefs = Persistence.LoadPreferences();
		if(prefs != null){
			currentResolution = prefs.resolutionIndex;
			Screen.SetResolution(ResolutionsWidth[prefs.resolutionIndex],ResolutionsHeight[prefs.resolutionIndex],true);
			QualitySettings.SetQualityLevel(prefs.quality);
			Debug.Log("prefs: " + ResolutionsWidth[prefs.resolutionIndex]+" X "+ResolutionsHeight[prefs.resolutionIndex]);
		}
		else{
			currentResolution = 0;
			Screen.SetResolution(ResolutionsWidth[currentResolution],ResolutionsHeight[currentResolution],true);
			QualitySettings.SetQualityLevel(0);
		}
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
