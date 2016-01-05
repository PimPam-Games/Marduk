using UnityEngine;
using System.Collections;

public class TrapGenerator : MonoBehaviour {

	public GameObject[] traps;

	// Use this for initialization
	void Start () {
		float num = 0;
		foreach(GameObject t in traps){ 
			num = Random.Range(1f,10f);
			if(num > 7f){
				t.SetActive(true);
				t.GetComponent<Trap>().triggerTime = Random.Range(1.5f,5f);
			}
			else{
				t.SetActive(false);
			}
		}
	}
	
}
