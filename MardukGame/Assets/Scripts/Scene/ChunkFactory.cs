﻿using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using g = GameController;

public class ChunkFactory : MonoBehaviour {

	public int zone = 1;
	public int  MatrixSize = 61;
	public string bgName = "Mountain";
	public string sceneName = "level1";
	public bool generateBackground;
	public bool[,] cmatrix;

	private Object[] chunkPool;
	private  List<Object> commonChunks = new List<Object>();
	public List<Object> LeftExitChunks = new List<Object>();
	public List<Object> RightExitChunks = new List<Object>();
	public List<Object> LeftEndChunks = new List<Object>();
	public List<Object> RightEndChunks = new List<Object>();
	private  List<Object> castleChunks = new List<Object>();
	private static Object bg;
	private static bool isEntry = false;
	public static float bgCount = 0;
	public static float bgPerChunk = 7; //cada cuantos chunks debe generar el fondo

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
			r = Random.Range (0,LeftEndChunks.Count);
			newChunk = (GameObject)Instantiate (LeftEndChunks [r], pos, rot);
			break;
		case Exits.Right:
			r = Random.Range (0,RightEndChunks.Count);
			newChunk = (GameObject)Instantiate (RightEndChunks [r], pos, rot);
			break;
		}
		g.chunksPerZone[g.currLevelName].Add(newChunk); //agrego el chunk a la lista de chunks de este nivel
		DontDestroyOnLoad(newChunk);
		return newChunk;
	}

	public GameObject GenerateChunk(Vector3 pos, Quaternion rot, Exits exit){ //entry: "left", "right" , "up" , "down" , null para cualquiera
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
			switch(exit){
			case Exits.Left:
				r = Random.Range (0,LeftExitChunks.Count);
				if(r >= LeftExitChunks.Count){
					Debug.LogError("problema al generar chunk");
					return null;
				}
				newChunk = (GameObject)Instantiate (LeftExitChunks [r], pos, rot);
				break;
		
			case Exits.Right:
				r = Random.Range (0,RightExitChunks.Count);
				if(r >= RightExitChunks.Count){
					Debug.LogError("problema al generar chunk");
					return null;
				}
				newChunk = (GameObject)Instantiate (RightExitChunks [r], pos, rot);
				break;
			}
			if(newChunk.name.Contains("Entry"))
				isEntry = true;
			g.chunksPerZone[g.currLevelName].Add(newChunk); //agrego el chunk a la lista de chunks de este nivel
			DontDestroyOnLoad(newChunk);
		}
		if(generateBackground)
			if (bgCount >= bgPerChunk) {
				bgCount = 0;
				GameObject go = (GameObject)Instantiate(bg,new Vector3(pos.x + 35,pos.y - 2,35),rot);
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
