using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using p = PlayerStats;

public class Weapon : MonoBehaviour {

	private float attackDelay; // tiempo de espera entre cada ataque
	private float maxAttackingTime = 0.5f; // tiempo que dura el ataque
	public bool canAttack = false;
	private bool isAttacking = false;
	[SerializeField] private Types.Element elem = Types.Element.None; // falta agregar esto a los arreglos de playerStats
	private float attackingTime; //tiempo que dura el ataque mientras se esta realizando
	//private bool enemyDamaged;
	private float attackTimer; 
	public AudioSource hitEnemySound;
	private bool alreadyAttacked; // para ver si le pego en OnEnterTrigger
	
	// Use this for initialization
	void Start () {
		attackTimer = 0;
		attackDelay = p.offensives [p.AttackRate];

	}
	
	// Update is called once per frame
	void Update () {
		attackDelay = p.offensives [p.AttackRate];
		attackingTime -= Time.deltaTime;
		attackTimer -= Time.deltaTime;
		if(attackingTime <=0) // si no esta atacando no le pego a ningun enemigo
			isAttacking = false;
		if (attackTimer <= 0)
			canAttack = true;
		//if (attackingTime > 0 && !enemyDamaged) { //hace daño en el tiempo en que tarda el ataque en ejecutarse siempre y cuando no le haya hecho daño al enemigo una vez
			//DoDammage(); 
		//}
		if(!GetComponent<PolygonCollider2D>().isTrigger)
			GetComponent<PolygonCollider2D>().isTrigger = true;
	}


	public void Attack(){
		if (canAttack) {
			alreadyAttacked = false;
			isAttacking = true;
			attackTimer = attackDelay;
			canAttack = false;
			attackingTime = maxAttackingTime;

		}
	}

	void OnTriggerEnter2D(Collider2D col){
		GameObject enemy = col.gameObject;
		if (alreadyAttacked)
			return;
		if (enemy.tag == "Enemy" && isAttacking) {
			//Debug.Log ("Le pegue a " + enemy.name);
			alreadyAttacked = true;
			hitEnemySound.Play();
			if(p.LifePerHit > 0 )
				p.currentHealth += p.defensives[p.LifePerHit];
			float damage = Random.Range (p.offensives[p.MinDmg], p.offensives[p.MaxDamge]);
			float[] critDmgProb = {1 - p.offensives[p.CritChance], p.offensives[p.CritChance] };
			if(Utils.Choose(critDmgProb) != 0){
				damage *= p.offensives[p.CritDmgMultiplier];
				Debug.Log("Critical Dmg: " + damage);
			}
			enemy.GetComponent<EnemyStats>().Hit(damage,elem);
			if(enemy.transform.position.x < this.transform.position.x)
				enemy.GetComponent<EnemyIAMovement>().Knock(true);
			else
				enemy.GetComponent<EnemyIAMovement>().Knock(false);

			
		}
		//alreadyAttacked = false;
		isAttacking = false;
	}

	void OnTriggerStay2D(Collider2D col){
		GameObject enemy = col.gameObject;
		if (alreadyAttacked)
			return;
		if (enemy.tag == "Enemy" && isAttacking) {
			//Debug.Log ("Le pegue a " + enemy.name);
			alreadyAttacked = true;
			hitEnemySound.Play();
			if(p.LifePerHit > 0 )
				p.currentHealth += p.defensives[p.LifePerHit];
			float damage = Random.Range (p.offensives[p.MinDmg], p.offensives[p.MaxDamge]);
			float[] critDmgProb = {1 - p.offensives[p.CritChance], p.offensives[p.CritChance] };
			if(Utils.Choose(critDmgProb) != 0){
				damage *= p.offensives[p.CritDmgMultiplier];
				Debug.Log("Critical Dmg: " + damage);
			}
			enemy.GetComponent<EnemyStats>().Hit(damage,elem);
			if(enemy.transform.position.x < this.transform.position.x)
				enemy.GetComponent<EnemyIAMovement>().Knock(true);
			else
				enemy.GetComponent<EnemyIAMovement>().Knock(false);
			
			
		}
		//alreadyAttacked = false;
		isAttacking = false;
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
