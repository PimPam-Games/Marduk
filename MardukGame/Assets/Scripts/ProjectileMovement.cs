using UnityEngine;
using System.Collections;

public class ProjectileMovement : MonoBehaviour {

	public Vector2 maxSpeed;
	public int moveDirX = 1;
	public int moveDirY = 0;
	private Rigidbody2D rb;

	// Use this for initialization
	void Start () {
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		rb.velocity = new Vector2(maxSpeed.x * moveDirX, maxSpeed.y * moveDirY);
	}
}
