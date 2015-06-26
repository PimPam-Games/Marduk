using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using g = GameController;

public class Chunk : MonoBehaviour {

	//public GameObject[] chunkPool;
	public List<Transform> enemies;
	public Transform chunkEndRight; // chunkEndLeft es la posicion del chunk
	public Transform chunkEndUp;
	public Transform chunkEndDown;
	public bool hasLeftEnd, hasRightEnd, hasUpEnd, hasDownEnd; //que salidas tiene el chunk
	public bool leftUsed, rightUsed, upUsed, downUsed; //salidas usadas
	public bool isFirst = false;
	private bool alreadyGenerated = false;
	private float collisionDetectCount;

	private ChunkFactory cf;
	// Use this for initialization

	void Awake(){
		cf = GameObject.Find ("LevelController").GetComponent<ChunkFactory>();
		collisionDetectCount = 1;
	}

	void Update(){
		collisionDetectCount -= Time.deltaTime;
	}

	void Start () {
		if (g.chunksPerZone.ContainsKey(g.currLevelName)) {
			if(g.chunksPerZone[g.currLevelName].Count == 0) //si esta en la lista pero no hay ningun chunk, crea el primero
				GenerateChunks();
		}
		else
			GenerateChunks(); //si  no esta en la lista  crea el primero
		foreach(Transform enemyPos in enemies){
			int index = Random.Range(0,g.enemyList.Length); //slecciona un enemigo aleatorio de la lista de enemigos
			GameObject newEnemy = (GameObject)Instantiate (g.enemyList[index],enemyPos.position,enemyPos.rotation);
			DontDestroyOnLoad(newEnemy);
			g.enemiesPerLevel[g.currLevelName].Add(newEnemy);
		}
		//Debug.Log ("meto la key: " + g.currLevelName);
		//g.enemiesPerLevel.Add (g.currLevelName, enems);		
	}

	void GenerateChunks(){
		if (alreadyGenerated)
			return;
		alreadyGenerated = true;
		if (hasRightEnd && !rightUsed) {
			GameObject g = cf.GenerateChunk (chunkEndRight.position, chunkEndRight.rotation,ChunkFactory.Exits.Left);
			if(g!=null)
				g.GetComponent<Chunk>().leftUsed = true;
		}
		if (hasLeftEnd && !leftUsed ) {
			GameObject g = cf.GenerateChunk(transform.position,transform.rotation,ChunkFactory.Exits.Right);
			if(g != null){
				g.GetComponent<Chunk>().rightUsed = true; //el nuevo chunk no tinene que generar por la derecha por que ya esta usada por el chunk que lo acaba de crear
				g.transform.position = new Vector3( g.transform.position.x -(g.transform.FindChild("ChunkEnd").position.x - g.transform.position.x),g.transform.position.y,g.transform.position.z);
			}
		}
		if (hasUpEnd) {
			GameObject g = cf.GenerateChunk(transform.position,transform.rotation,ChunkFactory.Exits.Down);
			if(g != null){
				g.GetComponent<Chunk>().downUsed = true;
				g.transform.position = new Vector3( g.transform.position.x,g.transform.position.y + (g.transform.FindChild("ChunkEndUp").position.y - g.transform.FindChild("ChunkEndDown").position.y),g.transform.position.z);
			}
		}
		if (hasDownEnd) {
			GameObject g = cf.GenerateChunk(transform.position,transform.rotation,ChunkFactory.Exits.Up);
			if(g != null){
				g.GetComponent<Chunk>().upUsed = true;
				g.transform.position = new Vector3( g.transform.position.x,g.transform.position.y - (g.transform.FindChild("ChunkEndUp").position.y - g.transform.FindChild("ChunkEndDown").position.y),g.transform.position.z);
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (collisionDetectCount > 0)
			return;
		if (col.gameObject.tag == "Player" && !alreadyGenerated) {
			Debug.Log("Genero");
			GenerateChunks();
		}
	}
}
