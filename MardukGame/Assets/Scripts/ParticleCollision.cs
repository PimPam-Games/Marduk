﻿using UnityEngine;
using System.Collections;

public class ParticleCollision : MonoBehaviour {

	public Types.Element elem = Types.Element.Fire ;
	public float dmg = 5f;
	public BoxCollider2D col;
	private string direction;

	// Use this for initialization
	void Start () {
		//Debug.Log (transform.rotation.eulerAngles);
		if (transform.rotation.eulerAngles.x == 270)
			direction = "Up";
		if(transform.rotation.eulerAngles.x == 90)
			direction = "Down";
		if(transform.rotation.eulerAngles.y == 90)
			direction = "Right";
		if(transform.rotation.eulerAngles.y == 270)
			direction = "Left";
		transform.rotation = Quaternion.Euler (0,0,0);
		StartCoroutine (RemoveColAfterTime(1.7f));
	}
	
	// Update is called once per frame
	void Update () {
		if (direction == "Left") { //agranda el collider dinamicamente hacia la izquierda
			if(col.size.x < 20 ){
				col.size += new Vector2(0.6f,0);
				col.offset += new Vector2 (-0.25f,0);
			}
		}
		if (direction == "Right") {
			if(col.size.x < 20 ){
				col.size += new Vector2(0.6f,0);
				col.offset += new Vector2 (0.25f,0);
			}
		}
		if (direction == "Up") {
			if(col.size.y < 20 ){
				col.size += new Vector2(0,0.6f);
				col.offset += new Vector2 (0,0.25f);
			}
		}
		if (direction == "Down") {
			if(col.size.y < 20 ){
				col.size += new Vector2(0,0.6f);
				col.offset += new Vector2 (0,-0.25f);
			}
		}
	}

	IEnumerator RemoveColAfterTime(float collLifetime) {
		yield return new WaitForSeconds (collLifetime);
		col.enabled = false;
		
	}

	void OnTriggerEnter2D (Collider2D col){
		if (col.tag == "Player") {
			col.GetComponent<PlayerStats>().Hit(dmg,elem);
			if(col.transform.position.x < this.transform.position.x)
				col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(true);
			else
				col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(false);
		}
	}
}
