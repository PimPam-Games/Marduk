using UnityEngine;
using System.Collections;

public class MenuInGame : MonoBehaviour {

	public GameController g;

	public void SaveAndQuit(){
		Persistence.Save ();
		g.stopMusic ();
		//Debug.Log (PlatformerCharacter2D.playerItemsGO.Count);
		foreach (GameObject it in PlatformerCharacter2D.playerItemsGO) {
			Destroy(it);
		}
		foreach (GameObject o in Object.FindObjectsOfType<GameObject>()) {
			Destroy(o);
		}

		this.gameObject.SetActive (false);
		Time.timeScale = 1.0f;
		Application.LoadLevel("MainMenu");

	}
}
