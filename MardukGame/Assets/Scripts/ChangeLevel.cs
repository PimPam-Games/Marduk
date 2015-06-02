using UnityEngine;
using System.Collections;
using g = GameController;

public class ChangeLevel : MonoBehaviour {

	private GameController gameMainController;
	public int exitNumber;
	public Fading fading;
	private string levelToLoad;
	
	// Use this for initialization
	void Start () {
		gameMainController = GameObject.Find ("GameMainController").GetComponent<GameController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	void OnCollisionEnter2D(Collision2D coll){
		if (coll.gameObject.tag == "Player") {
			DestroyItems(); //destruye los items que no hayan sido agarrados por el player
			Fade ();
			gameMainController.previousExit = exitNumber;
			foreach(string[] connection in g.levelConnections){
				if(connection[0] == "level" + g.currentLevel && connection[1] == exitNumber.ToString()){
					Application.LoadLevel(connection[2]);
					return;
				}
			}
			if(GameController.notVisitedLevels.Count == 0)
				return;
			int nextLevel = Random.Range(0,g.notVisitedLevels.Count); //random de todos lo levels que no hayan suido visitados
			levelToLoad = g.notVisitedLevels[nextLevel];
			g.notVisitedLevels.RemoveAt(nextLevel);
			Debug.Log(levelToLoad);
			string[] c1 = {"level"+g.currentLevel, exitNumber.ToString(), levelToLoad};
			string[] c2 = {levelToLoad, (exitNumber + 1).ToString(), "level"+g.currentLevel};

			g.levelConnections.Add(c1);
			g.levelConnections.Add(c2);
			Application.LoadLevel(levelToLoad);

		}
	}

	public static void DestroyItems(){
		GameObject[] items = GameObject.FindGameObjectsWithTag ("Item");
		foreach(GameObject item in items ){
			if(item.activeSelf)
				Destroy(item.gameObject);
		}
	}
	IEnumerator Fade(){
		float fadeTime = fading.BeginFade (1);
		yield return new WaitForSeconds (fadeTime);
	}
}