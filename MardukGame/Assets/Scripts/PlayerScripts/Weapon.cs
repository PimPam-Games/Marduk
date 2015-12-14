using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using p = PlayerStats;
using pc = PlatformerCharacter2D;

public class Weapon : MonoBehaviour {

	private float attackDelay; // tiempo de espera entre cada ataque
	//private float maxAttackingTime = 0.5f; // tiempo que dura el ataque
	public bool canAttack = false;
	private bool isAttacking = false;
	[SerializeField] private Types.Element elem = Types.Element.None; // falta agregar esto a los arreglos de playerStats
	//private float attackingTime; //tiempo que dura el ataque mientras se esta realizando
	private float attackTimer;
	public AudioSource hitEnemySound;
	public AudioSource criticalHitSound;
	public Animator anim = null; 
	public float animSpeed = 0;
	public GameObject weaponProjLauncher1;
	public GameObject weaponProjLauncher2;
	//private float normalAnimSpeed;
	void Start () {
		//attackTimer = 0;
		//attackDelay = p.offensives [p.AttackSpeed];
		//normalAnimSpeed = anim.speed;
	}
	
	// Update is called once per frame
	void Update () {
		attackDelay = 1 / (p.offensives [p.BaseAttacksPerSecond] + (p.offensives [p.BaseAttacksPerSecond] * (p.offensives [p.IncreasedAttackSpeed]/100)));
		if(attackDelay >= 0.8f)
			animSpeed = 0;
		if(attackDelay < 0.8f && attackDelay >= 0.5f)
			animSpeed = 1;
		if(attackDelay < 0.5f && attackDelay >= 0.3f)
			animSpeed = 2;
		if(attackDelay < 0.3f && attackDelay >= 0.15f)
			animSpeed = 6;
		if(attackDelay < 0.15f)
			animSpeed = 8;
		//attackingTime -= Time.deltaTime;
		//Debug.Log("attack timer : " + attackTimer.ToString());
		attackTimer -= Time.fixedDeltaTime;
		if (anim.GetBool ("Attacking") == false){
			isAttacking = false;
			this.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1); //si no esta atacando pone el arma en su color original
		}
		if (attackTimer <= 0 && anim.GetBool ("Attacking") == false && anim.GetBool ("BowAttacking") == false && anim.GetBool ("SpellCasting") == false) { //anim.GetBool ("Attacking") == false && 
			anim.speed = p.currentAnimSpeed;
			isAttacking = false;
			canAttack = true;
		}

		if(!GetComponent<PolygonCollider2D>().isTrigger)
			GetComponent<PolygonCollider2D>().isTrigger = true;
	}


	public void Attack(){
		if (canAttack) {
			if(PlayerItems.EquipedWeapon == null || PlayerItems.EquipedWeapon.Type == ItemTypes.Weapon)
				isAttacking = true;
			attackTimer = attackDelay;
			canAttack = false;
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		DoDamage(col);

	}

	void OnTriggerStay2D(Collider2D col){
		DoDamage(col);

	}

	void OnTriggerExit2D(Collider2D col){
		DoDamage(col);
	}

	private void DoDamage(Collider2D col){
		GameObject enemy = col.gameObject;
		elem = Types.Element.None;
		if (enemy.tag == "Enemy" && isAttacking) {
			//Debug.Log ("Le pegue a " + enemy.name);
			Support supportSkill = null;
			if(p.LifePerHit > 0 ){
				//Begin Traits
				if (Traits.traits[Traits.MPLEECH].isActive ()) 
					p.currentMana += p.defensives[p.LifePerHit];
				//End Traits
				else
					p.currentHealth += p.defensives[p.LifePerHit];
				
			}
			float damage = Random.Range (p.offensives[p.MinDmg], p.offensives[p.MaxDamge]);
			//Begin Traits
			if (Traits.traits[Traits.PDAMAGE].isActive ()) {
				damage = damage * (float)1.5;
			}
			if (Traits.traits[Traits.LOWHPDAMAGE].isActive ()) {
				if (p.currentHealth <= p.MaxHealth*(float)0.3)
					damage = damage * (float)1.25;
			}
			//End Traits
			if(pc.meleeSkillPos > -1){ //si la posicion es mayor a -1, significa que se esta usando un skill melee
				MeleeSkill ms = pc.playerSkills[pc.meleeSkillPos].GetComponent<MeleeSkill>();
				damage *= ms.DmgMultiplier/100;
				elem = ms.elementToConvert;
		
				if(pc.useMeleeProjLauncher && ms.projectile != null){
					PlayerProjStats msProj = ms.projectile.GetComponent<PlayerProjStats>();
					msProj.minDmg = damage * 0.55f; //por ahora es asi loco
					msProj.maxDmg = damage * 0.55f;
					if(pc.isFacingRight()){
						weaponProjLauncher1.transform.rotation = Quaternion.Euler(0,0,90);
					}
					else{
						weaponProjLauncher1.transform.rotation = Quaternion.Euler(0,0,270);
					}
					Instantiate (msProj, weaponProjLauncher1.transform.position, weaponProjLauncher1.transform.rotation);
				}	
			}
			if(pc.supportSkillPos > -1) //cargo el support del skill que se utilizo, si es -1 es por que no se uso ningun skill
				supportSkill = (Support)pc.playerSupportSkills[pc.supportSkillPos];

			float[] critDmgProb = {1 - p.offensives[p.CritChance], p.offensives[p.CritChance] };
			bool isCrit = false;
			if(Utils.Choose(critDmgProb) != 0){
				damage *= p.offensives[p.CritDmgMultiplier];
				//Begin Traits

				//End Traits
				isCrit = true;
				Debug.Log("Critical Dmg: " + damage);
			}

			bool hit = enemy.GetComponent<EnemyStats>().Hit(damage,elem, isCrit);

			
			if(hit){
				if (isCrit && !Traits.traits[Traits.ACCURACY].isActive ()){
					criticalHitSound.Play();
				}
				else{
					hitEnemySound.Play();
				}

				//Begin Traits
				if (Traits.traits[Traits.FIREDAMAGE].isActive ()) {
					enemy.GetComponent<EnemyStats>().Hit(damage/10,Types.Element.Fire, isCrit);
				}
				if (Traits.traits[Traits.COLDDAMAGE].isActive ()) {
					enemy.GetComponent<EnemyStats>().Hit(damage/10,Types.Element.Cold, isCrit);
				}
				if (Traits.traits[Traits.LIGHTDAMAGE].isActive ()) {
					enemy.GetComponent<EnemyStats>().Hit(damage/10,Types.Element.Lightning, isCrit);
				}
				if (Traits.traits[Traits.POISONDAMAGE].isActive ()) {
					enemy.GetComponent<EnemyStats>().Hit(damage/10,Types.Element.Poison, isCrit);
				}
				//End Traits

				if(supportSkill != null)
					enemy.GetComponent<EnemyStats>().Hit(supportSkill.damageAdded,supportSkill.dmgElement, isCrit); //le pego con el support
				if(enemy.transform.position.x < this.transform.position.x)
					enemy.GetComponent<EnemyIAMovement>().Knock(true);
				else
					enemy.GetComponent<EnemyIAMovement>().Knock(false);
			}
			
			isAttacking = false;
		}
	}
}
