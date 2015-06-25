using UnityEngine;
using System.Collections;

public class EnemyRangedAttack : MonoBehaviour {

	//public GameObject projectile;
	[SerializeField] Animator anim;
	[SerializeField] private float castDelay = 3f;
	private GameObject target;
	private float castTimer =0;
	public bool stopWalkWhenAttack = false;
	//[SerializeField] private float offsetY = 0; // offset para cambiar la posicion del eje y
	[SerializeField] private float maxDistance = 999; //distancia maxima que tiene que estar player para que comience a atacar
	private EnemyStats stats;
	private EnemyIAMovement movement;
	//public bool multipleProjectiles;
	public GameObject[] pLaunchers;

	
	// Use this for initialization
	void Start () {
		movement = GetComponent<EnemyIAMovement> ();
		target = GameObject.FindGameObjectWithTag ("Player");		
		stats = GetComponent<EnemyStats> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (stats.isDead)
			return;
		castTimer -= Time.deltaTime;
		float distance = Vector3.Distance (target.transform.position, transform.position);
		if (castTimer < 0 && distance <= maxDistance) {
			castTimer = castDelay;
			anim.SetBool ("Attacking", true);
			if(stopWalkWhenAttack)
				movement.StopWalk();
		}
	}

	public void StopAttackAmin(){
		anim.SetBool ("Attacking", false);
		if (stopWalkWhenAttack)
			movement.Walk ();
	}

	private void RangedAttack(){
		for (int i = 0; i<pLaunchers.Length; i++) {
			pLaunchers[i].GetComponent<ProjectileLauncher>().LaunchProjectile(target);
		}
	}
}
