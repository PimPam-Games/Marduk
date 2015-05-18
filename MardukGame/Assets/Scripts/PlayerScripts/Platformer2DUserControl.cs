using UnityEngine;



	public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D character;
        private bool jump;

        private void Awake()
        {
            character = GetComponent<PlatformerCharacter2D>();
			
        }

        private void Update()
        {
			if (!PlayerStats.isDead) {
				if (Input.GetButtonUp ("Grab"))
					character.Grab ();

				

					if (!jump)
				        // Read the jump input in Update so button presses aren't missed.
						jump = Input.GetButtonDown ("Jump");
			}

        }

        private void FixedUpdate()
        {
			
		if (!PlayerStats.isDead) {
			// Read the inputs.
			bool crouch = Input.GetKey (KeyCode.LeftControl);
			float h = Input.GetAxis ("Horizontal");
			// Pass all parameters to the character control script.
			character.Move (h, crouch, jump);
			jump = false;
			
			if (Input.GetButton ("Fire1")) {
				character.Attack ();
					
			}
		} else
			character.Move (0, false, false);
        }
	}
