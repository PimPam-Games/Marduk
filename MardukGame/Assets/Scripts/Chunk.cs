using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using g = GameController;

public class Chunk : MonoBehaviour {

	//public GameObject[] chunkPool;
	public List<Transform> enemies;
	public Transform chunkEnd;
	public bool isFirst = false;
	private bool alreadyGenerated = false;
	private ChunkFactory cf;
	// Use this for initialization

	void Awake(){
		cf = GameObject.Find ("LevelController").GetComponent<ChunkFactory>();
	}

	void Start () {
		if (g.chunksPerZone.ContainsKey(g.currLevelName)) {
			if(g.chunksPerZone[g.currLevelName].Count == 0) //si esta en la lista pero no hay ningun chunk, crea el primero
				cf.GenerateChunk(chunkEnd.position,chunkEnd.rotation);
		}
		else
			cf.GenerateChunk(chunkEnd.position,chunkEnd.rotation); //si  no esta en la lista  crea el primero
		foreach(Transform enemyPos in enemies){
			int index = Random.Range(0,g.enemyList.Length); //slecciona un enemigo aleatorio de la lista de enemigos
			GameObject newEnemy = (GameObject)Instantiate (g.enemyList[index],enemyPos.position,enemyPos.rotation);
			DontDestroyOnLoad(newEnemy);
			g.enemiesPerLevel[g.currLevelName].Add(newEnemy);
		}
		//Debug.Log ("meto la key: " + g.currLevelName);
		//g.enemiesPerLevel.Add (g.currLevelName, enems);		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Player" && !alreadyGenerated) {
			alreadyGenerated = true;
			cf.GenerateChunk(chunkEnd.position,chunkEnd.rotation);
		}
	}
}
