using UnityEngine;
using System.Collections;
using p = PlayerStats;

public class PlayerProjStats : MonoBehaviour {
	
	public Types.Element elem ;
	public float minDmg = 1, maxDmg = 3;
	public float duration = 5;
	private float lifeTime = 0;
	public float particleSpeed;
	public bool isParticle;
	public bool dontDestroy = false;
	public bool hasSplashAnim = false;
	public float manaCost = 5f;
	private float rotationChange;
	public Animator anim;
	private Rigidbody2D rb;
	public AudioSource hitEnemySound;
	public AudioSource criticalHitSound;
	private bool collision = false;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		PlayerStats.currentMana -= manaCost;
		//PlayerStats.UpdateMana ();
		if(isParticle)
			gameObject.GetComponent<ParticleSystem> ().playbackSpeed = particleSpeed;
	}
	
	// Update is called once per frame
	void Update () {
		/*Vector2 v = rb.velocity;
		float angle = Mathf.Atan2(v.y, v.x) * Mathf.Rad2Deg;
		transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);*/
		if (collision) {
			rb.velocity = new Vector2 (0, 0.2f);
			lifeTime += Time.deltaTime*5;
		}
		lifeTime += Time.deltaTime;
		if(lifeTime >= duration)
			Destroy(this.gameObject);
	}
	
	void DestroyProjectile(){
		Destroy (this.gameObject);
	}
	
	void OnTriggerEnter2D(Collider2D col){ //si le pego al jugador le resto la vida
		if (col.gameObject.tag == "Enemy") {
			EnemyStats enemy = col.gameObject.GetComponent<EnemyStats>();
			if(elem == Types.Element.None){
				if(p.LifePerHit > 0 )
					p.currentHealth += p.defensives[p.LifePerHit];
				float damage = Random.Range (p.offensives[p.MinDmg], p.offensives[p.MaxDamge]);
				float[] critDmgProb = {1 - p.offensives[p.CritChance], p.offensives[p.CritChance] };
				bool attackResult; 
				if(Utils.Choose(critDmgProb) != 0){
					damage *= p.offensives[p.CritDmgMultiplier];
					attackResult = enemy.GetComponent<EnemyStats>().Hit(damage,elem);
					if(attackResult){
						criticalHitSound.Play();
						Debug.Log("Critical Dmg: " + damage);
						collision = true;
					}
				}
				else{
					attackResult = enemy.GetComponent<EnemyStats>().Hit(damage,elem);
					if(attackResult){
						hitEnemySound.Play();
						collision = true;
					}
				}

			}
			else{
				float dmgDealt = Random.Range(minDmg,maxDmg);
				dmgDealt += p.offensives[p.MgDmg];
				enemy.Hit(dmgDealt, elem);
				/*if(col.transform.position.x < this.transform.position.x)
					col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(true);
				else
					col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(false);*/
			}
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
			else{
				if(elem == Types.Element.None)
					collision = true;

			}
		}
	}
}

