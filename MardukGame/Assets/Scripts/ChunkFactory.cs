using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using g = GameController;

public class ChunkFactory : MonoBehaviour {

	public int zone = 1;
	public string bgName = "Mountain";
	public string sceneName = "level1";

	private static Object[] chunkPool;
	private static List<Object> commonChunks = new List<Object>();
	private static List<Object> castleChunks = new List<Object>();
	private static Object bg;
	private static bool isEntry = false;
	public static float bgCount = 0;
	public static float bgPerChunk = 7; //cada cuantos chunks debe generar el fondo

	// Use this for initialization
	void Awake () {
		if (!g.chunksPerZone.ContainsKey (g.currLevelName)) {
			g.chunksPerZone.Add (g.currLevelName, new List<GameObject> ());
			g.enemiesPerLevel.Add(g.currLevelName, new List<GameObject>());
		}
		chunkPool = Resources.LoadAll("Level/ChunksZone" + zone, typeof(Object));
		bg = Resources.Load ("Level/Background/" + bgName);
		for (int i = 0; i< chunkPool.Length; i++) {
			if(chunkPool[i].name.Contains("Castle") && !chunkPool[i].name.Contains("Entry")){
				castleChunks.Add(chunkPool[i]);
			}
			else{
				commonChunks.Add(chunkPool[i]);
			}
		}

	}

	public static void Initialize(){
		isEntry = false;
		bgCount = 0;
	}

	public void GenerateChunk(Vector3 pos, Quaternion rot){
		bgCount++;
		if (!g.chunksPerZone.ContainsKey (g.currLevelName)) {
			Debug.LogError(g.currLevelName + " No encontrado!");
			return;
		}
		if (isEntry) {
			int r = Random.Range (0,castleChunks.Count);
			GameObject newChunk = (GameObject)Instantiate (castleChunks [r], pos, rot);
			if(newChunk.name.Contains("Exit"))
				isEntry = false;
			DontDestroyOnLoad(newChunk);
			g.chunksPerZone[g.currLevelName].Add(newChunk); //agrego el chunk a la lista de chunks de este nivel
		} else {
			int r = Random.Range (0,commonChunks.Count);
			GameObject newChunk = (GameObject)Instantiate (commonChunks [r], pos, rot);
			if(newChunk.name.Contains("Entry"))
				isEntry = true;
			g.chunksPerZone[g.currLevelName].Add(newChunk); //agrego el chunk a la lista de chunks de este nivel
			DontDestroyOnLoad(newChunk);
		}
		if (bgCount >= bgPerChunk) {
			bgCount = 0;
			GameObject go = (GameObject)Instantiate(bg,new Vector3(pos.x + 35,pos.y - 2,35),rot);
			DontDestroyOnLoad(go);
			g.chunksPerZone[g.currLevelName].Add(go); //agrego el chunk a la lista de chunks de este nivel
		}
	}
}
