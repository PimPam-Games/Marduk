using UnityEngine;
using System.Collections;

public class NextChunkGenerator : MonoBehaviour {


	public GameObject nextChunk;
	public Transform chunkStart;
	public Transform chunkEnd;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){
	
		if (col.gameObject.tag == "Player") {
			Instantiate(nextChunk,chunkEnd.position,chunkEnd.rotation);
			Debug.Log ("Generar Chunk");
		}
	}
}
