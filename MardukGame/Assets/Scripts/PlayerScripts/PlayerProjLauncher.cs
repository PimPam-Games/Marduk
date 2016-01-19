using UnityEngine;
using System.Collections;
using p = PlayerStats;
using pc = PlatformerCharacter2D;

public class PlayerProjLauncher : MonoBehaviour {

	public Vector2 force; //fuerza que se le aplica al proyectil cuando se crea
	public GameObject projectile;
	public bool staticProjectile;
	public bool flipProjectile = false;
	public bool dontChangeRotation = false;
	public float minDmg = 0;
	public float maxDmg = 0;
	public Support supportSkill;
	//public PlatformerCharacter2D character;
	private GameObject proj; //el proyectil
	//public float castDelay = 0;
	//public float castDelayCount = 0;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		if(staticProjectile && proj != null){
			if (pc.isFacingRight ())
				proj.transform.position = new Vector3(this.transform.position.x - 0.4f,this.transform.position.y,this.transform.position.z);
			else	
				proj.transform.position = new Vector3(this.transform.position.x + 0.4f,this.transform.position.y,this.transform.position.z);
		}
	}

	public void LaunchProjectile(){
		proj = null;
		if(!dontChangeRotation)
			proj = (GameObject)Instantiate (projectile, transform.position, transform.rotation);
		else{
			//proj = (GameObject)Instantiate (projectile, transform.position, Quaternion.Euler(0,0,0));
			proj = (GameObject)Instantiate (projectile, transform.position, projectile.transform.rotation);
		}
		proj.GetComponent<PlayerProjStats>().minDmg = minDmg;
		proj.GetComponent<PlayerProjStats>().maxDmg = maxDmg;
		proj.GetComponent<PlayerProjStats>().supportSkill = supportSkill;
		if (!flipProjectile) {	
			if (pc.isFacingRight ())
				proj.GetComponent<ProjectileMovement> ().moveDirX = 1;
			else{
				proj.GetComponent<ProjectileMovement> ().moveDirX = -1;
				proj.transform.rotation = Quaternion.Euler(0,0,-90);
			}
		}
		if (flipProjectile && pc.isFacingRight ()) {
			proj.GetComponent<Rigidbody2D> ().AddForce (new Vector2 (force.x * -1, force.y));
			if(!dontChangeRotation)
				proj.transform.rotation = Quaternion.Euler (0, 0, 90);
		} else {
			proj.GetComponent<Rigidbody2D> ().AddForce (force);
			if(!dontChangeRotation)
				proj.transform.rotation = Quaternion.Euler(0,0,-90);
		}
	}
}
