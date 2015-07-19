using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using g = GameController;

public class Chunk : MonoBehaviour {

	//public GameObject[] chunkPool;
	public List<Transform> enemies;
	public Transform chunkEndRight; // chunkEndLeft es la posicion del chunk
	public Transform chunkEndDownR;
	public Transform chunkEndDownL;
	public bool isDouble;
	public bool hasLeftEnd, hasRightEnd; //que salidas tiene el chunk
	public bool leftUsed, rightUsed; //salidas usadas
	public bool isFirst = false;
	public int[] position = new int[2]; // posision en la matriz
	private bool alreadyGenerated = false;
	private float collisionDetectCount;

	private ChunkFactory cf;
	// Use this for initialization

	void Awake(){
		cf = GameObject.Find ("LevelController").GetComponent<ChunkFactory>();
		collisionDetectCount = 1;
	}

	void Update(){
		collisionDetectCount -= Time.deltaTime;
	}

	void Start () {
		if (g.chunksPerZone.ContainsKey (g.currLevelName)) {
			if (g.chunksPerZone [g.currLevelName].Count == 0){ //si esta en la lista pero no hay ningun chunk, crea el primero
				cf.cmatrix[0,cf.MatrixSize/2] = true; 
				this.position[0] = 0;
				this.position[1] = cf.MatrixSize/2;
				GenerateChunks ();
			}
		} else {
			cf.cmatrix[0,cf.MatrixSize/2] = true; 
			this.position[0] = 0;
			this.position[1] = cf.MatrixSize/2;
			GenerateChunks (); //si  no esta en la lista  crea el primero
		}
		foreach(Transform enemyPos in enemies){
			int index = Random.Range(0,g.enemyList.Length); //slecciona un enemigo aleatorio de la lista de enemigos
			GameObject newEnemy = (GameObject)Instantiate (g.enemyList[index],enemyPos.position,enemyPos.rotation);
			DontDestroyOnLoad(newEnemy);
			g.enemiesPerLevel[g.currLevelName].Add(newEnemy);
		}
		//Debug.Log ("meto la key: " + g.currLevelName);
		//g.enemiesPerLevel.Add (g.currLevelName, enems);		
	}

	void GenerateChunks(){
		Debug.Log ("x: " + position[0] + "y: " + position[1]);
		if (alreadyGenerated)
			return;
		alreadyGenerated = true;
		if (hasRightEnd && !rightUsed) {
			if(this.position[1] < cf.MatrixSize-1 && !cf.cmatrix[this.position[0],this.position[1]+1]){
				cf.cmatrix[this.position[0],this.position[1]+1] = true; //marco como ucupada la posision de la derecha de este chunk
				GameObject g;
				if(this.position[1] == cf.MatrixSize-2){
					g = cf.GenerateEnd (chunkEndRight.position, chunkEndRight.rotation,ChunkFactory.Exits.Right);
				}
				else
					g = cf.GenerateChunk (chunkEndRight.position, chunkEndRight.rotation,ChunkFactory.Exits.Left);
				if(g!=null){
					Chunk nchunk = g.GetComponent<Chunk>();
					nchunk.leftUsed = true;
					nchunk.position[0] = this.position[0]; //la posicion del nuevo chunk es la derecha del actual
					nchunk.position[1] = this.position[1]+1;
					if(nchunk.isDouble){
						cf.cmatrix[this.position[0]+1,this.position[1]+1] = true;
					}
				}
			}
		}
		if (hasLeftEnd && !leftUsed ) {
			if(this.position[1] > 0 && !cf.cmatrix[this.position[0],this.position[1]-1] ){
				cf.cmatrix[this.position[0],this.position[1]-1] = true; //marco como ucupada la posision de la izq de este chunk
				GameObject g;
				if(this.position[1] == 1){
					g = cf.GenerateEnd (transform.position,transform.rotation,ChunkFactory.Exits.Left);
				}else
					g = cf.GenerateChunk(transform.position,transform.rotation,ChunkFactory.Exits.Right);
				if(g != null){
					Chunk nchunk = g.GetComponent<Chunk>();
					nchunk.rightUsed = true; //el nuevo chunk no tinene que generar por la derecha por que ya esta usada por el chunk que lo acaba de crear
					nchunk.position[0] = this.position[0]; //la posicion del nuevo chunk es la izq del actual
					nchunk.position[1] = this.position[1]-1;
					if(nchunk.isDouble){
						cf.cmatrix[this.position[0]+1,this.position[1]-1] = true;
					}
					g.transform.position = new Vector3( g.transform.position.x -(g.transform.FindChild("ChunkEnd").position.x - g.transform.position.x),g.transform.position.y,g.transform.position.z);
				}
			}
		}
		if (chunkEndDownL != null) {
			if(this.position[1] > 0 && this.position[0] < cf.MatrixSize-1 && !cf.cmatrix[this.position[0]+1,this.position[1]-1]){
				cf.cmatrix[this.position[0]+1,this.position[1]-1] = true; //la posicion del nuevo es abajo a la iaq
				GameObject g = cf.GenerateChunk(chunkEndDownL.position,chunkEndDownL.rotation,ChunkFactory.Exits.Right);
				if(g != null){
					Chunk nchunk = g.GetComponent<Chunk>();
					nchunk.rightUsed = true; //el nuevo chunk no tinene que generar por la derecha por que ya esta usada por el chunk que lo acaba de crear
					nchunk.position[0] = this.position[0]+1; //la posicion del nuevo chunk es la izq del actual
					nchunk.position[1] = this.position[1]-1;
					if(nchunk.isDouble){
						cf.cmatrix[this.position[0]+2,this.position[1]-1] = true;
					}
					g.transform.position = new Vector3( g.transform.position.x -(g.transform.FindChild("ChunkEnd").position.x - g.transform.position.x),g.transform.position.y,g.transform.position.z);
				}
			}
		}
		if (chunkEndDownR != null) {
			if(this.position[1] < cf.MatrixSize-1 && this.position[0] < cf.MatrixSize-1 && !cf.cmatrix[this.position[0]+1,this.position[1]+1]){
				cf.cmatrix[this.position[0]+1,this.position[1]+1] = true;
				GameObject g = cf.GenerateChunk (chunkEndDownR.position, chunkEndDownR.rotation,ChunkFactory.Exits.Left);
				if(g!=null){
					Chunk nchunk = g.GetComponent<Chunk>();
					nchunk.leftUsed = true;
					nchunk.position[0] = this.position[0]+1; //la posicion del nuevo chunk es la der del actual
					nchunk.position[1] = this.position[1]+1;
					if(nchunk.isDouble){
						cf.cmatrix[this.position[0]+2,this.position[1]+1] = true;
					}
				}
			}
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (collisionDetectCount > 0)
			return;
		if (col.gameObject.tag == "Player" && !alreadyGenerated) {
			Debug.Log("Genero");
			GenerateChunks();
		}
	}
}
