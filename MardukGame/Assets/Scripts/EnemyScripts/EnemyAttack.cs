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


	void Awake(){

	}
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
				movement.StopWalk(99);
			}
		}
	}

	void MeleeAttack(){
		var dir = (target.transform.position - transform.position).normalized;
		var dot = Vector2.Dot(dir, transform.right); //negativo si player esta a su izquierda
		float distance = Vector3.Distance (target.transform.position, transform.position);
		if (distance < attackRange)
			if ((dot < 0 && !movement.IsFacingRight ()) || (dot > 0 && movement.IsFacingRight ())) {
				float damage = Random.Range (stats.damage.First, stats.damage.Second);
				playerStats.Hit (damage, stats.elem); 
			}
	}

	void Idle(){
		movement.Walk ();
		anim.SetBool("Attacking", false);
	}
}
