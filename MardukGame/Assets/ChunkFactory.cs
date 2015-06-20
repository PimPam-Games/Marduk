using UnityEngine;
using System.Collections;

public class ChunkFactory : MonoBehaviour {

	public int zone = 1;
	private static Object[] chunkPool;
	// Use this for initialization
	void Awake () {
		chunkPool = Resources.LoadAll("Level/ChunksZone" + zone, typeof(Object));
	}
	
	public static void GenerateChunk(Vector3 pos, Quaternion rot){
		int r = Random.Range (0,chunkPool.Length);
		Instantiate(chunkPool[r],pos,rot);
	}
}
