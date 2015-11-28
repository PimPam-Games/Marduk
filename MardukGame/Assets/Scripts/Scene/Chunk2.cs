using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using g = GameController;

public class Chunk2 : MonoBehaviour {

	public Transform circle;
	public List<Transform> enemies;
	public Transform chunkEndRight; // chunkEndLeft es la posicion del chunk
	public Transform chunkEndUpR;
	public Transform chunkEndUpL;
	public bool isDouble;
	public bool hasLeftEnd, hasRightEnd; //que salidas tiene el chunk
	public bool leftUsed, rightUsed; //salidas usadas
	//public bool isFirst = false;
	public int[] position = new int[2]; // posicion en la matriz
	private bool alreadyGenerated = false;
	private bool layerChanged = false; //indica si ya se le cambio el layer para que sea visto por el minimapa
	public bool isFirstChunk = false;
	public int chunkId = 0;
	public bool isEnd = false; //true si es un chunk cuadrado, una terminacion
	private ChunkFactory cf;
	private LevelSettings ls;
	private float timeToGenerate = 0;

	// Use this for initialization
	
	void Awake(){
		cf = GameObject.Find ("LevelController").GetComponent<ChunkFactory>();
		ls = GameObject.Find ("LevelController").GetComponent<LevelSettings>();
		foreach(Transform child in this.transform){  //el layer del chunk y de todos los hijos es miniMapIgnored para que no lo muestre el minimapa
			child.gameObject.layer = LayerMask.NameToLayer("MiniMapIgnored");
			foreach(Transform c2 in child){
				c2.gameObject.layer = LayerMask.NameToLayer("MiniMapIgnored");
				foreach(Transform c3 in c2){
					if(c3.gameObject.name != "Collision")
						c3.gameObject.layer = LayerMask.NameToLayer("MiniMapIgnored");
					else
						c3.gameObject.layer = LayerMask.NameToLayer("Ground"); //el de collision tiene que ser ground si o si
				}
			}
		}
	}
	
	void Update(){
		
		timeToGenerate += Time.deltaTime;
		if (timeToGenerate > 0.02f && !alreadyGenerated && !isFirstChunk) {
			GenerateChunks();
		}
		
	}
	
	void Start () {
		if (g.chunksPerZone.ContainsKey (g.currLevelName)) {
			
			if (g.chunksPerZone [g.currLevelName].Count == 0){ //si esta en la lista pero no hay ningun chunk, crea el primero
				cf.cmatrix[cf.matrixDepth - 1,1] = true;                //  0000000
				this.position[0] = cf.matrixDepth - 1;					// 0000000 
				this.position[1] = 1;					            //	0000000
				GenerateChunks ();									//	1000000 
				alreadyGenerated = true;
			}
		} else {
			
			cf.cmatrix[cf.matrixDepth,1] = true; 
			this.position[0] = cf.matrixDepth;
			this.position[1] = 1;
			GenerateChunks (); //si  no esta en la lista  crea el primero
			alreadyGenerated = true;
		}
		//Debug.Log ("pos: "+ this.position[0]);
		foreach(Transform enemyPos in enemies){
			ls.generateEnemy(enemyPos.position,enemyPos.rotation);
		}
		/*if (!alreadyGenerated && !isFirstChunk) {

			GenerateChunks ();
		}*/
		//Debug.Log ("meto la key: " + g.currLevelName);
		//g.enemiesPerLevel.Add (g.currLevelName, enems);		
	}
	
	public void GenerateChunks(){
		//Debug.Log ("x: " + position[0] + "y: " + position[1]);
		if (alreadyGenerated)
			return;
		alreadyGenerated = true;
		if (hasRightEnd && !rightUsed) {
			rightUsed = true;
			if(this.position[1] < cf.MatrixSize-1 && !cf.cmatrix[this.position[0],this.position[1]+1]){

				GameObject g;
				g = cf.GenerateChunk (chunkEndRight.position, chunkEndRight.rotation,ChunkFactory.Exits.Left,this.position,this.position[0]);
				if(g!=null){
					Chunk2 nchunk = g.GetComponent<Chunk2>();
					nchunk.leftUsed = true;
					nchunk.position[0] = this.position[0]; //la posicion del nuevo chunk es la derecha del actual
					nchunk.position[1] = this.position[1]+1;
					if(cf.cmatrix[nchunk.position[0],nchunk.position[1]]){
						Debug.Log("Elimine un chunk ortiva");
						Destroy(nchunk);
					}
					if(nchunk.isDouble){
						cf.cmatrix[this.position[0]-1,this.position[1]+1] = true;
					}
					cf.cmatrix[this.position[0],this.position[1]+1] = true; //marco como ucupada la posicion de la derecha de este chunk
					//nchunk.GenerateChunks();
				}
			}
		}
		if (hasLeftEnd && !leftUsed ) {
			leftUsed = true;
			if(this.position[1] > 0 && !cf.cmatrix[this.position[0],this.position[1]-1] ){

				GameObject g;
				g = cf.GenerateChunk(transform.position,transform.rotation,ChunkFactory.Exits.Right,this.position,this.position[0]);
				if(g != null){
					Chunk2 nchunk = g.GetComponent<Chunk2>();
					nchunk.rightUsed = true; //el nuevo chunk no tinene que generar por la derecha por que ya esta usada por el chunk que lo acaba de crear
					nchunk.position[0] = this.position[0]; //la posicion del nuevo chunk es la izq del actual
					nchunk.position[1] = this.position[1]-1;
					if(cf.cmatrix[nchunk.position[0],nchunk.position[1]]){
						Debug.Log("Elimine un chunk ortiva");
						Destroy(nchunk);
					}
					if(nchunk.isDouble){
						cf.cmatrix[this.position[0]-1,this.position[1]-1] = true;
					}
					g.transform.position = new Vector3( g.transform.position.x -(g.transform.FindChild("ChunkEnd").position.x - g.transform.position.x),g.transform.position.y,g.transform.position.z);
					cf.cmatrix[this.position[0],this.position[1]-1] = true; //marco como ucupada la posision de la izq de este chunk
					//nchunk.GenerateChunks();
				}
			}
		}
		if (chunkEndUpL != null) {
			if(this.position[1] > 0 && this.position[0] > 0 && !cf.cmatrix[this.position[0]-1,this.position[1]-1]){
				//la posicion del nuevo es abajo a la izq

				GameObject g = cf.GenerateChunk(chunkEndUpL.position,chunkEndUpL.rotation,ChunkFactory.Exits.Right,this.position,this.position[0]-1);
				if(g != null){
					Chunk2 nchunk = g.GetComponent<Chunk2>();
					nchunk.rightUsed = true; //el nuevo chunk no tinene que generar por la derecha por que ya esta usada por el chunk que lo acaba de crear
					nchunk.position[0] = this.position[0]-1; //la posicion del nuevo chunk es la izq del actual
					nchunk.position[1] = this.position[1]-1;
					if(cf.cmatrix[nchunk.position[0],nchunk.position[1]]){
						Debug.Log("Elimine un chunk ortiva");
						Destroy(nchunk);
					}
					if(nchunk.isDouble){                                           // 0N000
						cf.cmatrix[this.position[0]-2,this.position[1]-1] = true;  // 0NX00
					} 															   // 00X00
					g.transform.position = new Vector3( g.transform.position.x -(g.transform.FindChild("ChunkEnd").position.x - g.transform.position.x),g.transform.position.y,g.transform.position.z);
					cf.cmatrix[this.position[0]-1,this.position[1]-1] = true;
					//nchunk.GenerateChunks();
				}
			}
		}
		if (chunkEndUpR != null) {
			if(this.position[1] < cf.MatrixSize-1 && this.position[0] > 0 && !cf.cmatrix[this.position[0]-1,this.position[1]+1]){
				
				//int[] p = {this.position[0]+1,this.position[1]};
			
				GameObject g = cf.GenerateChunk (chunkEndUpR.position, chunkEndUpR.rotation,ChunkFactory.Exits.Left,this.position,this.position[0]-1);
				if(g!=null){
					Chunk2 nchunk = g.GetComponent<Chunk2>();
					nchunk.leftUsed = true;
					nchunk.position[0] = this.position[0]-1; //la posicion del nuevo chunk es la der del actual
					nchunk.position[1] = this.position[1]+1;
					if(cf.cmatrix[nchunk.position[0],nchunk.position[1]]){
						Debug.Log("Elimine un chunk ortiva");
						Destroy(nchunk);
					}
					if(nchunk.isDouble){
						cf.cmatrix[this.position[0]-2,this.position[1]+1] = true;
					}
					cf.cmatrix[this.position[0]-1,this.position[1]+1] = true;
					//nchunk.GenerateChunks();
				}
			}
		}
	}
	

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "Chunk") {
			if(!this.isDouble && !col.GetComponent<Chunk2>().isEnd && !this.isEnd){ //este mambo es para solucionar dramas de chunks que se superponian en la cueva
				Debug.Log("choco el" + this.chunkId + " con "+ col.GetComponent<Chunk2>().chunkId);
				Destroy(this.gameObject);
			}
		}

		if (col.gameObject.tag == "Player") {
			if(!layerChanged){
				foreach(Transform child in this.transform){
					child.gameObject.layer = LayerMask.NameToLayer("Default");
					foreach(Transform c2 in child){
						c2.gameObject.layer = LayerMask.NameToLayer("Default");
						foreach(Transform c3 in c2){
							if(c3.gameObject.name != "Collision")
								c3.gameObject.layer = LayerMask.NameToLayer("Default");
							else
								c3.gameObject.layer = LayerMask.NameToLayer("Ground");
						}
					}
				}
				layerChanged = true;
			}
		}
	}
}
