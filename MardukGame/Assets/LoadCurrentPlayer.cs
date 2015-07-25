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
		if (loadCount > 0.2f && g.nameToLoad != null ){

			Persistence.Load (GameController.nameToLoad);
			g.nameToLoad = null;
			Debug.Log ("Load Data");
		}
	}
}
