using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ObjectsPool : MonoBehaviour {

	public GameObject bloodGo;

	public static Queue<GameObject> bloods;

	// Use this for initialization
	void Start () {
		bloods = new Queue<GameObject>();
		for(int i = 0; i < 3; i++){ //crea 3 sangres al inicio del juego
			GameObject b = (GameObject)Instantiate(bloodGo, this.transform.position, this.transform.rotation);
			DontDestroyOnLoad(b);
			b.SetActive(false);
			bloods.Enqueue(b);
		}
	}
	
	public static void GetBlood(Vector3 pos, Quaternion rot){ 
		if(!bloods.Peek().activeSelf){	//se fija si la siguiente sangre no se esta usando
			GameObject b = bloods.Dequeue();
			b.transform.position = pos;
			b.transform.rotation = rot;
			b.SetActive(true);
			bloods.Enqueue(b);
		}
		else{ //si se esta usando instancia otra y la agrega a la cola
			GameObject b = (GameObject)Instantiate(bloods.Peek(), pos, rot);
			b.transform.position = pos;
			b.transform.rotation = rot;
			b.SetActive(true);
			bloods.Enqueue(b);
		}	
	}

	public static void ClearQueues(){
		while(bloods.Count > 0){
			Destroy (bloods.Dequeue());
		}
	}

}
