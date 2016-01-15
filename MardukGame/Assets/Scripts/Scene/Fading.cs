using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using g = GameController;

public class Fading : MonoBehaviour {

	public static Image fadeImage;
	private static Animator anim;
	private static string sceneToLoad;
	public static bool loaded = false; // se usa en el optimizador para saber cuando se termino de cargar un nibvel
	public GameObject gameCtrlObj;
	private GameController gameCtrl;
	//private ChunkFactory currentChunkFactory;


	void Awake(){
		anim = GetComponent<Animator> ();
		fadeImage = GetComponent<Image> ();
		gameCtrl = gameCtrlObj.GetComponent<GameController> ();
	}

	public static void BeginFadeIn (string newScene)
	{	loaded = false;
		PlatformerCharacter2D.stopPlayer = true;
		sceneToLoad = newScene;
		anim.SetBool ("FadeOut",false);
		anim.SetBool ("FadeIn",true);
	}

	public static void BeginFadeOut ()
	{
		if (sceneToLoad == "level1") { //hay una pantalla negra al principio que se desactiva al cargar el primer nivel
			GameObject bs = GameObject.Find ("blackScreen"); //es para que no se vea nada hasta que se genere todo el lvl
			if(bs != null)
				bs.SetActive(false); 
		}
		PlatformerCharacter2D.stopPlayer = false;
		CameraController.stopFollow = false;
		loaded = true;
		anim.SetBool ("FadeOut",true);
		anim.SetBool ("FadeIn",false);
	}

	public void LoadScene(){
		DestroyItems();
		g.SetActiveEnemies(g.currLevelName,false);
		g.SetActiveChunks(g.currLevelName,false);
		g.currLevelName = sceneToLoad;
		//ChunkFactory.levelGenerated = false;
		Application.LoadLevel (sceneToLoad);
	}

	public void LevelLoaded(){
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

	void OnLevelWasLoaded (int level) {
		//Debug.Log ("level cargado;: " + level);
	}

	public static void DestroyItems(){
		GameObject[] items = GameObject.FindGameObjectsWithTag ("Item");
		foreach(GameObject item in items ){
			if(item.activeSelf)
				Destroy(item.gameObject);
		}
	}

}