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

	private bool escPressed = false;
	private bool backToMain = true;

	void OnEnable() {
		mainOptions.SetActive(true);
		videoMenu.SetActive(false);
		controlsMenu.gameObject.SetActive(false);
		resolutionText.text = MainMenu.ResolutionsWidth[MainMenu.currentResolution].ToString() + " X " + MainMenu.ResolutionsHeight[MainMenu.currentResolution].ToString();
		QualityText.text = QualitySettings.GetQualityLevel().ToString();
		escPressed = false;
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
		if(MainMenu.currentResolution < MainMenu.ResolutionsWidth.Length-1){
			MainMenu.currentResolution++;
			Screen.SetResolution(MainMenu.ResolutionsWidth[MainMenu.currentResolution],MainMenu.ResolutionsHeight[MainMenu.currentResolution],true);
			resolutionText.text = MainMenu.ResolutionsWidth[MainMenu.currentResolution].ToString() + " X " + MainMenu.ResolutionsHeight[MainMenu.currentResolution].ToString();
		}
	}

	public void DownResolution(){
		if(MainMenu.currentResolution > 0){
			MainMenu.currentResolution--;
			Screen.SetResolution(MainMenu.ResolutionsWidth[MainMenu.currentResolution],MainMenu.ResolutionsHeight[MainMenu.currentResolution],true);
			resolutionText.text = MainMenu.ResolutionsWidth[MainMenu.currentResolution].ToString() + " X " + MainMenu.ResolutionsHeight[MainMenu.currentResolution].ToString();
		}
	}

	public void UpQuality(){
		if(QualitySettings.GetQualityLevel() < 5){
			QualitySettings.SetQualityLevel(QualitySettings.GetQualityLevel()+1);
			QualityText.text = QualitySettings.GetQualityLevel().ToString();
		}
	}
	
	public void DownQuality(){
		if(QualitySettings.GetQualityLevel() > 0){
			QualitySettings.SetQualityLevel(QualitySettings.GetQualityLevel()-1);
			QualityText.text = QualitySettings.GetQualityLevel().ToString();
		}
	}

	public void Back(){
		StartCoroutine(ButtonPush(3));
	}	

	IEnumerator ButtonPush(int btnId){
		yield return new WaitForSeconds (0.3f);
		if(btnId == 1){
			videoMenu.gameObject.SetActive(true);
			mainOptions.gameObject.SetActive(false);
			backToMain = false;
		}
		if(btnId == 2){
			controlsMenu.gameObject.SetActive(true);
			mainOptions.gameObject.SetActive(false);
			backToMain = false;
		}
		if(btnId == 3){
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
