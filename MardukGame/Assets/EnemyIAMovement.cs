﻿using UnityEngine;
using System.Collections;

public class EnemyIAMovement : MonoBehaviour {

	public float maxSpeed = 2f;
	private bool facingRight = false;
	private Rigidbody2D rb;
	private GameObject target;
	public Transform groundCheck;
	public bool smartFollow;
	private bool isFollowingPlayer;
	private bool grounded;
	private float groundedRadius = .2f;
	private float jumpForce = 800f;
	public float flipDelay = 888f ;
	private float flipDelayCount = 0;
	public int moveDir = 1;
	private float stopTime = 0;
	private Animator anim;

	[SerializeField] private LayerMask whatIsGround;

	// Use this for initialization
	void Start () {
		target = GameObject.FindGameObjectWithTag ("Player");
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}

	private void FixedUpdate()
	{
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundedRadius, whatIsGround);
	}

	// Update is called once per frame
	void Update () {
		stopTime -= Time.deltaTime;

		if (stopTime <= 0) { // time the enemy will be stopped
			anim.SetBool("hit",false);
			if (smartFollow)
				FollowPlayer ();
			else
				Patrol ();
		} else {
			rb.velocity = new Vector2(0, rb.velocity.y);
		}
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.tag == "BorderJump") {
			EnemyJump();
		}
		if (col.gameObject.tag == "BorderFlip") {
			flipDelayCount = 0;
		}
	}

	private void EnemyJump () {
		if(grounded){
			rb.AddForce (new Vector2(0,jumpForce));
		}

	}

	private void Patrol(){

		flipDelayCount -= Time.deltaTime;
		if (flipDelayCount <= 0) {
			moveDir *= -1;
			flipDelayCount = flipDelay;
		}
		Move ();
	}

	private void FollowPlayer(){

		var dir = (target.transform.position - transform.position).normalized;
		var dot = Vector2.Dot (dir, transform.right); //negativo si player esta a su izquierda
		if (dot < 0)
			moveDir = -1;
		if (dot > 0)
			moveDir = 1;
		Move ();
	}
	

	private void Move(){ //move the enemy
		rb.velocity = new Vector2(moveDir*maxSpeed, rb.velocity.y);
		
		// If the input is moving the player right and the player is facing left...
		if (moveDir > 0 && !facingRight) 
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (moveDir < 0 && facingRight)
			// ... flip the player.
			Flip();
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

	/* Public Methods */

	public void StopWalk(float attackTime){
		stopTime = attackTime;
	}
	
	public void Knock(float knockTime){
		anim.SetBool("hit",true);
		stopTime = knockTime;
	}

	public bool IsFacingRight(){
		return facingRight;
	}
}
