using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour {

	public Image playMenuImage;

	void Awake(){
	}
	
	public void NewGame(){
		StartCoroutine(ButtonPush(1));
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
		if(btnId == 3){
			Application.Quit ();
		}
	}
}
