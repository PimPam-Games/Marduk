using UnityEngine;
using System.Collections;
using g = GameController;

public class LoadCurrentPlayer : MonoBehaviour {

	private float loadCount;
	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		loadCount += Time.deltaTime;
		if (loadCount > 0.2f && !g.levelLoaded && g.nameToLoad.Length > 0 ){
			Persistence.Load (g.nameToLoad);
			g.levelLoaded = true;
			Debug.Log ("Load Data");
		}
	}
}
