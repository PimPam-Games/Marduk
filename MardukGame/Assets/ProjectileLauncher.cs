using UnityEngine;
using System.Collections;

public class ProjectileLauncher : MonoBehaviour {

	public Vector2 force;
	public GameObject projectile;
	public Vector2 velocity;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LaunchProjectile(){
		GameObject p = (GameObject)Instantiate (projectile, transform.position, transform.rotation);
		p.GetComponent<Rigidbody2D> ().AddForce (force);
	}
}
