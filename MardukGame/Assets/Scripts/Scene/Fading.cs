using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using g = GameController;

public class Fading : MonoBehaviour {

	public static Image fadeImage;
	private static Animator anim;
	private static string sceneToLoad;
	public GameObject gameCtrlObj;
	private GameController gameCtrl;

	void Awake(){
		anim = GetComponent<Animator> ();
		fadeImage = GetComponent<Image> ();
		gameCtrl = gameCtrlObj.GetComponent<GameController> ();
	}

	public static void BeginFadeIn (string newScene)
	{
		PlatformerCharacter2D.stopPlayer = true;
		sceneToLoad = newScene;
		anim.SetBool ("FadeOut",false);
		anim.SetBool ("FadeIn",true);
	}

	public static void BeginFadeOut ()
	{
		PlatformerCharacter2D.stopPlayer = false;
		CameraController.stopFollow = false;
		anim.SetBool ("FadeOut",true);
		anim.SetBool ("FadeIn",false);
	}

	public void LoadScene(){
		DestroyItems();
		g.SetActiveEnemies(g.currLevelName,false);
		g.SetActiveChunks(g.currLevelName,false);
		g.currLevelName = sceneToLoad;
		Application.LoadLevel (sceneToLoad);



	}

	void OnLevelWasLoaded (int level) {
		Debug.Log ("level cargado;: " + level);
		if(g.chunksPerZone.ContainsKey(g.currLevelName))
			g.SetActiveChunks(g.currLevelName,true);
		if(g.enemiesPerLevel.ContainsKey(g.currLevelName))
			g.SetActiveEnemies(g.currLevelName,true);
		if (gameCtrl.playerStats.readyToRespawn) {
			gameCtrl.playerStats.RespawnStats ();
			gameCtrl.player.GetComponent<PlatformerCharacter2D> ().RespawnPosition (); //hace que el jugador mire a la derecha
			//gameCtrl.currentLevel = 0; //cambio al nivel 0 para que se reposicione el jugador entrando por el otro if despues
			gameCtrl.playerStats.readyToRespawn = false;
		}
		gameCtrl.RepositionPlayerAndCamera();
		Fading.BeginFadeOut ();
	}

	public static void DestroyItems(){
		GameObject[] items = GameObject.FindGameObjectsWithTag ("Item");
		foreach(GameObject item in items ){
			if(item.activeSelf)
				Destroy(item.gameObject);
		}
	}

}