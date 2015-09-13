using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using g = GameController;

public class ChunkFactory : MonoBehaviour {

	public int zone = 1;
	public int  MatrixSize = 61;
	public int matrixDepth = 10;
	public string bgName = "Mountain";
	public string sceneName = "level1";
	public bool hasCaveEntranceChunk = false; // si tiene un chunk de una entrada a una cueva o dungeon en la ultima posicion del arreglo
	public bool generateBackground;
	public bool[,] cmatrix;
	public bool zoneEntranceGenerated = false; // es para ver si ya se genero un chunk de entrada a otra zona o no
	private int currentChunkId = 0;
	private Object[] chunkPool;
	private  List<Object> commonChunks = new List<Object>();
	public List<Object> doubleChunks = new List<Object>();
	public List<Object> normalChunks = new List<Object>();
	public List<Object> leftEndChunks = new List<Object>();
	public List<Object> rightEndChunks = new List<Object>();
	private  List<Object> castleChunks = new List<Object>();
	private static Object bg;
	private static bool isEntry = false;
	public static float bgCount = 0;
	public static float bgPerChunk = 4; //cada cuantos chunks debe generar el fondo


	// Use this for initialization
	void Awake () {
		cmatrix = new bool[MatrixSize,MatrixSize];
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

	public GameObject GenerateEnd (Vector3 pos, Quaternion rot, Exits exit){
		GameObject newChunk = null;
		if (!g.chunksPerZone.ContainsKey (g.currLevelName)) {
			Debug.LogError(g.currLevelName + " No encontrado!");
			return null;
		}
		int r;
		switch (exit) {
		case Exits.Left:
			r = Random.Range (0,leftEndChunks.Count);
			newChunk = (GameObject)Instantiate (leftEndChunks [r], pos, rot);
			break;
		case Exits.Right:
			r = Random.Range (0,rightEndChunks.Count);
			newChunk = (GameObject)Instantiate (rightEndChunks [r], pos, rot);
			break;
		}
		g.chunksPerZone[g.currLevelName].Add(newChunk); //agrego el chunk a la lista de chunks de este nivel
		DontDestroyOnLoad(newChunk);
		return newChunk;
	}

	public GameObject GenerateChunk(Vector3 pos, Quaternion rot, Exits exit, int[] chunkPos){ //entry: "left", "right" , "up" , "down" , null para cualquiera
		bgCount++;
		GameObject newChunk = null;
		if (!g.chunksPerZone.ContainsKey (g.currLevelName)) {
			Debug.LogError(g.currLevelName + " No encontrado!");
			return null;
		}
		if (isEntry && castleChunks.Count > 0) {
			int r = Random.Range (0,castleChunks.Count);
			newChunk = (GameObject)Instantiate (castleChunks [r], pos, rot);
			if(newChunk.name.Contains("Exit"))
				isEntry = false;
			DontDestroyOnLoad(newChunk);
			g.chunksPerZone[g.currLevelName].Add(newChunk); //agrego el chunk a la lista de chunks de este nivel
		} else {
			int r;
			float[] chunksProb = {0.4f,0.4f,0.2f}; //40% uno normal, 40% un doble, 20% un cierre

			int choice = Utils.Choose(chunksProb);
			if(doubleChunks.Count == 0) // esto es por ahora nomas, para que ande el level 1
				choice = 0;
			if((chunkPos[1] == 1 && exit == Exits.Right) ||(chunkPos[1] == MatrixSize-2 && exit == Exits.Left)){ //si llego a algun borde horizontal hay que poner un final
				choice = 2;
			}
			if(chunkPos[0] >= matrixDepth && choice == 1){ // si llego al limite de la profundidad no puede tocar un doble
				choice = 0;
			}
			if((cmatrix[chunkPos[0]+1, chunkPos[1]-1] && exit == Exits.Right) || (cmatrix[chunkPos[0]+1, chunkPos[1]+1] && exit == Exits.Left)){
				//Debug.Log("este doble no deberia ir baby");
				choice = 0;
			}

			switch(choice){
			case 0:
				currentChunkId++;
				r = Random.Range (0,normalChunks.Count);
				if(hasCaveEntranceChunk && r == normalChunks.Count - 1){ //en la ultima posicion tiene que estar el chunk que entra a una cueva
					if(zoneEntranceGenerated){
						r = Random.Range (0,normalChunks.Count-1);
					}
					else{
						zoneEntranceGenerated = true;
					}
				}
				if(hasCaveEntranceChunk && chunkPos[0] >= matrixDepth && chunkPos[1] == MatrixSize - 3 && !zoneEntranceGenerated){ // si estoy llegando casi al final y no se genero
					zoneEntranceGenerated = true;                                                          // el chunk cueva lo genero si o si
					r = normalChunks.Count - 1;
				}
				newChunk = (GameObject)Instantiate (normalChunks [r], pos, rot);
				newChunk.GetComponent<Chunk>().chunkId = currentChunkId;
				break;
			case 1:
				/*if(exit == Exits.Right)
					cmatrix[pos[0]+2,pos[1]-1] = true;*/
				currentChunkId++;
				r = Random.Range (0,doubleChunks.Count);

				newChunk = (GameObject)Instantiate (doubleChunks [r], pos, rot);
				newChunk.GetComponent<Chunk>().chunkId = currentChunkId;
				break;
			case 2:
				currentChunkId++;
				if(exit == Exits.Right){
					r = Random.Range (0,leftEndChunks.Count);
					newChunk = (GameObject)Instantiate (leftEndChunks [r], pos, rot);
				}
				else{
					r = Random.Range (0,rightEndChunks.Count);
					newChunk = (GameObject)Instantiate (rightEndChunks [r], pos, rot);
				}
				newChunk.GetComponent<Chunk>().chunkId = currentChunkId;
				break;
			}
			//Debug.Log("Id" + currentChunkId);
			if(newChunk.name.Contains("Entry"))
				isEntry = true;
			g.chunksPerZone[g.currLevelName].Add(newChunk); //agrego el chunk a la lista de chunks de este nivel
			DontDestroyOnLoad(newChunk);
		}
		if(generateBackground)
			if (bgCount >= bgPerChunk) {
				bgCount = 0;
				GameObject go = (GameObject)Instantiate(bg,new Vector3(pos.x + 35,pos.y - 4,35),rot);
				DontDestroyOnLoad(go);
				g.chunksPerZone[g.currLevelName].Add(go); //agrego el chunk a la lista de chunks de este nivel
			}
		return newChunk;
	}

	public enum Exits{
		Right,
		Left,
		Up,
		Down
	}
}
