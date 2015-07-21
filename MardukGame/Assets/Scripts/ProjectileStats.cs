using UnityEngine;
using System.Collections;

public class ProjectileStats : MonoBehaviour {

	public Types.Element elem ;
	public float minDmg = 1, maxDmg = 3;
	private float duration = 6;
	private float lifeTime = 0;
	public float particleSpeed;
	public bool isParticle;
	public bool dontDestroy = false;
	public bool hasSplashAnim = false;
	private float rotationChange;
	public Animator anim;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();

		if(isParticle)
			gameObject.GetComponent<ParticleSystem> ().playbackSpeed = particleSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		/*Vector2 v = rb.velocity;
		float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);*/
		lifeTime += Time.deltaTime;
		if(lifeTime >= duration)
			Destroy(this.gameObject);
	}

	void DestroyProjectile(){
		Destroy (this.gameObject);
	}

	void OnTriggerEnter2D(Collider2D col){ //si le pego al jugador le resto la vida
		if (col.gameObject.tag == "Player") {
			float dmgDealt = Random.Range(minDmg,maxDmg);
			col.gameObject.GetComponent<PlayerStats>().Hit(dmgDealt, elem);
			if(col.transform.position.x < this.transform.position.x)
				col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(true);
			else
				col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(false);
			if(!dontDestroy){
				if(hasSplashAnim){

					anim.SetBool("hit",true);
					rb.isKinematic = true;
				}else{
					Destroy(this.gameObject);
				}
			}
		}
		if (col.gameObject.layer == LayerMask.NameToLayer("Ground")) {
			if(!dontDestroy){
				if(hasSplashAnim){

					anim.SetBool("hit",true);
					rb.isKinematic = true;
				}else{
					Destroy(this.gameObject);
				}
			}	
		}
	}
}
