using UnityEngine;
using System.Collections;
using g = GameController;

public class ChangeLevel : MonoBehaviour {

	public int exitNumber;
	public Fading fading;
	private string levelToLoad;
	private BoxCollider2D box;
	private float boxTimeCount = 0.5f;
	public GameObject enemyGenerator;

	void Awake(){
		box = GetComponent<BoxCollider2D> ();
		box.enabled = false;
		boxTimeCount = 0.5f;
	}

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if (boxTimeCount < 0)
			box.enabled = true; //espero un poco para habilitar el colisionador de las salidas del nuevo nivel. (es para arreglar el problema de la parada)
		boxTimeCount -= Time.deltaTime;

	}

	void OnTriggerEnter2D(Collider2D coll){
		if (coll.gameObject.tag == "Player") {
			Debug.Log("Cambie de level");
			DestroyItems(); //destruye los items que no hayan sido agarrados por el player
			Fade ();
			g.SetActiveEnemies(g.currLevelName,false);
			g.previousExit = exitNumber;
			foreach(string[] connection in g.levelConnections){
				if(connection[0] == "level" + g.currentLevel && connection[1] == exitNumber.ToString()){
					g.currLevelName = connection[2];
					Application.LoadLevel(connection[2]);
					return;
				}
			}
			if(GameController.notVisitedLevels.Count == 0)
				return;
			int nextLevel = Random.Range(0,g.notVisitedLevels.Count); //random de todos lo levels que no hayan suido visitados
			levelToLoad = g.notVisitedLevels[nextLevel];
			g.notVisitedLevels.RemoveAt(nextLevel);
			string[] c1 = {"level" + g.currentLevel, exitNumber.ToString(), levelToLoad};
			string[] c2 = {levelToLoad, (exitNumber + 1).ToString(), "level"+g.currentLevel};

			g.levelConnections.Add(c1);
			g.levelConnections.Add(c2);
			g.currLevelName = levelToLoad;
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