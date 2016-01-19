using UnityEngine;
using System.Collections;
using g = GameController;

public class LoadCurrentPlayer : MonoBehaviour {

	private float loadCount;
	public static bool showIntro = false; // esta variable se cambia en persistencia
//	private InventorySlotsPanel inv;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		loadCount += Time.deltaTime;
		if (loadCount > 0.2f && !g.levelLoaded && g.nameToLoad.Length > 0 ){
			Persistence.Load (g.nameToLoad);
			g.levelLoaded = true;
			if(showIntro){ 
				g.introText.SetActive(true);
				InputControllerGui.closeInventory = true;
			}
			else
				IntroText.introVisible = false;
			Debug.Log ("Load Data");
		}
	}
}
