using UnityEngine;
using System.Collections;
using g = GameController;
using System.Collections.Generic;

public class NextChunkGenerator : MonoBehaviour {


	public GameObject[] chunkPool;
	public List<Transform> enemies;
	public Transform chunkStart;
	public Transform chunkEnd;
	private bool alreadyGenerated = false;
	// Use this for initialization
	void Start () {
		foreach(Transform enemyPos in enemies){
			int index = Random.Range(0,g.enemyList.Length); //slecciona un enemigo aleatorio de la lista de enemigos
			GameObject newEnemy = (GameObject)Instantiate (g.enemyList[index],enemyPos.position,enemyPos.rotation);
			DontDestroyOnLoad(newEnemy);
			//enemies.Add(newEnemy);
		}
		//Debug.Log ("meto la key: " + g.currLevelName);
		//g.enemiesPerLevel.Add (g.currLevelName, enemies);		
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){
		int r = Random.Range (0,chunkPool.Length);
		if (col.gameObject.tag == "Player" && !alreadyGenerated) {
			alreadyGenerated = true;
			Instantiate(chunkPool[r],chunkEnd.position,chunkEnd.rotation);
			Debug.Log ("Generar Chunk");
		}
	}
}
