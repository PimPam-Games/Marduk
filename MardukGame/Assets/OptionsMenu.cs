using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

	public Image mainMenu;
	public GameObject mainOptions;
	public GameObject controlsMenu;
	public GameObject videoMenu;	
	private bool escPressed = false;
	private bool backToMain = true;

	void OnEnable() {
		mainOptions.SetActive(true);
		videoMenu.SetActive(false);
		controlsMenu.gameObject.SetActive(false);
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
