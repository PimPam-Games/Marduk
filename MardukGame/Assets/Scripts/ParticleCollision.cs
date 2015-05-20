using UnityEngine;
using System.Collections;

public class ParticleCollision : MonoBehaviour {

	public Types.Element elem = Types.Element.Fire ;
	public float dmg = 5f;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
