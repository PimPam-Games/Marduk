using UnityEngine;
using System.Collections;

public class ProjectileLauncher : MonoBehaviour {

	public Vector2 force; //fuerza que se le aplica al proyectil cuando se crea
	public GameObject projectile;
	public bool toTargetDir; // si debe apuntar a la direccion del objetivo o no

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LaunchProjectile(GameObject target){
		GameObject p = (GameObject)Instantiate (projectile, transform.position, transform.rotation);
		if (toTargetDir && target != null) {
			/*var dir = (target.transform.position - transform.position).normalized;
			var dot = Vector2.Dot(dir, transform.right);*/

			if(target.transform.position.x < transform.position.x)
				p.GetComponent<ProjectileMovement>().moveDirX= -1;
			else
				p.GetComponent<ProjectileMovement>().moveDirX = 1;
		}
		p.GetComponent<Rigidbody2D> ().AddForce (force);
	}
}
