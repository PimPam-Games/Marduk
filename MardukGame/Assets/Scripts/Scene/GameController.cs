using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public static int CantLevels = 11;
	public static string nameToLoad = ""; //nombre del personaje a cargar, null si es un nuevo personaje
	public static int previousExit = 0; // si la salida es 1, tiene que entrar por la entrada 1
	public GameObject player;
	private GameObject hudCanvas, mainCamera, gui;
	//public List<string> deadEnemies;
	public PlayerStats playerStats;
	public static int currentLevel = 0;
	private CameraController cameraController;
	public static List<string> notVisitedLevels;
	public static List<string[]> levelConnections;
	//public static bool newLevel = true;
	public static Object[] enemyList;
	public static Dictionary<string,List<GameObject>> enemiesPerLevel = new Dictionary<string,List<GameObject>>();
	public static Dictionary<string,List<GameObject>> chunksPerZone = new Dictionary<string,List<GameObject>> ();
	public static string currLevelName;
	public static bool levelLoaded = false;
	public static bool jumpOnLoad = false; //hacer que el player salte cuando inicie el nivel

	public AudioSource music1;
	void Awake(){
		player = (GameObject)Instantiate (player, this.transform.position,this.transform.rotation);
		//deadEnemies = new List<string>();

		DontDestroyOnLoad (this);
		DontDestroyOnLoad (player);
		playerStats = player.GetComponent<PlayerStats> ();
		hudCanvas = GameObject.Find("HUDCanvas");
		gui = GameObject.Find("GUI");
		mainCamera = GameObject.Find("MainCamera");
		cameraController = mainCamera.GetComponent<CameraController> ();	
		DontDestroyOnLoad (mainCamera);
		DontDestroyOnLoad (gui);
		if (hudCanvas != null)
			DontDestroyOnLoad(hudCanvas);
		else
			Debug.LogError ("HUDCanvas not found");
		currLevelName = "level1";
		Application.LoadLevel (currLevelName);
		currentLevel = Application.loadedLevel;
		Cursor.visible = false;
		notVisitedLevels = new List<string>();
		levelConnections = new List<string[]>();
		enemyList = Resources.LoadAll("Enemies/Zone1", typeof(Object));
		for (int i = 2; i <= CantLevels; i++) {
			notVisitedLevels.Add("level" + i);
		}
		//Debug.Log ("voy a cargar " + nameToLoad);
		music1.Play ();
	}

	void OnApplicationQuit(){
		Persistence.Save ();
	}

	public void stopMusic(){
		music1.Stop ();
	}
	
	// Update is called once per frame
	void Update () {

		//if (currentLevel != Application.loadedLevel) {
			/*GameObject[] enemies;
			enemies = GameObject.FindGameObjectsWithTag("Enemy");
			foreach(string deadEnemy in deadEnemies){ //Destruye enemigos muertos de esta scena
				foreach(GameObject enemy in enemies){
					if(enemy.name == deadEnemy)
						Destroy(enemy);
				}
			}*/
		//	RepositionPlayerAndCamera();
		//	currentLevel = Application.loadedLevel;

		//}
		/*if (playerStats.readyToRespawn) {
			//DestroyEnemies();

			//enemiesPerLevel.Clear ();
			//currLevelName = "level1";
			//Application.LoadLevel(currLevelName);
			Fading.BeginFadeIn("level1");
			currentLevel = 0;
			ChunkFactory.Initialize();
			previousExit = 1; //la proxima entrada tiene que ser la 1, la de la izquierda del nivel
			playerStats.RespawnStats();
			player.GetComponent<PlatformerCharacter2D>().RespawnPosition(); //hace que el jugador mire a la derecha
			currentLevel = 0; //cambio al nivel 0 para que se reposicione el jugador entrando por el otro if despues
			playerStats.readyToRespawn = false;
		}*/
	}



	public void RepositionPlayerAndCamera(){//posiciona al jugador y a la camara en la entrada del nuevo nivel
		GameObject camerab = GameObject.FindGameObjectWithTag ("CameraBounds");
		BoxCollider2D newBounds = null;
		if(camerab != null)
			 newBounds = camerab.GetComponent<BoxCollider2D>();
		if (newBounds != null)
			cameraController.SetBounds (newBounds);
		else {
			//Debug.LogError ("CameraBounds not found");
			return;
		}
		GameObject levelEntry = GameObject.Find("LevelEntry" + previousExit); 
		PlatformerCharacter2D.stopPlayer = true;
		player.transform.position = levelEntry.transform.position;
		if (jumpOnLoad)
			PlatformerCharacter2D.jumpNow = true;
		if(levelEntry.transform.position.x<=0)
			mainCamera.transform.position = new Vector3 (levelEntry.transform.position.x + 4, levelEntry.transform.position.y, mainCamera.transform.position.z);
		else
			mainCamera.transform.position = new Vector3 (levelEntry.transform.position.x - 4, levelEntry.transform.position.y, mainCamera.transform.position.z);
	}

	public void DestroyEnemies(){
		//GameObject[] enems = enemiesPerLevel.Values;
		Dictionary<string, List<GameObject>>.ValueCollection values = enemiesPerLevel.Values;
		foreach (List<GameObject> enems in values)
		{
			foreach(GameObject e in enems){
				Destroy(e.gameObject);
			}
		}
		//GameObject[] enems = GameObject.FindGameObjectsWithTag("Enemy");
		//foreach (GameObject e in enems)
		//	Destroy (e.gameObject);
		enemiesPerLevel.Clear ();

	}

	public static void DestroyAllChunks(){
		Dictionary<string, List<GameObject>>.ValueCollection values = chunksPerZone.Values;
		foreach (List<GameObject> chunks in values)
		{
			foreach(GameObject e in chunks){
				Destroy(e.gameObject);
			}
		}
		chunksPerZone.Clear ();
	}

	public static void SetActiveEnemies(string levelName, bool active){
		//Debug.Log ("Busco la key: " + levelName);
		if (!enemiesPerLevel.ContainsKey (levelName)) {
			//Debug.LogError(levelName + " No encontrado!");
			return;
		}
		List<GameObject> enemList = enemiesPerLevel [levelName];
		foreach(GameObject e in enemList){
			if(e!=null)
				e.SetActive(active);
			//else
			//	enemList.Remove(e);
		}
	}

	public static void SetActiveChunks(string levelName, bool active){
		if (!chunksPerZone.ContainsKey (levelName)) {
			Debug.LogError(levelName + " No encontrado!");
			return;
		}
		List<GameObject> chunkList = chunksPerZone [levelName];
		foreach(GameObject c in chunkList){
			if(c!=null)
				c.SetActive(active);
			//else
			//	enemList.Remove(e);
		}
	}
}
