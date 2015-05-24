using UnityEngine;
using System.Collections;

public class EnemyAttack : MonoBehaviour {

	
	public bool hasMeleeAttack;
	private float attackTimer = 0;
	[SerializeField] private float attackDelay = 3f;
	[SerializeField] private Animator anim;
	[SerializeField] private GameObject target;
	private PlayerStats playerStats;
	private EnemyMovement movement;
	private EnemyStats stats;

	void Awake(){

	}
	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player");
		movement = GetComponent<EnemyMovement> ();
		playerStats = target.GetComponent<PlayerStats> ();
		stats = GetComponent<EnemyStats> ();
	}
	
	// Update is called once per frame
	void Update () {
		attackTimer -= Time.deltaTime;
		if (hasMeleeAttack) {
			MeleeAttackAI ();
		} else {
			if(attackTimer <= 0){
				attackTimer = attackDelay;
				anim.SetBool("Attacking", true);
			}

		}
	}


	void MeleeAttackAI(){
		var dir = (target.transform.position - transform.position).normalized;
		var dot = Vector2.Dot(dir, transform.right); //negativo si player esta a su izquierda
		float distance = Vector3.Distance (target.transform.position, transform.position);
		if (distance < 1.5 && attackTimer <= 0 ) {
			if((dot < 0 && !movement.IsFacingRight()) || (dot > 0 && movement.IsFacingRight())){
				attackTimer = attackDelay;
				anim.SetBool("Attacking", true);
				movement.StopWalk(0.5f);
				float damage = Random.Range(stats.damage.First,stats.damage.Second);
				playerStats.Hit(damage,stats.elem); 
			}
		}
	}



	void Idle(){
		anim.SetBool("Attacking", false);
	}
}
