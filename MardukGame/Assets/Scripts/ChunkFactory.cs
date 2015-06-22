using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChunkFactory : MonoBehaviour {

	public int zone = 1;
	private static Object[] chunkPool;
	private static List<Object> commonChunks = new List<Object>();
	private static List<Object> castleChunks = new List<Object>();
	private static bool isEntry = false;

	// Use this for initialization
	void Awake () {
		chunkPool = Resources.LoadAll("Level/ChunksZone" + zone, typeof(Object));
		for (int i = 0; i< chunkPool.Length; i++) {
			if(chunkPool[i].name.Contains("Castle") && !chunkPool[i].name.Contains("Entry")){
				castleChunks.Add(chunkPool[i]);
			}
			else{
				commonChunks.Add(chunkPool[i]);
			}
		}

	}
	
	public static void GenerateChunk(Vector3 pos, Quaternion rot){
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
	}
}
