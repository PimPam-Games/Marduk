using UnityEngine;
using System.Collections;
using p = PlayerStats;
using pc = PlatformerCharacter2D;

public class PlayerProjStats : MonoBehaviour {
	
	public Types.Element elem ;
	public float minDmg = 1 , maxDmg = 3;
	public float physicalDmgMult = 100f; //se usa para aumentar el daño de ataque fisicos
	public float duration = 5;
	private float lifeTime = 0;
	public float particleSpeed;
	public bool isParticle;
	public bool dontDestroy = false;
	public bool hasSplashAnim = false;
	public bool isAoe = false;

	//public float manaCost = 5f;
	private float rotationChange;
	public Animator anim;
	private Rigidbody2D rb;
	public AudioSource hitEnemySound;
	public AudioSource criticalHitSound;
	public Types.SkillsRequirements projRequirements = Types.SkillsRequirements.None; //si se tiene que tirar con un arco o es melee, spell , etc
	public Types.Element convertElem = Types.Element.None; // a que elemento tiene que convertir el 40% del daño fisico
	private bool collision = false;
	private bool alreadyHit = false;
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
	
	void OnTriggerEnter2D(Collider2D col){ 
		float critChance = p.offensives [p.CritChance] + p.offensives [p.CritChance] * (p.offensives [p.IncreasedCritChance] / 100);
		float[] critDmgProb = {1 - critChance,critChance };
		//float[] critDmgProb = {0, 1f };
		float damage;
		float damageConverted = 0;
		Support supportSkill = null;

		if(pc.supportSkillPos > -1) //cargo el support del skill que se utilizo, si es -1 es por que no se uso ningun skill
			supportSkill = (Support)pc.playerSupportSkills[pc.supportSkillPos];

		if (col.gameObject.tag == "Enemy") {
			if (!isAoe && alreadyHit)
				return;
			alreadyHit = true;
			EnemyStats enemy = col.gameObject.GetComponent<EnemyStats>();
			if(elem == Types.Element.None){
				if(p.LifePerHit > 0) //solo los ataques fisicos roban vida
					p.currentHealth += p.defensives[p.LifePerHit];
				damage = Random.Range (p.offensives[p.MinDmg], p.offensives[p.MaxDamge]);
				damage = damage * physicalDmgMult/100;  //aumenta el daño en un porcentaje dependiendo de la habilidad	
				if(convertElem != Types.Element.None){
					damage = damage * 0.6f; 
					damageConverted = damage * 0.4f; //al 40% del daño fisico lo convierte en otro daño
				}
			}
			else{
				damage = Random.Range(minDmg,maxDmg);
				damage += damage * p.offensives[p.IncreasedMgDmg]/100;
				/*if(col.transform.position.x < this.transform.position.x)
					col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(true);
				else
					col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(false);*/
			}
			bool attackResult = false ;
			//Begin Traits
			if (Traits.traits[Traits.LOWHPDAMAGE].isActive ()) {
				if (p.currentHealth <= p.MaxHealth*(float)0.3)
					damage = damage * (float)1.25;
			}
			//End Traits
			if(Utils.Choose(critDmgProb) != 0){ //si el ataque es critico lo multiplico y dependiendo de si golpea o no, se larga el sonido
				damage *= p.offensives[p.CritDmgMultiplier];
				attackResult = enemy.GetComponent<EnemyStats>().Hit(damage,elem, true); //si elem no es None no se esquivan
				if(attackResult){
					enemy.GetComponent<EnemyStats>().Hit(damageConverted,convertElem, true);
					if(supportSkill != null){
						enemy.GetComponent<EnemyStats>().Hit(supportSkill.damageAdded,supportSkill.dmgElement, true);
						Debug.Log("daño agregado: " + supportSkill.damageAdded + "tipo: " + supportSkill.dmgElement);
					}
					criticalHitSound.Play();
					Debug.Log("Critical Dmg: " + damage);
					if(projRequirements == Types.SkillsRequirements.Bow)
						collision = true;
				}
			}
			else{
				attackResult = enemy.GetComponent<EnemyStats>().Hit(damage,elem, false);
				//Debug.Log("damage: " + damage);
				if(attackResult){  //si no es critico tira el sonido comun
						
					enemy.GetComponent<EnemyStats>().Hit(damageConverted,convertElem, false);
					if(supportSkill != null){
						enemy.GetComponent<EnemyStats>().Hit(supportSkill.damageAdded,supportSkill.dmgElement, false);
						Debug.Log("daño agregado: " + supportSkill.damageAdded + "tipo: " + supportSkill.dmgElement);
					}
					hitEnemySound.Play();
					if(projRequirements == Types.SkillsRequirements.Bow)
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

