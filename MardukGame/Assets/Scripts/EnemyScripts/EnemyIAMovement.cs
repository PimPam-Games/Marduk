using UnityEngine;
using System.Collections;

public class EnemyIAMovement : MonoBehaviour {


	public int MaxDistanceFollow = 11;
	public float maxSpeed = 2f;
	public float initMaxSpeed;
	public bool facingRight = false;
	public float currentSpeed;
	public Rigidbody2D rb;
	private GameObject target;
	public Transform[] groundChecks;
	private bool isFollowingPlayer;
	private bool grounded;
	private float groundedRadius = .2f;
	public float jumpForce = 800f;
	private float flipDelay = 1.5f ;
	private float flipDelayCount = 0;
	private int moveDir = 1, moveDirY;

	private float stopTime = 0;
	private Animator anim;
	private EnemyStats enemyStats;

	public bool knockable = true;
	public bool jumper = false; 
	public bool flying = false; //true si el enemigo es volador
	public bool common = true; // enemigo terrestre comun
	public bool hasIdleInstance = false;

	private float jumpTime = 0;
	public float jumpDelay = 2;
	private float groundCheckTime = 0;

	public float knockbackLength = 0.3f;
	private float knockbackTimer = 0;
	private bool knockFromRight = true;
	public float knockback = 2.5f;

	private Vector3 dir;
	private float dotX,dotY;
	private float calcDirTime, calcDirDelay = 0.6f;
	public bool horizontalFly = false;
	public bool smartFly = false;
	private float upFlyTimer = 0;
	private float followPlayerTimer =0; 
	public bool upFly = false;
	private const float HFlyC = 10; 
	private float hflyTimer = 0;
	
	public bool dontFlip = false;

	[SerializeField] private LayerMask whatIsGround;

	// Use this for initialization
	void Start () {
		hflyTimer = HFlyC;
		initMaxSpeed = maxSpeed;
		target = GameObject.FindGameObjectWithTag ("Player");
		rb = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
		enemyStats = GetComponent<EnemyStats> ();
		currentSpeed = maxSpeed;
		if (hasIdleInstance)
			anim.SetFloat ("Speed", currentSpeed);
	}

	private void FixedUpdate()
	{
		bool g = false;
		foreach (Transform groundCheck in groundChecks) {
			if (Physics2D.OverlapCircle (groundCheck.position, groundedRadius, whatIsGround)){
				g = true;
				break;
			}
		}
		grounded = g;
	}

	// Update is called once per frame
	void Update () {

		if (enemyStats.isDead) {
			rb.velocity = new Vector2 (0, rb.velocity.y);
			return;
		}
		stopTime -= Time.deltaTime;

		if(knockbackTimer>0){
			knocking();
			return;
		}
		if (stopTime <= 0) { // time the enemy will be stopped
			FollowPlayer ();
		} else {
			rb.velocity = new Vector2(0, rb.velocity.y);
		}
	
		if(smartFly){ //solo para el zu por ahora
			hflyTimer -= Time.deltaTime;  //comienza moviendose horizontalmente
			followPlayerTimer -= Time.deltaTime;
			upFlyTimer -= Time.deltaTime;
			if(hflyTimer <= 0 && followPlayerTimer <= 0 && horizontalFly && !upFly){ // empieza a seguir al player
				followPlayerTimer = HFlyC;
				horizontalFly = false;
			}			
			if(!horizontalFly && followPlayerTimer <= 0 && !upFly){ //deja de seguirlo, empieza a subir
				horizontalFly = true;
				upFly = true;
				upFlyTimer = 0.85f;
				Debug.Log("comienza a subir");
			}
			if(upFlyTimer <= 0 && upFly && horizontalFly){ //deja de subir, se muve horizontalmente
				horizontalFly = true;
				upFly = false;
				hflyTimer = HFlyC;
				Debug.Log("deja de subir");
				
			}
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
		if (!hasIdleInstance) {
			flipDelayCount -= Time.deltaTime;
			if (flipDelayCount <= 0) {
				moveDir *= -1;
				flipDelayCount = flipDelay;
			}
		} else {
			currentSpeed = 0;
			anim.SetFloat("Speed",currentSpeed);
		}
		Move ();
	}

	private void FlyPatrol(){
		flipDelayCount -= Time.deltaTime;
		moveDirY = 0;
		if (flipDelayCount <= 0) {
			moveDir *= -1;
			flipDelayCount = flipDelay;
		}
		Fly ();
	}

	private void jumpPatrol(){
		anim.SetBool ("Ground",grounded);
		groundCheckTime -= Time.deltaTime;
		if (groundCheckTime <= 0 && !common)
			if(grounded)
				rb.velocity = new Vector2 (0,rb.velocity.y);
		if (jumpTime <= 0)
			if (grounded) {
				rb.AddForce (new Vector2 (moveDir * 100, jumpForce));
				jumpTime = jumpDelay;
				groundCheckTime = 0.5f;
			}
		jumpTime -= Time.deltaTime;
		// If the input is moving the player right and the player is facing left...
		if (moveDir > 0 && !facingRight) 
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (moveDir < 0 && facingRight)
			// ... flip the player.
			Flip();
	}

	private void FollowPlayer(){
		float dist = Vector2.Distance (new Vector2(target.transform.position.x,target.transform.position.y),new Vector2(this.transform.position.x,this.transform.position.y));
		 // calcula la direccion donde esta el jugador respecto del enemigo
		if (common) {
			if (dist < MaxDistanceFollow){
				CalculateDir ();

				if(hasIdleInstance){
					currentSpeed = maxSpeed;
					anim.SetFloat("Speed",currentSpeed);
				}
				Move ();
			}
			else
				Patrol();
				//rb.velocity = new Vector2 (0, rb.velocity.y); //aca hay que poner la animacion en idle
		} 
		if(jumper){
			if (dist < MaxDistanceFollow){
				CalculateDir ();
				jumpPatrol();
			}
			else
				rb.velocity = new Vector2 (0, rb.velocity.y);
		}
		if (flying) {
			if (dist < MaxDistanceFollow){
				CalculateDir ();
				Fly();
			}
			else
				FlyPatrol();
		}
	}



	private void Fly(){
		if (knockbackTimer <= 0) {
			anim.SetBool ("hit", false);
			if(!horizontalFly)
				rb.velocity = new Vector2 (moveDir * maxSpeed, moveDirY * maxSpeed);
			else{
				if(!upFly)
					rb.velocity = new Vector2 (moveDir * maxSpeed, 0 * maxSpeed);
				else{
					rb.velocity = new Vector2 (moveDir * maxSpeed, maxSpeed);
				}
			}
		}
		else{
			if(knockable){
				if(knockFromRight)
					rb.velocity = new Vector2(-knockback, rb.velocity.y);
				else
					rb.velocity = new Vector2(knockback, rb.velocity.y);
				knockbackTimer -= Time.deltaTime;
			}
			return;
		}
		
		// If the input is moving the player right and the player is facing left...
		if (moveDir > 0 && !facingRight) 
			// ... flip the player.
			Flip();
		// Otherwise if the input is moving the player left and the player is facing right...
		else if (moveDir < 0 && facingRight)
			// ... flip the player.
			Flip();
	}

	private void CalculateDir(){
		calcDirTime -= Time.deltaTime;
		if (calcDirTime < 0) {
			calcDirTime = calcDirDelay;
			dir = (target.transform.position - transform.position).normalized;
			dotX = Vector2.Dot (dir, transform.right); //negativo si player esta a su izquierda
			dotY = Vector2.Dot(dir,transform.up); //negativo si player esta arriba?
			if (dotX < 0)
				moveDir = -1;
			else
				moveDir = 1;
			if(dotY < 0)
				moveDirY = -1;
			else
				moveDirY = 1;
		}
	}

	private void knocking(){
		if (knockbackTimer <= 0) {
			if(knockable)
				anim.SetBool ("hit", false);
		}
		else{
			if(knockable){
				if(knockFromRight)
					rb.velocity = new Vector2(-knockback, rb.velocity.y);
				else
					rb.velocity = new Vector2(knockback, rb.velocity.y);
				knockbackTimer -= Time.deltaTime;
			}
			return;
		}
	}

	private void Move(){ //move the enemy

		rb.velocity = new Vector2 (moveDir * currentSpeed, rb.velocity.y);
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
		if (dontFlip)
			return;
		// Switch the way the player is labelled as facing.
		facingRight = !facingRight;
		
		// Multiply the player's x local scale by -1.
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	/* Public Methods */

	public void StopWalk(){

		stopTime = 999999;
	}

	public void Walk(){
		stopTime = 0;
	}

	public void setKnockbackTimer(float knockTimer){
		this.knockbackTimer = knockTimer;
	}


	public void Knock(bool knockFromRight){
		//anim.SetBool("hit",true);
		//stopTime = 0;
		if (knockable) {
			knockbackTimer = knockbackLength;
			this.knockFromRight = knockFromRight;
		}
	}

	public bool IsFacingRight(){
		return facingRight;
	}
}
