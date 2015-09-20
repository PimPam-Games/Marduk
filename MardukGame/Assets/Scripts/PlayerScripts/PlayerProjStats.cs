using UnityEngine;
using System.Collections;
using p = PlayerStats;

public class PlayerProjStats : MonoBehaviour {
	
	public Types.Element elem ;
	public float minDmg = 1 , maxDmg = 3;
	public float duration = 5;
	private float lifeTime = 0;
	public float particleSpeed;
	public bool isParticle;
	public bool dontDestroy = false;
	public bool hasSplashAnim = false;
	//public float manaCost = 5f;
	private float rotationChange;
	public Animator anim;
	private Rigidbody2D rb;
	public AudioSource hitEnemySound;
	public AudioSource criticalHitSound;
	public Types.SkillsTypes projType = Types.SkillsTypes.Spell; //si se tiene que tirar con un arco o es melee, spell , etc
	public Types.Element convertElem = Types.Element.None; // a que elemento tiene que convertir el 40% del daño fisico
	private bool collision = false;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();

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
		//float[] critDmgProb = {1 - p.offensives[p.CritChance], p.offensives[p.CritChance] };
		float[] critDmgProb = {0, 1f };
		float damage;
		float damageConverted = 0;
		if (col.gameObject.tag == "Enemy") {
			EnemyStats enemy = col.gameObject.GetComponent<EnemyStats>();
			if(elem == Types.Element.None){
				if(p.LifePerHit > 0) //solo los ataques fisicos roban vida
					p.currentHealth += p.defensives[p.LifePerHit];
				damage = Random.Range (p.offensives[p.MinDmg], p.offensives[p.MaxDamge]);
				if(convertElem != Types.Element.None){
					damage = damage * 0.6f; 
					damageConverted = damage * 0.4f; //al 40% del daño fisico lo convierte en otro daño
				}
			}
			else{
				damage = Random.Range(minDmg,maxDmg);
				damage += p.offensives[p.MgDmg];
				/*if(col.transform.position.x < this.transform.position.x)
					col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(true);
				else
					col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(false);*/
			}
			bool attackResult; 
			if(Utils.Choose(critDmgProb) != 0){ //si el ataque es critico lo multiplico y dependiendo de si golpea o no, se larga el sonido
				damage *= p.offensives[p.CritDmgMultiplier];
				attackResult = enemy.GetComponent<EnemyStats>().Hit(damage,elem, true); //si elem no es None no se esquivan
				if(attackResult){
					enemy.GetComponent<EnemyStats>().Hit(damageConverted,convertElem, true);
					criticalHitSound.Play();
					Debug.Log("Critical Dmg: " + damage);
					if(projType == Types.SkillsTypes.Bow)
						collision = true;
				}
			}
			else{
				attackResult = enemy.GetComponent<EnemyStats>().Hit(damage,elem, false);
				if(attackResult){  //si no es critico tira el sonido comun
					enemy.GetComponent<EnemyStats>().Hit(damageConverted,convertElem, false);
					hitEnemySound.Play();
					if(projType == Types.SkillsTypes.Bow)
						collision = true;
				}
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

