using UnityEngine;
using System.Collections;

public class ProjectileStats : MonoBehaviour {

	public Types.Element elem ;
	public float minDmg = 1, maxDmg = 3;
	private float duration = 6;
	private float lifeTime = 0;
	public float particleSpeed;

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<ParticleSystem> ().playbackSpeed = particleSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		lifeTime += Time.deltaTime;
		if(lifeTime >= duration)
			Destroy(this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D col){ //si le pego al jugador le resto la vida
		if (col.gameObject.tag == "Player") {
			float dmgDealt = Random.Range(minDmg,maxDmg);
			col.gameObject.GetComponent<PlayerStats>().Hit(dmgDealt, elem);
			if(col.transform.position.x < this.transform.position.x)
				col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(true);
			else
				col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(false);
			Destroy(this.gameObject);
		}
		if (col.gameObject.layer == LayerMask.NameToLayer("Ground")) {
			Destroy(this.gameObject);
		}
	}
}
