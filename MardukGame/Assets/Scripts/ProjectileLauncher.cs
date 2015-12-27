using UnityEngine;
using System.Collections;

public class ProjectileLauncher : MonoBehaviour {

	public Vector2 force; //fuerza que se le aplica al proyectil cuando se crea
	public GameObject projectile;
	public bool toTargetDir; // si debe apuntar a la direccion del objetivo o no
	public bool staticProjectile; // si el ataque sigue al jugador que lo tira, ej: un lanzallamas
	public EnemyIAMovement ia;
	public bool flipProjectile = false;
	private GameObject p;
	public EnemyStats stats;
	PlayerProjStats projStats = null;
	public bool dontChangeRotation = false;
	
	// Use this for initialization
	void Awake () {
		projStats = projectile.GetComponent<PlayerProjStats>();
		//projStats.fromEnemy = true;
		//projStats.enemyStats = stats;
	}

	// Update is called once per frame
	void Update () {
		if(staticProjectile && p != null)
			p.transform.position = this.transform.position;
	}

	public void SetDamage(float minDmg, float maxDmg){
		projStats.minDmg = minDmg;
		projStats.maxDmg = maxDmg;
	}

	public void LaunchProjectile(GameObject target){
		if(!dontChangeRotation)
			p = (GameObject)Instantiate (projectile, transform.position, transform.rotation);
		else{
			//proj = (GameObject)Instantiate (projectile, transform.position, Quaternion.Euler(0,0,0));
			p = (GameObject)Instantiate (projectile, transform.position, projectile.transform.rotation);
		}
		p.GetComponent<PlayerProjStats> ().enemyStats = stats;
		p.GetComponent<PlayerProjStats> ().fromEnemy = true;
		if (toTargetDir && target != null) {
			if(target.transform.position.x < transform.position.x){
				p.GetComponent<ProjectileMovement>().moveDirX= -1;
				p.transform.rotation = Quaternion.Euler(0,0,-90);
			}
			else
				p.GetComponent<ProjectileMovement>().moveDirX = 1;
		}
		if (flipProjectile && ia.facingRight)
			p.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (force.x * -1, force.y));
		else
			p.GetComponent<Rigidbody2D> ().AddForce (force);
	}
}
