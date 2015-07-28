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
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		if(staticProjectile && p != null)
			p.transform.position = this.transform.position;
	}

	public void LaunchProjectile(GameObject target){
		p = (GameObject)Instantiate (projectile, transform.position, transform.rotation);
		if (toTargetDir && target != null) {
			/*var dir = (target.transform.position - transform.position).normalized;
			var dot = Vector2.Dot(dir, transform.right);*/

			if(target.transform.position.x < transform.position.x)
				p.GetComponent<ProjectileMovement>().moveDirX= -1;
			else
				p.GetComponent<ProjectileMovement>().moveDirX = 1;
		}
		if (flipProjectile && ia.facingRight)
			p.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (force.x * -1, force.y));
		else
			p.GetComponent<Rigidbody2D> ().AddForce (force);
	}
}
