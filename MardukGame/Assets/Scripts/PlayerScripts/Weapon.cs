using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using p = PlayerStats;

public class Weapon : MonoBehaviour {

	[SerializeField] private float attackDelay = 5f;
	public bool canAttack = false;
	private bool isAttacking = false;
	[SerializeField] private Types.Element elem = Types.Element.None; // falta agregar esto a los arreglos de playerStats
	private float attackingTime; //tiempo que dura el ataque mientras se esta realizando
	//private bool enemyDamaged;
	private float attackTimer; 
	public AudioSource hitEnemySound;

	
	// Use this for initialization
	void Start () {
		attackTimer = 0;

	}
	
	// Update is called once per frame
	void Update () {

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
			isAttacking = true;
			attackTimer = attackDelay;
			canAttack = false;
			attackingTime = 0.7f;

		}
	}

	void OnTriggerEnter2D(Collider2D col){
		GameObject enemy = col.gameObject;
		if (enemy.tag == "Enemy" && isAttacking) {
			//Debug.Log ("Le pegue a " + enemy.name);
			hitEnemySound.Play();
			if(p.LifePerHit > 0 )
				p.currentHealth += p.defensives[p.LifePerHit];
			float damage = Random.Range (p.offensives[p.MinDmg], p.offensives[p.MaxDamge]);
			enemy.GetComponent<EnemyStats>().Hit(damage,elem);
			if(enemy.transform.position.x < this.transform.position.x)
				enemy.GetComponent<EnemyIAMovement>().Knock(true);
			else
				enemy.GetComponent<EnemyIAMovement>().Knock(false);

			
		}
		isAttacking = false;
	}

	void OnTriggerStay2D(Collider2D col){

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
