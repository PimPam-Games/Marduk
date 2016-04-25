using UnityEngine;
using System.Collections;
using p = PlayerStats;
public class Door : MonoBehaviour {

	public int id = -1; //depths entrance = 0

/*	void Start () {
		
	}*/

	void OnEnable(){
		switch (id) {
			case 0:
				this.gameObject.SetActive(!p.depthsEntranceOpened);	//si la puerta esta cerrada desactiva este objeto	
				break;
		}
	}

}
