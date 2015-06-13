using UnityEngine;
using System.Collections;
using g = GameController;
using System.Collections.Generic;

public class EnemyRandomGenerator : MonoBehaviour {

	public List<Transform> enemyPositions;
	private List<GameObject> enemies = new List<GameObject> ();

	// Use this for initialization
	void Start () {
		if (!g.enemiesPerLevel.ContainsKey(g.currLevelName)) {
			GenerateEnemies ();
		} else 
			g.SetActiveEnemies (g.currLevelName,true);

	}

	private void GenerateEnemies(){
		foreach(Transform enemyPos in enemyPositions){
			int index = Random.Range(0,g.enemyList.Length); //slecciona un enemigo aleatorio de la lista de enemigos
			GameObject newEnemy = (GameObject)Instantiate (g.enemyList[index],enemyPos.position,enemyPos.rotation);
			DontDestroyOnLoad(newEnemy);
			enemies.Add(newEnemy);
		}
		//Debug.Log ("meto la key: " + g.currLevelName);
		g.enemiesPerLevel.Add (g.currLevelName, enemies);
	}
}
