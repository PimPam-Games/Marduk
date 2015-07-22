using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChestGenerator : MonoBehaviour {

	public List<Transform> chestsPositions;
	private Object[] chestList;
	// Use this for initialization
	void Start () {
		chestList = Resources.LoadAll ("Level/Objects",typeof(Object));
		float[] chestProb = {0.5f,0.5f}; // 50% de prob de que aparezca un cofre

		if (Utils.Choose (chestProb) == 1) {
			int pos = Random.Range(0,chestsPositions.Count);
			int chestType = Random.Range(0,chestList.Length);
			Instantiate (chestList[chestType],chestsPositions[pos].position,chestsPositions[pos].rotation);
			//DontDestroyOnLoad(newChest);
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
