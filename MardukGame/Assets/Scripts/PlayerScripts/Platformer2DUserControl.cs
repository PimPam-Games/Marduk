﻿using UnityEngine;



	public class Platformer2DUserControl : MonoBehaviour
    {
        private PlatformerCharacter2D character;
        private bool jump;

        private bool jumpPressed = false, movePressed = false;

        private void Awake()
        {
            character = GetComponent<PlatformerCharacter2D>();
			
        }

        private void Update()
        {
            if (!Fading.loaded)
                return;
            if(movePressed && jumpPressed)
            {
                TutorialController.moveTutorialOn = false;
                TutorialController.moveTutorialShowed = true;
            }
			if (!PlayerStats.isDead && !IntroText.introVisible) {
				if (!jump)
					jump = Input.GetButtonDown ("Jump");
                if (Input.GetButtonUp("Jump"))
                {
                    character.Fall();
                    jumpPressed = true;
                }

							
			}
        }

    private void FixedUpdate()
    {
        if (!Fading.loaded)
            return;
        if (!PlayerStats.isDead && !IntroText.introVisible) {
			// Read the inputs.
			bool crouch = Input.GetKey (KeyCode.S);
			/*bool crouch = false;
			if(Input.GetAxis ("Vertical") < 0){
				crouch = true;
			}*/
			
			float h = Input.GetAxis ("Horizontal");
            if(h != 0)
            {
                movePressed = true;
            }
			// Pass all parameters to the character control script.
			if(PlatformerCharacter2D.skillBtnPressed > 0) // si esta pulsando un boton de habilidad no se puede mover
				h=0;
			character.Move (h, crouch, jump);
			jump = false;

            if (InputControllerGui.noAttack)
                return;
			if(Input.GetButton("Spell1"))
                character.Spell1();
			if(Input.GetButton("Spell2"))
				character.Spell2();
			if(Input.GetButton("Spell3"))
				character.Spell3();
			if(Input.GetButton("Spell4"))
				character.Spell4();		
			/*if (Input.GetButton ("Fire1")) {	
				PlatformerCharacter2D.meleeSkillPos = -1; //es el ataque comun	
				PlatformerCharacter2D.supportSkillPos = -1; 
				PlatformerCharacter2D.useMeleeProjLauncher = false;
				character.Attack ();
			}*/
		} else
			character.Move (0, false, false);
        }
	}
