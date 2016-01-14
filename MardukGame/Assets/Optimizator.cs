using UnityEngine;
using System.Collections;

public class Optimizator : MonoBehaviour {

	private float dist = 0;
	private Transform target = null;
	private float checkDistTimer = 0;
	private GameObject child;	

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player").transform;
		child = transform.GetChild(0).gameObject; //obtiene el enemigo u objeto, que deberia ser el unico hijo
	}
	
	// Update is called once per frame
	void Update () {
		checkDistTimer -= Time.deltaTime;
		if(target != null && checkDistTimer <= 0){		
			if(child == null){
				Destroy(this.gameObject);
				return;
			}
			dist = Vector2.Distance (new Vector2(target.transform.position.x,target.transform.position.y),new Vector2(child.transform.position.x,child.transform.position.y));			
			if(dist > 25){ //deshabilita al objeto si esta muy lejos del jugador
				if(child.activeSelf)
					child.SetActive(false);
			}
			else{
				if(!child.activeSelf)
					child.SetActive(true);
			}
			checkDistTimer = 0.3f;
		}
	}
}
