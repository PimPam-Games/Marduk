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
	public float attackTimer;
	public AudioSource hitEnemySound;
	public AudioSource criticalHitSound;
	public Animator anim = null; 
	public float animSpeed = 0;
	public GameObject weaponProjLauncher1;
	public GameObject weaponProjLauncher2;    
	public float checkAnimSpeedTimer = 0;

    public static bool newWeaponEquipped = false; //se setea en true cada vez que se equipa un arma
    public static bool newWeaponRecentlyEquipped = false; //este se chequea en el update, para que se cambie mas rapido la rotacion del arma
    public SpriteRenderer weaponSprite;
    //private float normalAnimSpeed;
    Quaternion rotation;
    void Awake()
    {
        rotation = transform.rotation;
       // Debug.Log("rotation " + rotation);
    }

    public void UpdateRotation() {
        if (newWeaponEquipped) //si se equipo un arma se fija si es de dos manos o no para ubicarla
        {

            newWeaponEquipped = false;
            if (weaponSprite != null && weaponSprite.sprite != null)
            {
                 if (string.Compare(weaponSprite.sprite.name, "bill") == 0)
                 {
                    // this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -0.29f));
                     rotation = new Quaternion(0.0f,0.0f,-0.33f,1.0f);
                     weaponSprite.sortingOrder = 4;
                 }
                 else {
                    //this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0.149f));
                    Debug.Log("rotation updated");
                     rotation = Quaternion.Euler(new Vector3(0, 0, 370.69f));

                    // rotation = new Quaternion(0.0f, 0.0f, -0.1f, 1.0f);
                    this.transform.rotation = rotation;
                    //rotation = Quaternion.Euler(new Vector3(0, 0, 0.149f));
                    weaponSprite.sortingOrder = 1;
                }
            }
        }
    }

    // Update is called once per frame
    void Update () {
        if (newWeaponRecentlyEquipped && newWeaponEquipped) //si se equipo un arma se fija si es de dos manos o no para ubicarla
        {
            newWeaponRecentlyEquipped = false;
            if (weaponSprite != null && weaponSprite.sprite != null)
            {
                if (string.Compare(weaponSprite.sprite.name, "bill") == 0)
                {
                    // this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, -0.29f));
                    rotation = new Quaternion(0.0f, 0.0f, -0.33f, 1.0f);
                    weaponSprite.sortingOrder = 4;
                }
                else {
                    Debug.Log("recentrly Equipped updated");
                    //this.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0.149f));
                    rotation = Quaternion.Euler(new Vector3(0, 0, 370.69f));
                    // rotation = new Quaternion(0.0f, 0.0f, -0.1f, 1.0f);
                    this.transform.rotation = rotation;
                    //rotation = Quaternion.Euler(new Vector3(0, 0, 0.149f));
                    weaponSprite.sortingOrder = 1;
                }
            }
        }
        checkAnimSpeedTimer -= Time.deltaTime;
        attackDelay = 1 / (p.offensives[p.BaseAttacksPerSecond] + (p.offensives[p.BaseAttacksPerSecond] * (p.offensives[p.IncreasedAttackSpeed] / 100)));
        if (checkAnimSpeedTimer <= 0){
			checkAnimSpeedTimer = 0.4f;			
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
		}
		//attackingTime -= Time.deltaTime;
		//Debug.Log("attack timer : " + attackTimer.ToString());
	
		if (anim.GetBool ("Attacking") == false && anim.GetBool("PolearmAttacking") == false)
        {
            if (weaponSprite != null && weaponSprite.sprite != null)
            {
                if (string.Compare(weaponSprite.sprite.name, "bill") == 0 && !anim.GetBool("Crouch"))
                    this.transform.rotation = rotation;
            }

            isAttacking = false;
			this.GetComponent<SpriteRenderer>().color = new Color(1,1,1,1); //si no esta atacando pone el arma en su color original
		}

		if (attackTimer <= 0 && anim.GetBool ("Attacking") == false && anim.GetBool ("BowAttacking") == false && anim.GetBool ("SpellCasting") == false && anim.GetBool("PolearmAttacking") == false) { //anim.GetBool ("Attacking") == false && 
			anim.speed = p.currentAnimSpeed;
			isAttacking = false;
			canAttack = true;
        }

		if(!GetComponent<PolygonCollider2D>().isTrigger)
			GetComponent<PolygonCollider2D>().isTrigger = true;
	}

	private void FixedUpdate(){
		attackTimer -= Time.fixedDeltaTime;
	}


	public void Attack(){
		if (canAttack) {
			if(PlayerItems.EquipedWeapon == null || PlayerItems.EquipedWeapon.Type == ItemTypes.Weapon
			   || PlayerItems.EquipedWeapon.Type == ItemTypes.TwoHandedWeapon)
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
		EnemyStats estats = col.gameObject.GetComponent<EnemyStats>();

		elem = Types.Element.None;
		if (estats != null && isAttacking) {
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
			//if (Traits.traits[Traits.PDAMAGE].isActive ()) {
			//	damage = damage * (float)1.5;
			//}
			if (Traits.traits[Traits.LOWHPDAMAGE].isActive ()) {
				if (p.currentHealth <= p.MaxHealth*(float)0.3)
					damage = damage * (float)1.25;
			}
            //End Traits
           // Debug.Log("meleeskillposWepapon" + pc.meleeSkillPos);
            if (pc.meleeSkillPos > -1){ //si la posicion es mayor a -1, significa que se esta usando un skill melee
               
				MeleeSkill ms = pc.playerSkills[pc.meleeSkillPos].GetComponent<MeleeSkill>();
				damage *= ms.DmgMultiplier/100;
				elem = ms.elementToConvert;
				supportSkill = ms.SupportSkill;
				if(PlatformerCharacter2D.useSacrifice)
					p.currentHealth -= (p.defensives[p.MaxHealth] * ms.SacrifiedLife) /100;
				if(pc.useMeleeProjLauncher && ms.projectile != null){
					PlayerProjStats msProj = ms.projectile.GetComponent<PlayerProjStats>();
					msProj.minDmg = damage * 0.30f; //por ahora es asi loco,
					msProj.maxDmg = damage * 0.30f;
					if(pc.isFacingRight()){
						weaponProjLauncher1.transform.rotation = Quaternion.Euler(0,0,90);
					}
					else{
						weaponProjLauncher1.transform.rotation = Quaternion.Euler(0,0,270);
					}
					Instantiate (msProj, weaponProjLauncher1.transform.position, weaponProjLauncher1.transform.rotation);
				}	
				if(pc.useThunderBlow && ms.projectile != null){ //thunder blow
					PlayerProjStats msProj = ms.projectile.GetComponent<PlayerProjStats>();
					msProj.minDmg = damage *  0.40f; //por ahora es asi loco,
					msProj.maxDmg = damage *  0.40f;
					weaponProjLauncher1.transform.rotation = Quaternion.Euler(0,0,90);
					Instantiate (msProj, weaponProjLauncher1.transform.position, weaponProjLauncher1.transform.rotation);
				}

				//Begin Traits
				if (Traits.traits[Traits.MSKILLDMG].isActive()){
					damage *= 1.2f;
				}
				//End Traits
			}
			/*if(pc.supportSkillPos > -1) //cargo el support del skill que se utilizo, si es -1 es por que no se uso ningun skill
				supportSkill = (Support)pc.playerSupportSkills[pc.supportSkillPos];*/
			float critChance = p.offensives [p.CritChance] + p.offensives [p.CritChance] * (p.offensives [p.IncreasedCritChance] / 100);
			float[] critDmgProb = {1 - critChance, critChance };
			bool isCrit = false;
			if(Utils.Choose(critDmgProb) != 0){
				damage *= p.offensives[p.CritDmgMultiplier];

				isCrit = true;
				Debug.Log("Critical Dmg: " + damage);
			}
			damage += damage * p.offensives[p.IncreasedDmg]/100;
			//Begin Traits
			if (Traits.traits[Traits.LOWHPCRIT].isActive()){
				if (p.currentHealth <= p.defensives[p.MaxHealth] * 0.15)
					isCrit = true;
			}
			if (Traits.traits[Traits.ANTIAIR].isActive()){
				if (estats.enemyName == "Wraith" ||
				    estats.enemyName == "Roc" ||
				    estats.enemyName == "Pirobolus" ||
				    estats.enemyName == "Zu")
					damage *= 1.2f;
			}
			if (Traits.traits[Traits.SWORDDMG].isActive() && PlayerItems.EquipedWeapon.WeaponType == WeaponTypes.Sword){
				damage *= 1.1f;
			}
			if (Traits.traits[Traits.AXEDMG].isActive() && PlayerItems.EquipedWeapon.WeaponType == WeaponTypes.Axe){
				damage *= 1.1f;
			}
			if (Traits.traits[Traits.MACEDMG].isActive() && PlayerItems.EquipedWeapon.WeaponType == WeaponTypes.Mace){
				damage *= 1.1f;
			}
			if (Traits.traits [Traits.ACCURACY].isActive ()) {
				if (p.currentMana >= p.MaxMana * 0.9)
					damage += damage/10;
			}
			//End Traits
			//weapon constraints
			if (estats.IsArmored)
            {
				switch (PlayerItems.EquipedWeapon.WeaponType){
				case WeaponTypes.Sword: damage *= 0.5f;break;
				case WeaponTypes.Axe: damage *= 0.75f;break;
				default: break;
				}
			}

			bool hit = estats.Hit(damage,elem, isCrit,supportSkill);

			if(hit){
				if (isCrit){
					criticalHitSound.Play();
				}
				else{
					hitEnemySound.Play();
				}

				//Begin Traits
				if (Traits.traits[Traits.FIREDAMAGE].isActive ()) {
					estats.Hit(damage/10,Types.Element.Fire, isCrit);
				}
				if (Traits.traits[Traits.COLDDAMAGE].isActive ()) {
					estats.Hit(damage/10,Types.Element.Cold, isCrit);
				}
				if (Traits.traits[Traits.LIGHTDAMAGE].isActive ()) {
					estats.Hit(damage/10,Types.Element.Lightning, isCrit);
				}
				if (Traits.traits[Traits.POISONDAMAGE].isActive ()) {
					estats.Hit(damage/10,Types.Element.Poison, isCrit);
				}
				//End Traits

				/*if(supportSkill != null){
					estats.Hit((damage * supportSkill.damageAdded) / 100, supportSkill.dmgElement, isCrit); //le pego con el support
					Debug.Log("daño agregado: " + (damage * supportSkill.damageAdded) / 100 + "tipo: " + supportSkill.dmgElement);
				}*/
				if(enemy.transform.position.x < this.transform.position.x)
					enemy.GetComponent<EnemyIAMovement>().Knock(true);
				else
					enemy.GetComponent<EnemyIAMovement>().Knock(false);
			}
			else {
				CombatText.ShowCombatText("Miss");
			}
			isAttacking = false;
		}
	}
}
