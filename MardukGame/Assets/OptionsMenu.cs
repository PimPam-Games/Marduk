using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {


	public Image mainMenu;
	public GameObject mainOptions;
	public GameObject controlsMenu;
	public GameObject videoMenu;	

	public Text resolutionText;
	public Text QualityText;

//	private bool escPressed = false;
	private bool backToMain = true;

	private int scrResOption;		
	private int qualityOption;	


	void OnEnable() {
		mainOptions.SetActive(true);
		videoMenu.SetActive(false);
		controlsMenu.gameObject.SetActive(false);
		scrResOption = MainMenu.currentResolution;
		qualityOption = QualitySettings.GetQualityLevel();
		resolutionText.text = MainMenu.ResolutionsWidth[scrResOption].ToString() + " X " + MainMenu.ResolutionsHeight[scrResOption].ToString();
		QualityText.text = qualityOption.ToString();
//		escPressed = false;
		backToMain = true;
	}

	public void Update(){
		if (Input.GetButtonUp ("Escape")){
			//escPressed = true;
			StartCoroutine(ButtonPush(3));
		}
	}

	public void Video(){
		StartCoroutine(ButtonPush(1));
	}
	
	public void Controls(){
		StartCoroutine(ButtonPush(2));
	}

	public void UpResolution(){
		if(scrResOption < MainMenu.ResolutionsWidth.Length-1){
			scrResOption++; 
			resolutionText.text = MainMenu.ResolutionsWidth[scrResOption].ToString() + " X " + MainMenu.ResolutionsHeight[scrResOption].ToString();
		}
	}

	public void DownResolution(){
		if(scrResOption > 0){
			scrResOption--; 
			resolutionText.text = MainMenu.ResolutionsWidth[scrResOption].ToString() + " X " + MainMenu.ResolutionsHeight[scrResOption].ToString();
		}
	}

	public void UpQuality(){
		if(qualityOption < 5){
			qualityOption++;
			QualityText.text = qualityOption.ToString();
		}
	}
	
	public void DownQuality(){
		if(qualityOption > 0){
			qualityOption--;
			QualityText.text = qualityOption.ToString();
		}
	}

	public void Apply(){
		MainMenu.currentResolution = scrResOption;
		Screen.SetResolution(MainMenu.ResolutionsWidth[scrResOption], MainMenu.ResolutionsHeight[scrResOption],true);
		QualitySettings.SetQualityLevel(qualityOption);
		Persistence.SavePreferences(scrResOption,qualityOption);
	}

	public void Back(){
		StartCoroutine(ButtonPush(3));
	}	

	IEnumerator ButtonPush(int btnId){
		yield return new WaitForSeconds (0.3f);
		if(btnId == 1){ //habilitar opciones de video
			videoMenu.gameObject.SetActive(true);
			mainOptions.gameObject.SetActive(false);
			backToMain = false;
		}
		if(btnId == 2){ // habilitar controles
			controlsMenu.gameObject.SetActive(true);
			mainOptions.gameObject.SetActive(false);
			backToMain = false;
		}
		if(btnId == 3){ //back
			if(backToMain){
				mainOptions.gameObject.SetActive(true);
				mainMenu.gameObject.SetActive(true);
				this.gameObject.SetActive(false);
			}
			else{
				controlsMenu.gameObject.SetActive(false);
				videoMenu.gameObject.SetActive(false);
				mainOptions.gameObject.SetActive(true);
				backToMain = true;
			}
		}
	}
}
