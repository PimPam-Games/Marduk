using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameController : MonoBehaviour {

	public static int CantLevels = 11;

	public static int previousExit = 1; // si la salida es 1, tiene que entrar por la entrada 1
	public GameObject player;
	private GameObject hudCanvas, mainCamera, gui;
	//public List<string> deadEnemies;
	private PlayerStats playerStats;
	public static int currentLevel = 0;
	private CameraController cameraController;
	public static List<string> notVisitedLevels;
	public static List<string[]> levelConnections;
	//public static bool newLevel = true;
	public static Object[] enemyList;
	public static Dictionary<string,List<GameObject>> enemiesPerLevel = new Dictionary<string,List<GameObject>>();
	public static Dictionary<string,List<GameObject>> chunksPerZone = new Dictionary<string,List<GameObject>> ();
	public static string currLevelName;

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
		music1.Play ();

	}
	// Use this for initialization
	void Start () {
		cameraController = mainCamera.GetComponent<CameraController> ();
	}
	
	// Update is called once per frame
	void Update () {

		if (currentLevel != Application.loadedLevel) {
			/*GameObject[] enemies;
			enemies = GameObject.FindGameObjectsWithTag("Enemy");
			foreach(string deadEnemy in deadEnemies){ //Destruye enemigos muertos de esta scena
				foreach(GameObject enemy in enemies){
					if(enemy.name == deadEnemy)
						Destroy(enemy);
				}
			}*/
			RepositionPlayerAndCamera();
			currentLevel = Application.loadedLevel;

		}
		if (playerStats.readyToRespawn) {
			DestroyEnemies();
			ChangeLevel.DestroyItems();
			enemiesPerLevel.Clear ();
			currLevelName = "level1";
			Application.LoadLevel(currLevelName);
			ChunkFactory.Initialize();
			previousExit = 1; //la proxima entrada tiene que ser la 1, la de la izquierda del nivel
			playerStats.RespawnStats();
			player.GetComponent<PlatformerCharacter2D>().RespawnPosition(); //hace que el jugador mire a la derecha
			currentLevel = 0; //cambio al nivel 0 para que se reposicione el jugador entrando por el otro if despues
			playerStats.readyToRespawn = false;
		}
	}

	private void RepositionPlayerAndCamera(){//posiciona al jugador y a la camara en la entrada del nuevo nivel
		BoxCollider2D newBounds = GameObject.FindGameObjectWithTag("CameraBounds").GetComponent<BoxCollider2D>();
		if(newBounds!=null)
			cameraController.SetBounds(newBounds);
		else
			Debug.LogError("CameraBounds not found");
		GameObject levelEntry = GameObject.Find("LevelEntry" + previousExit); 
		player.transform.position = levelEntry.transform.position;
		if(levelEntry.transform.position.x<=0)
			mainCamera.transform.position = new Vector3 (levelEntry.transform.position.x + 4, levelEntry.transform.position.y, mainCamera.transform.position.z);
		else
			mainCamera.transform.position = new Vector3 (levelEntry.transform.position.x - 4, levelEntry.transform.position.y, mainCamera.transform.position.z);
	}

	private void DestroyEnemies(){
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
