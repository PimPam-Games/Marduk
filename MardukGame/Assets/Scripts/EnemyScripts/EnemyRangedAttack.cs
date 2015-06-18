using UnityEngine;
using System.Collections;

public class EnemyRangedAttack : MonoBehaviour {

	public GameObject projectile;
	[SerializeField] Animator anim;
	[SerializeField] private float castDelay = 3f;
	private GameObject target;
	private float castTimer =0;
	//[SerializeField] private float offsetY = 0; // offset para cambiar la posicion del eje y
	[SerializeField] private float maxDistance = 999; //distancia maxima que tiene que estar player para que comience a atacar
	private EnemyStats stats;
	public bool multipleProjectiles;
	public GameObject[] pLaunchers;

	
	// Use this for initialization
	void Start () {
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
		}
	}

	public void StopAttackAmin(){
		anim.SetBool ("Attacking", false);
	}

	public void MultipleRangedAttack(){
		for (int i = 0; i<pLaunchers.Length; i++) {
			pLaunchers[i].GetComponent<ProjectileLauncher>().LaunchProjectile();
		}
	}

	private void RangedAttack(){
		//float distance = Vector3.Distance (target.transform.position, transform.position);
		//if (castTimer < 0 && distance <= maxDistance) {
	/*	for (int i = 0; i<pLaunchers.Length; i++) {
			pLaunchers[i].GetComponent<ProjectileLauncher>().LaunchProjectile();
		}*/	
			var dir = (target.transform.position - transform.position).normalized;
			var dot = Vector2.Dot(dir, transform.right);
			if(dot < 0)
				projectile.GetComponent<EnemyMovement>().move = -1;
			else
				projectile.GetComponent<EnemyMovement>().move = 1;
			anim.SetBool("Attacking",true);
			Instantiate(projectile, pLaunchers[0].transform.position, transform.rotation);
		//}
	}
}
