﻿using UnityEngine;
using System.Collections;
using g = GameController;

public class LevelSettings : MonoBehaviour {

	public int enemiesLvl = 1;
	public int zoneNumber = 1;
	public GameObject[] zoneEnemies;
	public GameObject miniBoss;
//	private bool minibossGenerated = false;
	// Use this for initialization
	void Start () {
		ZoneName.ShowZoneName(zoneNumber);
	}

	public void generateEnemy(Vector3 pos, Quaternion rot){
		int index = Random.Range(0,zoneEnemies.Length); //slecciona un enemigo aleatorio de la lista de enemigos
		if(index >= zoneEnemies.Length)
			Debug.LogError("Arreglo de enemigos fuera de rango");
		GameObject newEnemy = (GameObject)Instantiate (zoneEnemies[index],pos,rot);
		if (zoneNumber == 1 || zoneNumber == 4 || zoneNumber == 9) {
			if (string.Compare (newEnemy.GetComponentInChildren<EnemyStats> ().enemyName, "Hell mouth") == 0) { //al lado del treefather genero unos stumps

				GameObject newEnemy1 = (GameObject)Instantiate (zoneEnemies [0], new Vector3 (pos.x + 5, pos.y, pos.z), rot);
				GameObject newEnemy2 = (GameObject)Instantiate (zoneEnemies [0], new Vector3 (pos.x - 5, pos.y, pos.z), rot);
				DontDestroyOnLoad (newEnemy1);
				g.enemiesPerLevel [g.currLevelName].Add (newEnemy1);
				DontDestroyOnLoad (newEnemy2);
				g.enemiesPerLevel [g.currLevelName].Add (newEnemy2);
			}
		}
		if (zoneNumber == 1 || zoneNumber == 7 || zoneNumber == 9 | zoneNumber == 8) {
			if (string.Compare (newEnemy.GetComponentInChildren<EnemyStats> ().enemyName, "Roc") == 0) { //si es un crow genero 2
				int cant = Random.Range(1,3);
				for(int i = 1; i <= cant;i++){
					GameObject newEnemy1;
					if(i == 2){
						newEnemy1 = (GameObject)Instantiate (zoneEnemies [3], new Vector3 (pos.x - 3, pos.y + 1, pos.z), rot);
					}
					else
						newEnemy1 = (GameObject)Instantiate (zoneEnemies [3], new Vector3 (pos.x + 3, pos.y - 1, pos.z), rot);
					//GameObject newEnemy2 = (GameObject)Instantiate (zoneEnemies [0], new Vector3 (pos.x - 2, pos.y, pos.z), rot);
					DontDestroyOnLoad (newEnemy1);
					g.enemiesPerLevel [g.currLevelName].Add (newEnemy1);
				}
				//DontDestroyOnLoad (newEnemy2);
				//g.enemiesPerLevel [g.currLevelName].Add (newEnemy2);
			}
		}
		DontDestroyOnLoad(newEnemy);

		//Champion Affixes
		float randomPoint = Random.value;
		if(randomPoint < 0.1f){ //10% chance of being champion
			int randomAffix = Random.Range(1,EnemyStats.cantEnemyAffixes);
			switch (randomAffix) {
			case 1 : newEnemy.GetComponentInChildren<EnemyStats>().isArmored = true; break;
			default : break;
			}
		}

		g.enemiesPerLevel[g.currLevelName].Add(newEnemy);
	}
	
	public void GenerateMiniBoss(Vector3 pos, Quaternion rot){
		//float[] prob = {1 - miniBossProb, miniBossProb };
		//if(Utils.Choose (prob) == 0 || minibossGenerated) //no toco el boss o ya esta generado
		//	return;
//		minibossGenerated = true;
		//if(zoneNumber == 2){
			Debug.Log("MINIBOSSGENERADO");
			GameObject newEnemy = (GameObject)Instantiate (miniBoss,pos,rot);
			DontDestroyOnLoad(newEnemy);
			g.enemiesPerLevel[g.currLevelName].Add(newEnemy);
		//}
	}

}
