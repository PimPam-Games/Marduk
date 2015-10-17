using UnityEngine;
using System.Collections;
using p = PlayerStats;

public class PlayerProjLauncher : MonoBehaviour {

	public Vector2 force; //fuerza que se le aplica al proyectil cuando se crea
	public GameObject projectile;
	public bool staticProjectile;
	public bool flipProjectile = false;
	public PlatformerCharacter2D character;
	private GameObject proj; //el proyectil
	//public float castDelay = 0;
	//public float castDelayCount = 0;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(staticProjectile && proj != null){
			if (character.isFacingRight ())
				proj.transform.position = new Vector3(this.transform.position.x - 0.4f,this.transform.position.y,this.transform.position.z);
			else	
				proj.transform.position = new Vector3(this.transform.position.x + 0.4f,this.transform.position.y,this.transform.position.z);
		}
	}

	public void LaunchProjectile(){
		//if (castDelayCount > 0)
		//	return;
		/*if (p.currentMana < projectile.GetComponent<PlayerProjStats> ().manaCost)
			return;*/
		proj = (GameObject)Instantiate (projectile, transform.position, transform.rotation);
		//castDelayCount = castDelay;
			/*var dir = (target.transform.position - transform.position).normalized;
			var dot = Vector2.Dot(dir, transform.right);*/
		if (!flipProjectile) {	
			if (character.isFacingRight ())
				proj.GetComponent<ProjectileMovement> ().moveDirX = 1;
			else
				proj.GetComponent<ProjectileMovement> ().moveDirX = -1;
		}
		if (flipProjectile && character.isFacingRight ()) {
			proj.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (force.x * -1, force.y));
			proj.transform.rotation = Quaternion.Euler (0, 0, 90);
		} else {
			proj.GetComponent<Rigidbody2D> ().AddForce (force);
			proj.transform.rotation = Quaternion.Euler(0,0,-90);
		}
	}
}
