using UnityEngine;
using System.Collections;
using p = PlayerStats;

public class Trap : MonoBehaviour {

	public Types.Element elem = Types.Element.None;
	public float minDmg = 0, maxDmg = 0;
	public float triggerTime = 4f;

	private float timer;
	private Animator anim;
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if(timer <= 0 && !anim.GetBool("triggered")){
			anim.SetBool("triggered",true);
			timer = triggerTime;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if(col.gameObject.tag == "Player" && p.isDead)
			Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		if (col.gameObject.tag == "Player" && !p.isDead) {
			float dmgDealt = Random.Range(minDmg,maxDmg);
			col.gameObject.GetComponent<PlayerStats>().Hit(dmgDealt, elem,-1, false);
			if(col.transform.position.x < this.transform.position.x)
				col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(true);
			else
				col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(false);
			PlatformerCharacter2D.skillBtnPressed = -1;
		}
	}

	void DeactivateTrap(){
		anim.SetBool("triggered",false);
	}
}
