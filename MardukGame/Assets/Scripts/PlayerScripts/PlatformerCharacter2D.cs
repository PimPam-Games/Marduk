﻿using UnityEngine;
using p = PlayerStats;
using System.Collections.Generic;



public class PlatformerCharacter2D : MonoBehaviour
    {

		public static List<GameObject> playerItemsGO = new List<GameObject>();	

        private bool facingRight = true; // For determining which way the player is currently facing.

        public float maxSpeed; // The fastest the player can travel in the x axis.
        [SerializeField] private float jumpForce = 400f; // Amount of force added when the player jumps.	

        [Range(0, 1)] [SerializeField] private float crouchSpeed = .36f;
                                                     // Amount of maxSpeed applied to crouching movement. 1 = 100%

        [SerializeField] private bool airControl = false; // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character
		[SerializeField] private GameObject weapon;
		public PlayerProjLauncher[] projLaunchers;

		private Transform backforeArm;
        private Transform groundCheck; // A position marking where to check if the player is grounded.
        private float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool grounded = false; // Whether or not the player is grounded.
        private Transform ceilingCheck; // A position marking where to check for ceilings
        private float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator anim; // Reference to the player's animator component.
		private Rigidbody2D rb;
		private Weapon weaponScript;
		public AudioSource attackSound;
		public AudioSource walkGrassSound;

		
		public float knockback = 2;
		public float knockbackLength = 0.4f;
		private float knockbackTimer = 0;
		private bool knockFromRight = true;
		private int a = 0;

        private void Awake()
        {

            // Setting up references.
            groundCheck = transform.Find("GroundCheck");
            ceilingCheck = transform.Find("CeilingCheck");
            anim = GetComponent<Animator>();
			walkGrassSound.pitch = 1.2f;
			rb = GetComponent<Rigidbody2D> ();
			if(weapon != null)
				weaponScript = weapon.GetComponent<Weapon> ();

        }


		void Update(){
			a = 0;
			if (p.isDead)
				maxSpeed = 0;
			else
				maxSpeed = p.utils [p.MovementSpeed];	
		}

		public void knockBackPlayer(bool knockFromRight){
			knockbackTimer = knockbackLength;
			this.knockFromRight = knockFromRight;
		}
		
        private void FixedUpdate()
        {
            // The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
            grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
            anim.SetBool("Ground", grounded);

            // Set the vertical animation
			
            anim.SetFloat("vSpeed", rb.velocity.y);

        }


		public void RespawnPosition(){ //hace que el jugador mire a la derecha
			if (!facingRight)
				Flip ();;
		}

		public void Attack(){
			if (MyGUI.CharacterWindowOpen () || MyGUI.InventoryOpen ())
				return;
			if (weaponScript == null)
				return;
			if (weaponScript.canAttack) {
				attackSound.Play();
				anim.SetBool ("Attacking", true);
				weaponScript.Attack ();
			}
		}

		void OnTriggerStay2D(Collider2D col){
			a++;
			if (Input.GetButtonUp("Grab") && !PlayerStats.isDead) {
				if(a >1)
					return;
				GameObject item = col.gameObject;
				if(item == null)
					return;
				playerItemsGO.Add(item);
				if (item.tag == "Item") {
					item.SetActive(false);
					if(PlayerItems.InventoryMaxSize <= PlayerItems.inventoryCantItems)
						return;
					PlayerItems.Inventory.Add (item.GetComponent<Item> ());
					PlayerItems.inventoryCantItems++;					
				}
			}
		}

		public bool isFacingRight(){
			return facingRight;
		}

        public void Move(float move, bool crouch, bool jump)
        {
			
			
            // If crouching, check to see if the character can stand up
            if (!crouch && anim.GetBool("Crouch"))
            {
                // If the character has a ceiling preventing them from standing up, keep them crouching
                if (Physics2D.OverlapCircle(ceilingCheck.position, ceilingRadius, whatIsGround))
                    crouch = true;
            }

            // Set whether or not the character is crouching in the animator
            anim.SetBool("Crouch", crouch);

            //only control the player if grounded or airControl is turned on
		//Debug.Log (grounded);
		//Debug.Log (Mathf.Abs(move));
            if (grounded || airControl)
            {
                // Reduce the speed if crouching by the crouchSpeed multiplier
                move = (crouch ? move*crouchSpeed : move);

                // The Speed animator parameter is set to the absolute value of the horizontal input.
                anim.SetFloat("Speed", Mathf.Abs(move));

				if(!walkGrassSound.isPlaying && anim.GetFloat("Speed") > 0 && grounded)
					walkGrassSound.Play();
				if(walkGrassSound.isPlaying && (anim.GetFloat("Speed") == 0 || !grounded))
					walkGrassSound.Stop();
                // Move the character
				if(knockbackTimer <= 0){
                	rb.velocity = new Vector2(move*maxSpeed, rb.velocity.y);
				}
				else{
					anim.SetFloat("Speed", 0);
					if(!PlayerStats.isDead){
							if(knockFromRight)
								rb.velocity = new Vector2(-knockback, rb.velocity.y);
							else
								rb.velocity = new Vector2(knockback, rb.velocity.y);
					}
					knockbackTimer -= Time.deltaTime;
				}
                // If the input is moving the player right and the player is facing left...
                if (move > 0 && !facingRight)
                    // ... flip the player.
                    Flip();
                    // Otherwise if the input is moving the player left and the player is facing right...
                else if (move < 0 && facingRight)
                    // ... flip the player.
                    Flip();
            }
            // If the player should jump...
            if (grounded && jump && anim.GetBool("Ground"))
            {
                // Add a vertical force to the player.
				
                grounded = false;
                anim.SetBool("Ground", false);
				rb.velocity = new Vector2(rb.velocity.x,0);
                rb.AddForce(new Vector2(0f, jumpForce));
            }
        }

		public void Fall(){ //si el jugador suelta boton de saltar se llama este metodo
			//Debug.Log ("Fall");
			if(rb.velocity.y > 0)
				rb.velocity = new Vector2 (rb.velocity.x,rb.velocity.y/2);
		}

		public void Spell1(){
			if(projLaunchers[0].projectile != null)
				projLaunchers[0].LaunchProjectile ();
		}
		
		public void Spell2(){
			if(projLaunchers[1].projectile != null)
				projLaunchers[1].LaunchProjectile ();
		}	

		public void Spell3(){
			if(projLaunchers[2].projectile != null)
				projLaunchers[2].LaunchProjectile ();
		}

		public void Spell4(){
			if(projLaunchers[3].projectile != null)
				projLaunchers[3].LaunchProjectile ();
		}
		public void Idle(){
			anim.SetBool ("Attacking",false);
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
