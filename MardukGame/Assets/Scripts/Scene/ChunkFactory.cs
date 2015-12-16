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
	public bool bottomUpGeneration = false; // si los chunks dobles del nivel van hacia el piso de arriba
	public bool hasCaveEntranceChunk = false; // si tiene un chunk de una entrada a una cueva o dungeon en la ultima posicion del arreglo
	public bool generateBackground;
	public bool[,] cmatrix;
	public bool zoneEntranceGenerated = false; // es para ver si ya se genero un chunk de entrada a otra zona o no
	private int currentChunkId = 0;
	private Object[] chunkPool;
	private  List<Object> commonChunks = new List<Object>();
	public List<GameObject> doubleChunks = new List<GameObject>();
	public List<Object> normalChunks = new List<Object>();
	public List<Object> leftEndChunks = new List<Object>();
	public List<Object> rightEndChunks = new List<Object>();
	public Object outcastleEntrance = null;
	public Object miniBossChunk = null;

	private  List<Object> castleChunks = new List<Object>();
	private static Object bg;
	public static float bgCount = 0;
	//public static int newChunkPosY; //esta variable se deberia actualziar antes de generar un nuevo chunk, en las clases chunk y chunk2. es para poner la entrada a la cueva
	public static float bgPerChunk = 4; //cada cuantos chunks debe generar el fondo
	public static bool levelGenerated = false; //true si ya se termino de generar todo el lvl
	private int chunksCount = 0; //cuenta los chunks que se van generando si este numero no cambia mas, es por que ya no hay mas chunks generandose
	private bool fadingFuncCalled = false;
	private Fading fading;
	private bool[] doubleInRow; //si hay un doble en cada fila
	private bool outcastleExitGenerated = false;
	private bool minibossChunkGenerated = false;
	// Use this for initialization
	void Awake () {
		cmatrix = new bool[matrixDepth, MatrixSize];
		doubleInRow = new bool[matrixDepth];
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
		fading = GameObject.Find ("HUDCanvas").GetComponentInChildren <Fading>();
		levelGenerated = false;
		StartCoroutine (CheckLevelGeneration ());

	}

	IEnumerator CheckLevelGeneration () {
		int currentChunkCount = 0;
		while (!levelGenerated) {
			yield return new WaitForSeconds (0.2f);
			if(currentChunkCount == chunksCount){
				levelGenerated = true;
			}
			currentChunkCount = chunksCount;
		}
	}

	void Update(){
		if (levelGenerated && !fadingFuncCalled) {
			fadingFuncCalled = true;
			fading.LevelLoaded();
		}
	}

	public static void Initialize(){
		bgCount = 0;
	}

	public GameObject GenerateEnd (Vector3 pos, Quaternion rot, Exits exit){
		chunksCount++;
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

	public GameObject GenerateChunk(Vector3 pos, Quaternion rot, Exits exit, int[] chunkPos, int newChunkPosY){ //entry: "left", "right" , "up" , "down" , null para cualquiera
		bgCount++;
		chunksCount++;
		GameObject newChunk = null;
		if (!g.chunksPerZone.ContainsKey (g.currLevelName)) {
			Debug.LogError(g.currLevelName + " No encontrado!");
			return null;
		}
		int r;
		float[] chunksProb = {0.5f,0.3f,0.2f}; //40% uno normal, 40% un doble, 20% un cierre

		int choice = Utils.Choose(chunksProb);
		//Debug.Log ("chunk pos 0 " + chunkPos [0]);
		if((!doubleInRow[newChunkPosY] &&  choice == 2) || (!doubleInRow[newChunkPosY] && chunkPos[1] == MatrixSize - 3)){ //si estoy casi por llegar a un borde
			choice = 1;																												//o si toca un cierre, me aseguro que haya un doble antes		
		}
		if(doubleChunks.Count == 0) // esto es por ahora nomas, para que ande el level 1
			choice = 0;
		
		if((chunkPos[1] == 1 && exit == Exits.Right) ||(chunkPos[1] == MatrixSize-2 && exit == Exits.Left)){ //si llego a algun borde horizontal hay que poner un final
			choice = 2;
		}
		if(bottomUpGeneration){
			if(choice == 1){ //casos en los que no puede ir un doble
				if(newChunkPosY <= 0){ //si esta en el limite de la matriz no puede venir un doble
						choice = 0;
				}
				else{
					if((cmatrix[chunkPos[0]-1, chunkPos[1]-1] && exit == Exits.Right) || (cmatrix[chunkPos[0]-1, chunkPos[1]+1] && exit == Exits.Left)){
						Debug.Log("este doble no deberia ir baby");
						choice = 0;
					}
				}
			}

		}
		else{
			if(choice == 1){ //casos en los que no puede ir un doble
				if(newChunkPosY >= matrixDepth -1){ //si esta en el limite de la matriz no puede venir un doble
						choice = 0;
				}
				else{
					if((cmatrix[chunkPos[0]+1, chunkPos[1]-1] && exit == Exits.Right) || (cmatrix[chunkPos[0]+1, chunkPos[1]+1] && exit == Exits.Left)){ //si la posciciones de abajo
						Debug.Log("este doble no deberia ir baby");																						//de los doobles estan ocupadas		
						choice = 0;
					}
				}
			}

		}
			
		switch(choice){
			case 0:
				currentChunkId++;
				float miniBossProb = (newChunkPosY+1)/7;
				if(newChunkPosY == matrixDepth-1){ // si estoy abajo de todo genero el boss si o si
					miniBossProb = 1;
				}     
				float[] prob = {1 - miniBossProb, miniBossProb }; 
				                   //lo del miniboss por ahora deberia andar solo para la cueva
				if(Utils.Choose (prob) == 0 || minibossChunkGenerated || miniBossChunk == null){ //no toco el boss o ya esta generado o no hay miniboss					
					r = Random.Range (0,normalChunks.Count);
					if(hasCaveEntranceChunk && r == normalChunks.Count - 1){ //en la ultima posicion tiene que estar el chunk que entra a una cueva
						if(zoneEntranceGenerated || newChunkPosY != matrixDepth - 1){
							r = Random.Range (0,normalChunks.Count-1);
						}
						else{
							zoneEntranceGenerated = true;
						}
					}
					if(hasCaveEntranceChunk && chunkPos[0] == matrixDepth - 1 && chunkPos[1] == MatrixSize - 3 && !zoneEntranceGenerated){ // si estoy llegando casi al final y no se genero
						zoneEntranceGenerated = true;                                                          // el chunk cueva lo genero si o si
						r = normalChunks.Count - 1;
					}
									
					newChunk = (GameObject)Instantiate (normalChunks [r], pos, rot);
				}
				else{ //genero el chunk del miniboss
					newChunk = (GameObject)Instantiate (miniBossChunk, pos, rot);
					minibossChunkGenerated = true;	
				}
			
				if(bottomUpGeneration){
					newChunk.GetComponent<Chunk2>().chunkId = currentChunkId;
				}
				else
					newChunk.GetComponent<Chunk>().chunkId = currentChunkId;
				break;
			case 1:
				/*if(exit == Exits.Right)
					cmatrix[pos[0]+2,pos[1]-1] = true;*/
				currentChunkId++;
				if(hasCaveEntranceChunk && chunkPos[0] == matrixDepth - 1 && chunkPos[1] == MatrixSize - 3 && !zoneEntranceGenerated){ // si estoy llegando casi al final y no se genero
					zoneEntranceGenerated = true;                                                          // el chunk cueva lo genero si o si
					r = normalChunks.Count - 1;
					newChunk = (GameObject)Instantiate (normalChunks [r], pos, rot);
				}
				r = Random.Range (0,doubleChunks.Count);
				if(newChunk == null){
					newChunk = (GameObject)Instantiate (doubleChunks [r], pos, rot);
					doubleInRow[chunkPos[0]] = true;
				}
				if(bottomUpGeneration)
					newChunk.GetComponent<Chunk2>().chunkId = currentChunkId;
				else
					newChunk.GetComponent<Chunk>().chunkId = currentChunkId;
				break;
			case 2:
				currentChunkId++;
				if(exit == Exits.Right){
					r = Random.Range (0,leftEndChunks.Count);
				
					if(newChunk == null)
						newChunk = (GameObject)Instantiate (leftEndChunks [r], pos, rot);
				}
				else{
					r = Random.Range (0,rightEndChunks.Count);
					if(!outcastleExitGenerated && outcastleEntrance != null && newChunkPosY == 0){
						Debug.Log("OUTCASTLENETRANCEGENERATED");
						outcastleExitGenerated = true;
						newChunk = (GameObject)Instantiate (outcastleEntrance, pos, rot);
					}
					if(hasCaveEntranceChunk && !zoneEntranceGenerated && newChunkPosY == matrixDepth -1){
						newChunk = (GameObject)Instantiate (normalChunks [normalChunks.Count -1], pos, rot); //si va a tocar un cierre pero no se genero 
						zoneEntranceGenerated = true;																						//el chunk para entrar a la cueva, lo genero
					}
					if(newChunk == null)
						newChunk = (GameObject)Instantiate (rightEndChunks [r], pos, rot);
				}
				if(bottomUpGeneration)
					newChunk.GetComponent<Chunk2>().chunkId = currentChunkId;
				else
					newChunk.GetComponent<Chunk>().chunkId = currentChunkId;
				break;
			}
			g.chunksPerZone[g.currLevelName].Add(newChunk); //agrego el chunk a la lista de chunks de este nivel
			DontDestroyOnLoad(newChunk);
		//}
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
