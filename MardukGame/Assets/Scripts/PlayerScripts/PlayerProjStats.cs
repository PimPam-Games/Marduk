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
	public bool dotSkill = false;
	public bool continuosRelease = false;
	public bool alwaysCrit = false;
	public int stopAfterTime = -1;

	//public float manaCost = 5f;
	private float rotationChange;
	public Animator anim;
	private Rigidbody2D rb;
	public AudioSource hitEnemySound;
	public AudioSource criticalHitSound;
	public AudioSource skillSound;
	public Types.SkillsRequirements projRequirements = Types.SkillsRequirements.None; //si se tiene que tirar con un arco o es melee, spell , etc
	public Types.Element convertElem = Types.Element.None; // a que elemento tiene que convertir el 40% del daño fisico
	private bool collision = false;
	private bool alreadyHit = false;
	private float stopCount = 0;
	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
		if(skillSound != null){
			skillSound.Play();
		}
		//PlayerStats.UpdateMana ();
		if(isParticle)
			gameObject.GetComponent<ParticleSystem> ().playbackSpeed = particleSpeed;
	}

	// Update is called once per frame
	void Update () {
		stopCount += Time.deltaTime;
		if(stopAfterTime != -1 && rb.velocity.x != 0){
			rb.velocity = new Vector2(rb.velocity.x /1.04f,rb.velocity.y);
			//rb.velocity = new Vector2(0,rb.velocity.y);
		}
		if(stopCount >= stopAfterTime && stopAfterTime != -1){
			rb.velocity = new Vector2(0,rb.velocity.y);
		}
		if (collision) {
			rb.velocity = new Vector2 (0, 0.2f);
			lifeTime += Time.deltaTime*5;
		}
		lifeTime += Time.deltaTime;
		if(lifeTime >= duration)
			Destroy(this.gameObject);

		if(continuosRelease){ //acomoda el poder segun para que lado esta mirando el personaje
			if(pc.isFacingRight())
				this.transform.rotation = Quaternion.Euler(0,0,90);
			else
				this.transform.rotation = Quaternion.Euler(0,0,-90);
		}
		if(continuosRelease && pc.skillBtnPressed < 1) //si el personaje deja de presionar una tecla, se deja de castear el poder
			StopIncinerate();								//en PlatformerUserControl y enemyStats se modifica la variable
	}
	
	void DestroyProjectile(){
		Destroy (this.gameObject);
	}
	
	public void Ready(){
		anim.SetBool("Ready",true); // solo lo usa glacier por ahora
	}

	public void Incinerate(){
		anim.SetBool("Incinerate",true);
	}

	public void StopIncinerate(){
		anim.SetBool("StopIncinerate", true);
	}

	void OnTriggerEnter2D(Collider2D col){ 
		if(dotSkill) //el daño de estos skills se calcula en la parte del enemigo
			return;
		float critChance = p.offensives [p.CritChance] + p.offensives [p.CritChance] * (p.offensives [p.IncreasedCritChance] / 100);
		float[] critDmgProb = {1 - critChance,critChance };
		//float[] critDmgProb = {0, 1f };
		float damage;
		float damageConverted = 0;
		Support supportSkill = null;

		if(pc.supportSkillPos > -1 && !alwaysCrit) //cargo el support del skill que se utilizo, si es -1 es por que no se uso ningun skill
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
			if (Traits.traits[Traits.LOWHPCRIT].isActive())
				if (p.currentHealth <= p.MaxHealth * 0.15)
					alwaysCrit = true;
			if (Traits.traits[Traits.ANTIAIR].isActive())
				if (enemy.GetComponent<EnemyStats>().enemyName == "Wraith" ||
				    enemy.GetComponent<EnemyStats>().enemyName == "Roc" ||
				    enemy.GetComponent<EnemyStats>().enemyName == "Pirobolus")
					damage *= 1.2f;
			//End Traits
			bool isCrit = false;
			if(Utils.Choose(critDmgProb) != 0 || alwaysCrit)
				isCrit = true;
			if(isCrit){ //si el ataque es critico lo multiplico y dependiendo de si golpea o no, se larga el sonido
				if(!alwaysCrit)
					damage *= p.offensives[p.CritDmgMultiplier];
				attackResult = enemy.GetComponent<EnemyStats>().Hit(damage,elem, true); //si elem no es None no se esquivan
				//Begin Traits
				if (Traits.traits[Traits.FIREDAMAGE].isActive ()) {
					enemy.GetComponent<EnemyStats>().Hit(damage/10,Types.Element.Fire, true);
				}
				if (Traits.traits[Traits.COLDDAMAGE].isActive ()) {
					enemy.GetComponent<EnemyStats>().Hit(damage/10,Types.Element.Cold, true);
				}
				if (Traits.traits[Traits.LIGHTDAMAGE].isActive ()) {
					enemy.GetComponent<EnemyStats>().Hit(damage/10,Types.Element.Lightning, true);
				}
				if (Traits.traits[Traits.POISONDAMAGE].isActive ()) {
					enemy.GetComponent<EnemyStats>().Hit(damage/10,Types.Element.Poison, true);
				}
				//End Traits
				if(attackResult){
					enemy.GetComponent<EnemyStats>().Hit(damageConverted,convertElem, true);
					if(supportSkill != null){
						enemy.GetComponent<EnemyStats>().Hit(supportSkill.damageAdded,supportSkill.dmgElement, true);
						Debug.Log("daño agregado: " + supportSkill.damageAdded + "tipo: " + supportSkill.dmgElement);
					}
					if(!alwaysCrit)
						criticalHitSound.Play();
					Debug.Log("Critical Dmg: " + damage);
					if(projRequirements == Types.SkillsRequirements.Bow)
						collision = true;
				}
				else{
					CombatText.ShowCombatText("Miss");
				}
			}
			else{
				attackResult = enemy.GetComponent<EnemyStats>().Hit(damage,elem, false);
				//Debug.Log("damage: " + damage);
				if(attackResult){  //si no es critico tira el sonido comun
						
					enemy.GetComponent<EnemyStats>().Hit(damageConverted,convertElem, false);
					//Begin Traits
					if (Traits.traits[Traits.FIREDAMAGE].isActive ()) {
						enemy.GetComponent<EnemyStats>().Hit(damage/10,Types.Element.Fire, false);
					}
					if (Traits.traits[Traits.COLDDAMAGE].isActive ()) {
						enemy.GetComponent<EnemyStats>().Hit(damage/10,Types.Element.Cold, false);
					}
					if (Traits.traits[Traits.LIGHTDAMAGE].isActive ()) {
						enemy.GetComponent<EnemyStats>().Hit(damage/10,Types.Element.Lightning, false);
					}
					if (Traits.traits[Traits.POISONDAMAGE].isActive ()) {
						enemy.GetComponent<EnemyStats>().Hit(damage/10,Types.Element.Poison, false);
					}
					//End Traits
					if(supportSkill != null){
						enemy.GetComponent<EnemyStats>().Hit(supportSkill.damageAdded,supportSkill.dmgElement, false);
						Debug.Log("daño agregado: " + supportSkill.damageAdded + "tipo: " + supportSkill.dmgElement);
					}
					hitEnemySound.Play();
					if(projRequirements == Types.SkillsRequirements.Bow)
						collision = true;
				}
				else{
					CombatText.ShowCombatText("Miss");
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

