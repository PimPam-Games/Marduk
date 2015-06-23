using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChunkFactory : MonoBehaviour {

	public int zone = 1;
	public string bgName = "Mountain";

	private static Object[] chunkPool;
	private static List<Object> commonChunks = new List<Object>();
	private static List<Object> castleChunks = new List<Object>();
	private static Object bg;
	private static bool isEntry = false;
	public static float bgCount = 0;
	public static float bgPerChunk = 7; //cada cuantos chunks debe generar el fondo

	// Use this for initialization
	void Awake () {
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
	}

	public static void GenerateChunk(Vector3 pos, Quaternion rot){
		bgCount++;
		if (isEntry) {
			int r = Random.Range (0,castleChunks.Count);
			GameObject newChunk = (GameObject)Instantiate (castleChunks [r], pos, rot);
			if(newChunk.name.Contains("Exit"))
				isEntry = false;
		} else {
			int r = Random.Range (0,commonChunks.Count);
			GameObject newChunk = (GameObject)Instantiate (commonChunks [r], pos, rot);
			if(newChunk.name.Contains("Entry"))
				isEntry = true;
		}
		if (bgCount >= bgPerChunk) {
			Debug.Log("Cree un bg");
			bgCount = 0;
			Instantiate(bg,new Vector3(pos.x + 35,pos.y - 2,35),rot);
		}
	}
}
