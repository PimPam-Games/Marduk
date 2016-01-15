using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ChestGenerator : MonoBehaviour {

	public List<Transform> chestsPositions;
	private Object[] chestList;
	private Object[] anotherList;
	// Use this for initialization
	void Start () {
		chestList = Resources.LoadAll ("Level/Objects/chests",typeof(Object));
		anotherList = Resources.LoadAll ("Level/Objects/another",typeof(Object));
		float[] chestProb = {0.5f,0.5f}; // 50% de prob de que aparezca un cofre

		if (Utils.Choose (chestProb) == 0) {
			int pos = Random.Range (0, chestsPositions.Count);
			int chestType = Random.Range (0, chestList.Length);
			Instantiate (chestList [chestType], chestsPositions [pos].position, chestsPositions [pos].rotation);
			//DontDestroyOnLoad(newChest);
		} else {
			//float[] anotherProb = {0.5f,0.5f};
			//if (Utils.Choose (anotherProb) == 1) {
				int pos = Random.Range(0,chestsPositions.Count);
				int anotherType = Random.Range(0,anotherList.Length);
				Instantiate (anotherList[anotherType],chestsPositions[pos].position,chestsPositions[pos].rotation);
			//}
		}
	}
}
