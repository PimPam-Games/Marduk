using UnityEngine;
using p = PlayerStats;
using System.Collections.Generic;



public class PlatformerCharacter2D : MonoBehaviour
    {

		public static List<GameObject> playerItemsGO = new List<GameObject>();	
		public static bool stopPlayer = false;
        private bool facingRight = true; // For determining which way the player is currently facing.

        public float maxSpeed; // The fastest the player can travel in the x axis.
        [SerializeField] private float jumpForce = 400f; // Amount of force added when the player jumps.	

        [Range(0, 1)] [SerializeField] private float crouchSpeed = .36f;
                                                     // Amount of maxSpeed applied to crouching movement. 1 = 100%

        [SerializeField] private bool airControl = false; // Whether or not a player can steer while jumping;
        [SerializeField] private LayerMask whatIsGround; // A mask determining what is ground to the character
		[SerializeField] private GameObject weapon;
		[SerializeField] private GameObject RangedWeapon;
		public PlayerProjLauncher[] projLaunchers;
		public PlayerProjLauncher bowLauncher;
		private SpellsPanel spellsPanel;

		private Transform backforeArm;
        private Transform groundCheck; // A position marking where to check if the player is grounded.
        private float groundedRadius = .2f; // Radius of the overlap circle to determine if grounded
        private bool grounded = false; // Whether or not the player is grounded.
        private Transform ceilingCheck; // A position marking where to check for ceilings
        private float ceilingRadius = .01f; // Radius of the overlap circle to determine if the player can stand up
        private Animator anim; // Reference to the player's animator component.
		private Rigidbody2D rb;
		private Weapon weaponScript;
		private RangedWeapon rangedWeaponScript;
		public AudioSource attackSound;
		public AudioSource walkGrassSound;
		
		public GameObject bowprojectile; //para setearle la flecha al arco por las dudas que no aparezca
		private float normalAnimSpeed;
		public float knockback = 15f;
		public float knockbackLength = 0.2f;
		private float knockbackTimer = 0;
		private bool knockFromRight = true;
		private float auxxx = 0;
		public static bool jumpNow = false; //para que el jugador salte, se usa para cuando se carga un nivel que entra desde un hueco
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
			if(RangedWeapon != null)
				rangedWeaponScript = RangedWeapon.GetComponent<RangedWeapon> ();
			normalAnimSpeed = anim.speed;
        }

		void Start(){
			spellsPanel = GameObject.Find ("SpellsPanel").GetComponent<SpellsPanel> ();
		}

		void Update(){
			auxxx = 0;
			if (p.isDead)
				maxSpeed = 0;
			else
				maxSpeed = p.utils [p.MovementSpeed];	
			if(stopPlayer){
				rb.velocity = new Vector2(0,0); //esto se usa cuando cambia a la cueva para que no caiga tan rapido
				//stopPlayer = false;
			}
			if (jumpNow) {
				rb.AddForce (new Vector2(0, 900f));
				jumpNow = false;
			}
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
				Flip ();
		}

		public void Attack(){
			if (MyGUI.CharacterWindowOpen () || MyGUI.InventoryOpen ())
				return;
			if (weaponScript == null)
				return;
			if (weaponScript.canAttack) {
				if(PlayerItems.EquipedWeapon == null || PlayerItems.EquipedWeapon.Type == ItemTypes.Weapon){
					attackSound.Play();
					anim.SetBool ("Attacking", true);
				}
				else{
					bowLauncher.projectile = bowprojectile;
					anim.SetBool ("BowAttacking", true);
				}
			}	
		}
			
		public void setAttackAnimSpeed(){ //se llama desde la animacion de ataque para setear la velocidad	
				
			//if(auxxx == 0)
				anim.speed += weaponScript.animSpeed/2;
			//auxxx++;
			//Debug.Log ("+speed: " + weaponScript.animSpeed);
		}

		public void DoDamage(){ //se llama desde la animacion de ataque para que el ataque empiecwe a hacer daño
			weaponScript.Attack ();
		}

		void OnTriggerStay2D(Collider2D col){
			
			if (Input.GetButtonUp("Grab") && !PlayerStats.isDead) {
				GameObject item = col.gameObject;
				if(item == null){
					Debug.Log("es null");
					return;
				}
				if (item.tag == "Item") {
					playerItemsGO.Add(item);
					item.SetActive(false);
					if(PlayerItems.InventoryMaxSize <= PlayerItems.inventoryCantItems)
						return;
					PlayerItems.Inventory.Add (item.GetComponent<Item> ());
					PlayerItems.inventoryCantItems++;
					checkInventory();
				}
				else{
					if(item.tag == "Spell"){
						string itName = item.GetComponent<Item>().Name;
						spellsPanel.AddSpell(itName);
						Destroy(item);
					}
				}
		
			}
		}
		
		void OnTriggerEnter2D(Collider2D col){			
			if (col.gameObject.tag == "CameraStopFollow") {
				CameraController.stopFollow = true;
			}
			if (col.gameObject.tag == "CameraFollow") {
				CameraController.stopFollow = false;
				CameraController.stopFollowX = false;
			}
			if (col.gameObject.tag == "CameraStopX") {
				CameraController.stopFollowX = true;
			}
		}

		private void checkInventory(){ //para que no se dupliquen los putos items
			bool b = true;
			bool c = true;
			bool d = true;
			if (PlayerItems.inventoryCantItems < 2)
				return;
			b = true && PlayerItems.Inventory [PlayerItems.inventoryCantItems - 1].Name == PlayerItems.Inventory [PlayerItems.inventoryCantItems - 2].Name;
			b = true && PlayerItems.Inventory [PlayerItems.inventoryCantItems - 1].Rarity == PlayerItems.Inventory [PlayerItems.inventoryCantItems - 2].Rarity;
			b = true && PlayerItems.Inventory [PlayerItems.inventoryCantItems - 1].Atributes == PlayerItems.Inventory [PlayerItems.inventoryCantItems - 2].Atributes;
			b = true && PlayerItems.Inventory [PlayerItems.inventoryCantItems - 1].Defensives == PlayerItems.Inventory [PlayerItems.inventoryCantItems - 2].Defensives;
			
			if (PlayerItems.inventoryCantItems > 2) {
				c = true && PlayerItems.Inventory [PlayerItems.inventoryCantItems - 1].Name == PlayerItems.Inventory [PlayerItems.inventoryCantItems - 3].Name;
				c = true && PlayerItems.Inventory [PlayerItems.inventoryCantItems - 1].Rarity == PlayerItems.Inventory [PlayerItems.inventoryCantItems - 3].Rarity;
				c = true && PlayerItems.Inventory [PlayerItems.inventoryCantItems - 1].Atributes == PlayerItems.Inventory [PlayerItems.inventoryCantItems - 3].Atributes;
				c = true && PlayerItems.Inventory [PlayerItems.inventoryCantItems - 1].Defensives == PlayerItems.Inventory [PlayerItems.inventoryCantItems - 3].Defensives;
			} else 
				c = false;
			
			if (PlayerItems.inventoryCantItems > 3) {
				d = true && PlayerItems.Inventory [PlayerItems.inventoryCantItems - 1].Name == PlayerItems.Inventory [PlayerItems.inventoryCantItems - 4].Name;
				d = true && PlayerItems.Inventory [PlayerItems.inventoryCantItems - 1].Rarity == PlayerItems.Inventory [PlayerItems.inventoryCantItems - 4].Rarity;
				d = true && PlayerItems.Inventory [PlayerItems.inventoryCantItems - 1].Atributes == PlayerItems.Inventory [PlayerItems.inventoryCantItems - 4].Atributes;
				d = true && PlayerItems.Inventory [PlayerItems.inventoryCantItems - 1].Defensives == PlayerItems.Inventory [PlayerItems.inventoryCantItems - 4].Defensives;
			} else 
				d = false;
			
			if(b || c || d){
				
				PlayerItems.Inventory.RemoveAt (PlayerItems.inventoryCantItems - 1);
				PlayerItems.inventoryCantItems--;
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
					if((anim.GetBool("Attacking") == true || anim.GetBool("BowAttacking") == true) && move != 0)
						rb.velocity = new Vector2(move*(maxSpeed/1.5f), rb.velocity.y);
					else
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
	    
		public  void EnableArrowRender(){
			PlayerItems.arrowRenderer.enabled = true;
		}

		public  void DisableArrowRender(){
			PlayerItems.arrowRenderer.enabled = false;
		}

		public void LaunchArrow(){
			
			if (bowLauncher.projectile == null)
				bowLauncher.projectile = bowprojectile;
			bowLauncher.LaunchProjectile ();
		}

		public void Fall(){ //si el jugador suelta boton de saltar se llama este metodo
			//Debug.Log ("Fall");
			if(rb.velocity.y > 0)
				rb.velocity = new Vector2 (rb.velocity.x,rb.velocity.y/2);
		}

		public void Spell1(){
			if (projLaunchers [0].projectile != null) {
				if (!projLaunchers [0].projectile.GetComponent<PlayerProjStats> ().bowLauncher){
					projLaunchers [0].LaunchProjectile ();
				} else {
					bowLauncher.projectile = projLaunchers [0].projectile;
					if(PlayerItems.EquipedWeapon != null && PlayerItems.EquipedWeapon.Type == ItemTypes.RangedWeapon){
						anim.SetBool ("BowAttacking", true);
					}
				}
			}
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

			anim.speed = normalAnimSpeed;
			//Debug.Log ("-speed: " + weaponScript.animSpeed);

			anim.SetBool ("Attacking",false);
			anim.SetBool ("BowAttacking",false);
			
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
