using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public int previousExit = 1; // si la salida es 1, tiene que entrar por la entrada 1
	public GameObject player;
	private GameObject hudCanvas, mainCamera, gui;
	public List<string> deadEnemies;
	private PlayerStats playerStats;
	private int currentLevel = 0;
	private CameraController cameraController;

	void Awake(){
		player = (GameObject)Instantiate (player, this.transform.position,this.transform.rotation);
		deadEnemies = new List<string>();
		DontDestroyOnLoad (this);
		DontDestroyOnLoad (player);
		playerStats = player.GetComponent<PlayerStats> ();
		hudCanvas = GameObject.Find("HUDCanvas");
		gui = GameObject.Find("GUI");
		mainCamera = GameObject.Find("MainCamera");
		DontDestroyOnLoad (mainCamera);
		DontDestroyOnLoad (gui);
		if (hudCanvas != null)
			DontDestroyOnLoad(hudCanvas);
		else
			Debug.LogError ("HUDCanvas not found");
		Application.LoadLevel ("Level1");
		currentLevel = Application.loadedLevel;

	}
	// Use this for initialization
	void Start () {
		cameraController = mainCamera.GetComponent<CameraController> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (currentLevel != Application.loadedLevel) {
			GameObject[] enemies;
			enemies = GameObject.FindGameObjectsWithTag("Enemy");
			foreach(string deadEnemy in deadEnemies){ //Destruye enemigos muertos de esta scena
				foreach(GameObject enemy in enemies){
					if(enemy.name == deadEnemy)
						Destroy(enemy);
				}
			}
			RepositionPlayerAndCamera();
			currentLevel = Application.loadedLevel;
		}
		if (playerStats.readyToRespawn) {
			deadEnemies.Clear();
			Application.LoadLevel("Level1");
			previousExit = 1; //la proxima entrada tiene que ser la 1, la de la izquierda del nivel
			playerStats.RespawnStats();
			player.GetComponent<PlatformerCharacter2D>().RespawnPosition(); //hace que el jugador mire a la derecha
			currentLevel = 0; //cambio al nivel 0 para que se reposicione el jugador entrando por el otro if despues
			playerStats.readyToRespawn = false;
		}
	}


	/*IEnumerator PlayerDying() {
		yield return new WaitForSeconds (3);

	}*/

	private void RepositionPlayerAndCamera(){//posiciona al jugador y a la camara en la entrada del nuevo nivel
		BoxCollider2D newBounds = GameObject.FindGameObjectWithTag("CameraBounds").GetComponent<BoxCollider2D>();
		if(newBounds!=null)
			cameraController.SetBounds(newBounds);
		else
			Debug.LogError("CameraBounds not found");
		GameObject levelEntry = GameObject.Find("LevelEntry" + previousExit); 
		mainCamera.transform.position = new Vector3 (levelEntry.transform.position.x, levelEntry.transform.position.y, mainCamera.transform.position.z);
		player.transform.position = levelEntry.transform.position;
	}
}
