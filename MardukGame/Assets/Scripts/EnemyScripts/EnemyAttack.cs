using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

	
	public bool hasMeleeAttack;
	public float attackRange = 1.9f;
	private float attackTimer = 0;
	[SerializeField] private float attackDelay = 3f;
	[SerializeField] private Animator anim;
	[SerializeField] private GameObject target;
	private PlayerStats playerStats;
	private EnemyIAMovement movement;
	private EnemyStats stats;

	public AudioSource attackSound = null;
	public bool useSacrifice = false;
	public bool canBlock;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player");
		movement = GetComponent<EnemyIAMovement> ();
		playerStats = target.GetComponent<PlayerStats> ();
		stats = GetComponent<EnemyStats> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (stats.isDead)
			return;
		attackTimer -= Time.deltaTime;
		if (hasMeleeAttack) {
			MeleeAttackPrepare ();
		} else {
			if(attackTimer <= 0){
				attackTimer = attackDelay;
				anim.SetBool("Attacking", true);
			}

		}
	}


	void MeleeAttackPrepare(){
		var dir = (target.transform.position - transform.position).normalized;
		var dot = Vector2.Dot(dir, transform.right); //negativo si player esta a su izquierda
		float distance = Vector3.Distance (target.transform.position, transform.position);
		if (distance < (attackRange - 0.2) && attackTimer <= 0 ) {
			if((dot < 0 && !movement.IsFacingRight()) || (dot > 0 && movement.IsFacingRight())){
				attackTimer = attackDelay;
				anim.SetBool("Attacking", true);
				movement.StopWalk();
			}
		}
	}

	void MeleeAttack(){
		var dir = (target.transform.position - transform.position).normalized;
		var dot = Vector2.Dot(dir, transform.right); //negativo si player esta a su izquierda
		float distance = Vector3.Distance (target.transform.position, transform.position);
		if(attackSound != null)
			attackSound.Play();
		if (distance < attackRange)
			if ((dot < 0 && !movement.IsFacingRight ()) || (dot > 0 && movement.IsFacingRight ())) {
				float damage = Random.Range (stats.MinDamage, stats.MaxDamage) * stats.meleeAttackMultiplier;
				bool isCrit = false;
				float[] critDmgProb = {1 - stats.critChance, stats.critChance};
				if(useSacrifice && stats.currHealth > (stats.initMaxHealth * 7 /100) + 1){
					damage *= 2;
					
					stats.currHealth -= stats.initMaxHealth * 7 /100; //resta 7 % de vida al atacar
					//Instantiate (stats.blood, new Vector3(transform.position.x,transform.position.y,-4), transform.rotation);
					ObjectsPool.GetBlood(this.transform.position,this.transform.rotation);
				}
				if(Utils.Choose(critDmgProb) != 0){
					isCrit = true;
					damage *= 2; //si es critico lo multiplico por 2 al daño del enemigo
				}
				//bool hitConfirmed = playerStats.Hit (damage, stats.elem,stats.Accuracy,isCrit); 
				bool hitConfirmed = playerStats.Hit (damage, stats.meleeAttackElem,1f,isCrit);
				if(hitConfirmed){
					if(target.transform.position.x < this.transform.position.x)
						target.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(true);
					else
						target.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(false);
				}
				PlatformerCharacter2D.skillBtnPressed = -1;
			}
	}

	void Idle(){
		movement.Walk ();
		if(canBlock)
			anim.SetBool ("Blocking", false);
		anim.SetBool("Attacking", false);

	}
}
