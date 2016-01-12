using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour {

	public Image mainMenu;

	public void Back(){
		StartCoroutine(BackPush());
	}
	
	IEnumerator BackPush(){
		yield return new WaitForSeconds (0.3f); //ahce que espere un poco para que se escuche el sonido
		mainMenu.gameObject.SetActive(true);
		this.gameObject.SetActive(false);
	}
}
