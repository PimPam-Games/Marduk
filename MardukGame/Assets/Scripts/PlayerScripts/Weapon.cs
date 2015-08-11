using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using p = PlayerStats;

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
	public static float animSpeed = 0;

	void Start () {
		//attackTimer = 0;
		//attackDelay = p.offensives [p.AttackSpeed];
	}
	
	// Update is called once per frame
	void Update () {
		attackDelay = 1 / (p.offensives [p.BaseAttacksPerSecond] + (p.offensives [p.BaseAttacksPerSecond] * (p.offensives [p.IncreasedAttackSpeed]/100)));

		//attackingTime -= Time.deltaTime;
		//Debug.Log("attack timer : " + attackTimer.ToString());
		attackTimer -= Time.fixedDeltaTime;
		if (attackTimer <= 0) { //anim.GetBool ("Attacking") == false && 

			isAttacking = false;
			canAttack = true;
		}
		if(!GetComponent<PolygonCollider2D>().isTrigger)
			GetComponent<PolygonCollider2D>().isTrigger = true;
	}


	public void Attack(){
		if (canAttack) {
			//Debug.Log("ataque!" + Time.time);
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
		//	Debug.Log("attack Delay : " + attackDelay.ToString());
		//	Debug.Log(animSpeed.ToString());
			anim.speed += animSpeed;
			isAttacking = true;
			attackTimer = attackDelay;
			canAttack = false;
			//attackingTime = maxAttackingTime;

		}
	}

	void OnTriggerEnter2D(Collider2D col){
		GameObject enemy = col.gameObject;

		if (enemy.tag == "Enemy" && isAttacking) {
			//Debug.Log ("Le pegue a " + enemy.name);
		

			if(p.LifePerHit > 0 )
				p.currentHealth += p.defensives[p.LifePerHit];
			float damage = Random.Range (p.offensives[p.MinDmg], p.offensives[p.MaxDamge]);
			float[] critDmgProb = {1 - p.offensives[p.CritChance], p.offensives[p.CritChance] };
			if(Utils.Choose(critDmgProb) != 0){
				damage *= p.offensives[p.CritDmgMultiplier];
				criticalHitSound.Play();
				Debug.Log("Critical Dmg: " + damage);
			}
			else{
				hitEnemySound.Play();
			}
			enemy.GetComponent<EnemyStats>().Hit(damage,elem);
			if(enemy.transform.position.x < this.transform.position.x)
				enemy.GetComponent<EnemyIAMovement>().Knock(true);
			else
				enemy.GetComponent<EnemyIAMovement>().Knock(false);

			isAttacking = false;
		}
		//alreadyAttacked = false;

	}

	void OnTriggerStay2D(Collider2D col){
		GameObject enemy = col.gameObject;

		if (enemy.tag == "Enemy" && isAttacking) {
			//Debug.Log ("Le pegue a " + enemy.name);

			if(p.LifePerHit > 0 )
				p.currentHealth += p.defensives[p.LifePerHit];
			float damage = Random.Range (p.offensives[p.MinDmg], p.offensives[p.MaxDamge]);
			float[] critDmgProb = {1 - p.offensives[p.CritChance], p.offensives[p.CritChance] };
			if(Utils.Choose(critDmgProb) != 0){
				damage *= p.offensives[p.CritDmgMultiplier];
				criticalHitSound.Play();
				Debug.Log("Critical Dmg: " + damage);
			}
			else{
				hitEnemySound.Play();
			}
			enemy.GetComponent<EnemyStats>().Hit(damage,elem);
			if(enemy.transform.position.x < this.transform.position.x)
				enemy.GetComponent<EnemyIAMovement>().Knock(true);
			else
				enemy.GetComponent<EnemyIAMovement>().Knock(false);
			
			isAttacking = false;
		}
		//alreadyAttacked = false;

	}

	void OnTriggerExit2D(Collider2D col){
		GameObject enemy = col.gameObject;
		
		if (enemy.tag == "Enemy" && isAttacking) {
			//Debug.Log ("Le pegue a " + enemy.name);

			if(p.LifePerHit > 0 )
				p.currentHealth += p.defensives[p.LifePerHit];
			float damage = Random.Range (p.offensives[p.MinDmg], p.offensives[p.MaxDamge]);
			float[] critDmgProb = {1 - p.offensives[p.CritChance], p.offensives[p.CritChance] };
			if(Utils.Choose(critDmgProb) != 0){
				damage *= p.offensives[p.CritDmgMultiplier];
				criticalHitSound.Play();
				Debug.Log("Critical Dmg: " + damage);
			}
			else{
				hitEnemySound.Play();
			}
			enemy.GetComponent<EnemyStats>().Hit(damage,elem);
			if(enemy.transform.position.x < this.transform.position.x)
				enemy.GetComponent<EnemyIAMovement>().Knock(true);
			else
				enemy.GetComponent<EnemyIAMovement>().Knock(false);
			
			isAttacking = false;
		}
		//alreadyAttacked = false;

	}
	/*private void DoDammage(){ // se fija si colisiona con enemigos y les saca vida
		int enemyLayerMask = 1 << LayerMask.NameToLayer ("Enemy");
		float radius = this.GetComponent<CircleCollider2D> ().radius;
		Collider2D[] overlappedThings = Physics2D.OverlapCircleAll (new Vector2 (transform.position.x, transform.position.y), radius, enemyLayerMask); //crea una lista de todos los enemigos que colisiona
		if (overlappedThings.Length > 0)
			enemyDamaged = true;
		for (int i = 0; i < overlappedThings.Length; i++) { 
			GameObject enemy = overlappedThings [i].gameObject;
			// Do something to the enemy
			float damage = Random.Range(minDmg,maxDmg);
			Debug.Log ("Le pegue a " + enemy.name + " daño:" + damage);
			enemy.GetComponent<EnemyStats>().Hit(damage,elem);
		}
	}*/
}
