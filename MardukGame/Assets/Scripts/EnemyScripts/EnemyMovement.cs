using UnityEngine;
using System.Collections;

public class EnemyMovement : MonoBehaviour {


	private bool facingRight = false;
	[SerializeField] private float maxSpeed = 10f;
	private Rigidbody2D rb;
	[SerializeField] private float flipDelay = 2;
	[SerializeField] private bool followPlayer = false;
	private float delayTime;
	public int move = 1;
	private float stopTime;
	private GameObject target;



	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player");
		delayTime = flipDelay;
		rb = GetComponent<Rigidbody2D> ();
	}
	
	// Update is called once per frame
	void Update () {
		stopTime -= Time.deltaTime;

		LookAtPlayer ();
		if (stopTime < 0 && move == 0) {
			GetComponent<Animator>().SetBool("hit",false);
			if(facingRight)
				move = 1;
			else
				move = -1;
		}
		Move ();
	}

	void OnCollisionEnter2D (Collision2D col){
		if (col.gameObject.tag == "Border") {
			if(col.transform.position.x < transform.position.x)
				move = 1;
			else
				move = -1;
		}
	}

	private void Move(){
		if (!followPlayer) {
			if (Time.time > delayTime) {
				move *= -1;
				delayTime = Time.time + flipDelay;
			}
		}
		rb.velocity = new Vector2(move*maxSpeed, rb.velocity.y);
		
		// If the input is moving the player right and the player is facing left...
		if (move > 0 && !facingRight)
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (move < 0 && facingRight)
			// ... flip the player.
			Flip();
	}

	public void StopWalk(float attackTime){
		move = 0;
		stopTime = attackTime;
	}

	public void Knock(float knockTime){
		move = 0;
		stopTime = knockTime;
	}

	public bool IsFacingRight(){
		return facingRight;
	}

	private void LookAtPlayer(){
		if (followPlayer && stopTime<=0) {
			GetComponent<Animator>().SetBool("hit",false);
			var dir = (target.transform.position - transform.position).normalized;
			var dot = Vector2.Dot (dir, transform.right); //negativo si player esta a su izquierda
			if (dot < 0)
				move = -1;
			if (dot > 0)
				move = 1;
		}
	}
	
	private void Flip()
	{
		
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
