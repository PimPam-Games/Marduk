using UnityEngine;
using System.Collections;

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
			if(GameController.notVisitedLevels.Count == 0)
				return;
			int nextLevel = Random.Range(0,GameController.notVisitedLevels.Count); //random de todos lo levels que no hayan suido visitados
			levelToLoad = GameController.notVisitedLevels[nextLevel];
			GameController.notVisitedLevels.RemoveAt(nextLevel);
			Debug.Log(levelToLoad);
			Application.LoadLevel(levelToLoad);

		}
	}

	private void DestroyItems(){
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