using UnityEngine;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public void NewGame(){
		Application.LoadLevel ("level0");
	}

	public void ExitGame(){
		Application.Quit ();
	}
}
