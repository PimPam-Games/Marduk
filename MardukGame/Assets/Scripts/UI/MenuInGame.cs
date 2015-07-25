using UnityEngine;
using System.Collections;

public class MenuInGame : MonoBehaviour {

	public GameController g;

	public void SaveAndQuit(){
		Persistence.Save ();
		g.stopMusic ();
		foreach (GameObject o in Object.FindObjectsOfType<GameObject>()) {
			Destroy(o);
		}
		this.gameObject.SetActive (false);
		Time.timeScale = 1.0f;
		Application.LoadLevel("MainMenu");

	}
}
