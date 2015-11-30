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
	public EnemyStats enemyStats;
	private bool alreadyHit = false; //booleano para evitar que le pegue dos veces al jugador

	

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		if(isParticle)
			gameObject.GetComponent<ParticleSystem> ().playbackSpeed = particleSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		lifeTime += Time.deltaTime;
		if(lifeTime >= duration)
			Destroy(this.gameObject);
	}

	void DestroyProjectile(){
		Destroy (this.gameObject);
	}
	
	void OnTriggerEnter2D(Collider2D col){ //si le pego al jugador le resto la vida
		if (col.gameObject.tag == "Player" && !alreadyHit) {
			bool hitConfirmed = false;
			float dmgDealt = Random.Range(minDmg,maxDmg);
			bool isCrit = false;
			float[] critDmgProb = {1 - enemyStats.critChance, enemyStats.critChance};
			if(Utils.Choose(critDmgProb) != 0){
				isCrit = true;
				dmgDealt *= 2; //si es critico lo multiplico por 2 al daño del enemigo
			}
			hitConfirmed = col.gameObject.GetComponent<PlayerStats>().Hit(dmgDealt, elem,enemyStats.Accuracy,isCrit);
			alreadyHit = true;
			if(hitConfirmed){
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
			PlatformerCharacter2D.castInterruptByMovement = true;
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
