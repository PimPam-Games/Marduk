using UnityEngine;
using System.Collections;
using System;
using pi = PlayerItems;
using expUi = ExpUiController;
using plat = PlatformerCharacter2D;

public class PlayerStats : MonoBehaviour {


	public const int CantAtributes = 4, CantOffensives = 17, CantDefensives = 14, CantUtils = 4;
	public const int Strength = 0, Dextery = 1, Vitality = 2, Spirit = 3; //atributes
	public const int MinDmg = 0, MaxDamge = 1 ,CritChance = 2, CritDmgMultiplier = 3, Accuracy = 4, StunChance = 5, BleedChance = 6, CertainStrChance = 7, ManaPerSec = 8, MaxMana = 9, BaseAttacksPerSecond = 10, IncreasedAttackSpeed = 11, IncreasedCritChance = 12, IncreasedDmg = 13, IncreasedMgDmg = 14, IncreasedCastSpeed = 15, IncreasedAccuracy = 16; //offensives
	public const int MaxHealth = 0 ,Defense = 1, ColdRes = 2, FireRes = 3, LightRes = 4, PoisonRes = 5, BlockChance = 6, Evasiveness = 7, Thorns = 8, LifePerHit = 9, LifePerSecond = 10, AllRes = 11, IncreasedEvasion = 12, IncreasedDefense = 13;  //defensives
	public const int MovementSpeed = 0, IncreasedMoveSpeed = 1, MagicFind = 2, AllAttr = 3;//utils

	public const float InitMoveSpeed = 5;
	public const float InitMaxHealth = 45;
	public const float InitAttacksPerSecond = 1;
	public const float InitMinDmg = 1;
	public const float InitMaxDmg = 2;
	public const float InitMgDmg = 1;
	public const float InitMana = 40; 
	public const float InitManaRegen = 1.75f;
	public const float InitCritChance = 0.05f;
	public const float InitCritDmgMult = 2f;
	public const float InitAccuracy = 50;
	public const float InitEvasion = 55;

	public static float currentHealth;
	public static float currentMana;
	public static bool isDead;
	public bool readyToRespawn = false;
	public static float[] atributes;
	public static float[] offensives;
	public static float[] defensives;
	public static float[] utils;

	private Animator anim;
	private PlayerUIController ui;


	public static double currentExp;
	public static int lvl;
	public static int passivePoints;
	public static double nextLevelExp;
	public static double oldNextLevelExp;
	public static int atributesPoints = 0; //puntos de atributos que quedan por poner cada vez que se pasa de nivel
	public static int strAddedPoints = 0;
	public static int vitAddedPoints = 0;
	public static int spiAddedPoints = 0;
	public static int dexAddedPoints = 0;

	public AudioSource playerDeathSound;
	public AudioSource blockSound;
	public AudioSource levelUpSound	;

	private static AudioSource levelUpSoundStatic;
	
	public static string playerName;
	
	public GameObject graphics;
	private SpriteRenderer[] renders;

	/*status ailments variables*/

	public static bool ghostMode;
	public float ghostModeTime = 1f;
	private float ghostModeCount;

	private bool chill = false;
	private bool freeze = false;
	private float chillTimer = 1f;
	private float chillCount = 0;

	private bool poisoned = false;
	private float poisonedTimer = 2f;
	private float poisonedCount = 0;
	private float poisonedDmg = 0f;

	private bool ignited = false;
	private float ignitedCount = 0;
	private float ignitedTime = 4f; //4 segundos
	private float ignitedDmg = 0;

	private float shockTimer = 2f;
	private float shockCount = 0; 
	private bool shock = false; 

	private float initAnimSpeed;
	public static float currentAnimSpeed; // guarda la velocidad de movimiento actual, es para usar en Weapon



	void Awake(){
		atributes = new float[CantAtributes];
		offensives = new float[CantOffensives];
		defensives = new float[CantDefensives];
		utils = new float[CantUtils];
		anim = GetComponent<Animator> ();
		initAnimSpeed = anim.speed;
		currentAnimSpeed = anim.speed;
	}

	// Use this for initialization
	void Start () {
		intialStats ();
		isDead = false;
		ui = this.GetComponent<PlayerUIController> ();
		renders = graphics.GetComponentsInChildren<SpriteRenderer> ();
		levelUpSoundStatic = levelUpSound;
		currentHealth = defensives [MaxHealth];
		currentMana = offensives [MaxMana];
		UpdateMana ();
		StartCoroutine(LifeRegeneration());
		StartCoroutine (ManaRegeneration());
		//StartCoroutine (GhostModeRender());
	}
	
	// Update is called once per frame
	void Update () {
		ghostModeCount -= Time.deltaTime;
		if (ghostModeCount <= 0 && ghostMode) {
			ghostMode = false;

			for(int i=0 ; i<renders.Length-1;i++){
				renders[i].color = new Color (renders[i].color.r, renders[i].color.g, renders[i].color.b, 1f);
			}
		}
		if (currentHealth <= 0 && !isDead) {
			isDead = true;
			chillCount = 0;
			StartCoroutine(PlayerDying());
			//gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false ;//esto es provisorio HAY QUE CAMBIARLO!
		}
		UpdateMana ();
		chillUpdate ();
		shockUpdate ();
	}


	public static void LoadAtributes(){ //actualiza los atributos con los puntos añadidos, se llama cuando se carga un juego guardado
		atributes [Strength] = strAddedPoints;
		atributes [Vitality] = vitAddedPoints;
		atributes [Spirit] = spiAddedPoints;
		atributes [Dextery] = dexAddedPoints;

		defensives [MaxHealth] = atributes [Vitality] * 3 + InitMaxHealth; 
		offensives [MinDmg] = atributes [Strength] * 0.25f + InitMinDmg;
		offensives [MaxDamge] = atributes [Strength] * 0.25f + InitMaxDmg;
		offensives [MaxMana] = atributes [Spirit] * 3 + InitMana;
		//offensives[MgDmg] = atributes[Spirit] * 0.25f + InitMgDmg;
		offensives[ManaPerSec] = atributes[Spirit] * 0.1f + InitManaRegen;
		offensives [Accuracy] = atributes [Dextery] * 2 + InitAccuracy; //uno de destreza 2 de accuracy
		defensives [Evasiveness] = atributes [Dextery] * 2 + InitEvasion;
		currentHealth = defensives[MaxHealth];
		currentMana = offensives [MaxMana];
		UpdateMana ();
	}

	private void chillUpdate(){
		chillCount -= Time.deltaTime;
		if (chillCount <= 0 && chill) {
			chill = false;
			freeze = false;
			anim.speed = initAnimSpeed;
			currentAnimSpeed = anim.speed;
			utils[MovementSpeed] = InitMoveSpeed; //agregar el increased move speed cuando este implementado!!!!!!!!"!
			PlatformerCharacter2D.stopPlayer = false;
			for(int i=0 ; i<renders.Length-1;i++){
				renders[i].color = new Color (1f, 1f, 1f, 1f);
			}
		}
	}

	private void shockUpdate(){
		shockCount -= Time.deltaTime;
		if (shockCount <= 0 && shock) {
			shock = false;
			for(int i=0 ; i<renders.Length-1;i++){
				renders[i].color = new Color (1f, 1f, 1f, 1f);
			}
		}
	}

	IEnumerator PoisonedUpdate () {
		for(int i=0 ; i<renders.Length-1;i++){
			renders[i].color = new Color (0, 1f, 0, 1f);
		}
		while (poisonedCount >= 0 && !isDead) {
			yield return new WaitForSeconds (0.2f);
			poisonedCount -= 0.2f;
			currentHealth -= poisonedDmg;
			if (currentHealth < 0)
				currentHealth = 0;
		}
		poisoned = false;
		for(int i=0 ; i<renders.Length-1;i++){
			renders[i].color = new Color (1f, 1f, 1f, 1f);
		}
	}

	IEnumerator IgnitedUpdate () {
		for(int i=0 ; i<renders.Length-1;i++){
			renders[i].color = new Color (1f, 0.1f, 0f, 1f);
		}
		while (ignitedCount >= 0 && !isDead) {
			yield return new WaitForSeconds (0.2f);
			ignitedCount -= 0.2f;
			currentHealth -= ignitedDmg;
			if (currentHealth <= 0){
				currentHealth = 0;
			}
		}
		ignited = false;
		for(int i=0 ; i<renders.Length-1;i++){
			renders[i].color = new Color (1f, 1f, 1f, 1f);
		}
	}

	public static void AddAtribute(int atribute){
		switch (atribute) {
			case 0:
				atributes [Strength]++;
				offensives [MinDmg] += 0.25f;
				offensives [MaxDamge] += 0.25f;
				break;
			case 1:
				atributes [Dextery]++;
				offensives [Accuracy] +=  2 ;
				defensives [Evasiveness] +=  2; 
				break;
			case 2:
				atributes [Vitality]++;
				defensives [MaxHealth] += 3;
				break;
			case 3:
				atributes [Spirit]++;
				offensives[MaxMana] += 3;
			//	offensives[MgDmg] += 0.25f;
				offensives[ManaPerSec] += 0.1f;
				break;
		}

	}


	IEnumerator PlayerDying () {
		anim.SetBool("IsDead", true);
		playerDeathSound.Play ();
		yield return new WaitForSeconds (2.5f);
		anim.SetBool("IsDead", false);
		anim.speed = initAnimSpeed;
		readyToRespawn = true;
		GameController.previousExit = 0;
		Fading.BeginFadeIn("level1");
	}

	IEnumerator ManaRegeneration () {
		while (!isDead) {
			//Begin Traits
			if(Traits.traits[Traits.MPREGEN].isActive()){
				if (currentMana < offensives [MaxMana]/2){
					currentMana += (offensives [MaxMana]/2 * offensives [ManaPerSec]/100);
				}
				if (currentMana > offensives [MaxMana]/2)
					currentMana = offensives [MaxMana]/2;
			}
			//End Traits
			else{
				if (currentMana < offensives [MaxMana]){
					currentMana += (offensives [MaxMana] * offensives [ManaPerSec]/100)/2;
				}
				if (currentMana > offensives [MaxMana])
					currentMana = offensives [MaxMana];
			}
			yield return new WaitForSeconds (0.5f);
		}
	}

	IEnumerator LifeRegeneration () {
		while (!isDead) {
			if (currentHealth < defensives [MaxHealth])
				currentHealth += (defensives[MaxHealth] * defensives [LifePerSecond] / 100)/2;
			if (currentHealth > defensives [MaxHealth])
				currentHealth = defensives [MaxHealth];
			yield return new WaitForSeconds (0.5f);
		}
	}

	public static void AddLife(float percentage){
		if (currentHealth < defensives [MaxHealth])
			currentHealth += (defensives[MaxHealth] * percentage / 100); //da un 5% de la vida maxima
		if (currentHealth > defensives [MaxHealth])
			currentHealth = defensives [MaxHealth];
	}

	/* setea los stats iniciales */
	private static void intialStats(){
		offensives [MinDmg] = InitMinDmg;
		offensives [MaxDamge] = InitMaxDmg;
		defensives [MaxHealth] = InitMaxHealth;
		utils [MovementSpeed] = InitMoveSpeed;
		offensives [BaseAttacksPerSecond] = InitAttacksPerSecond;
		offensives [CritChance] = InitCritChance;
		offensives [CritDmgMultiplier] = InitCritDmgMult;
		offensives [MaxMana] = InitMana;
		offensives [ManaPerSec] = InitManaRegen;
		offensives [Accuracy] = InitAccuracy;
		defensives [Evasiveness] = InitEvasion;
		lvl = 1;
		currentExp = 0;
		oldNextLevelExp = 0;

		nextLevelExp = ExpFormula ();

	}

	public void RespawnStats(){ //Restaura los valores predeterminados del jugador
		currentHealth = defensives [MaxHealth];
		currentMana = offensives [MaxMana];
		utils [MovementSpeed] = InitMoveSpeed;
		StartCoroutine (ManaRegeneration());
		isDead = false;
		StartCoroutine (LifeRegeneration ());
		StartCoroutine (ManaRegeneration());
		gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
		poisoned = false;
		chill = false;
		ignited = false;
		for(int i=0 ; i<renders.Length-1;i++){
			renders[i].color = new Color (1f, 1f, 1f, 1f);
		}
		UpdateMana ();
	}

	public static double ExpFormula(){
		return oldNextLevelExp + Math.Pow(1.2,lvl)*100;
	}

	public static void UpdateExp(double exp){
		 
		currentExp += exp;
		if (currentExp >= nextLevelExp) {
			lvl++;
			levelUpSoundStatic.Play();
			CombatText.ShowCombatText("Level Up!");
			passivePoints++;
			oldNextLevelExp = nextLevelExp;
			nextLevelExp = ExpFormula();
			atributesPoints += 5;
		}
		expUi.UpdateExpBar (currentExp,oldNextLevelExp,nextLevelExp);
		for (int i = 0; i < plat.playerSkills.Length; i++) {
			if(plat.playerSkills[i] != null)
				plat.playerSkills[i].GetComponent<SpellStats>().UpdateExp(exp);
		}
		//Debug.Log ("currExp " + currentExp + ", " + "nextLevelExp " + nextLevelExp + ", " + "lvl " + lvl );
	}

	public static void UpdateMana(){
		//Begin Traits
		if(Traits.traits[Traits.MPREGEN].isActive())
			ManaUiController.UpdateManaBar (currentMana,offensives[MaxMana]/2);
		//End Traits
		else
			ManaUiController.UpdateManaBar (currentMana,offensives[MaxMana]);
	}

	public bool Hit(float dmg, Types.Element type, float accuracy, bool isCritical){ //se llama cuando un enemigo le pega al jugador

		float defense = defensives [Defense] + defensives [IncreasedDefense];
		if (ghostMode == true) {
			return false;
		}
		if (accuracy > -1) { //si es -1 siempre le pega
			float chanceToEvade = (float)System.Math.Round ((float)(1 - accuracy / (accuracy + System.Math.Pow ((double)((defensives [Evasiveness]+defensives[IncreasedEvasion]) / 4), 0.8))), 2);
			float[] cteProbs = {1 - chanceToEvade, chanceToEvade};
			if (Utils.Choose (cteProbs) != 0) {
				if(!anim.GetBool("Attacking") && !anim.GetBool("BowAttacking") && !anim.GetBool("SpellCasting"))
					anim.SetBool ("Evading", true);
				Debug.Log ("Esquivaste el ataque! ");
				CombatText.ShowCombatText("Dodge");
				return false;
			}
		}
		float[] blockProb = {1 - defensives[BlockChance]/100 , defensives[BlockChance]/100 };
		if (Utils.Choose (blockProb) != 0) { 
			anim.SetBool ("Blocking", true);
			blockSound.Play();
			Debug.Log ("Bloqueaste el ataque! " );
			return true;
		}

		ghostMode = true; //ghost mode
		for(int i=0 ; i<renders.Length-1;i++){
			renders[i].color = new Color (renders[i].color.r, renders[i].color.g, renders[i].color.b, 0.3f);
		}
		ghostModeCount = ghostModeTime;
		
		float realDmg = dmg;
		if(shock){
			realDmg *= 1.5f; //cuando esta shokeado aumenta el daño recibido de cualquier tipo
		}
		switch (type){
		case Types.Element.None:
			//Begin Traits
			if (Traits.traits[Traits.PDAMAGE].isActive ())
				realDmg -= (defense*(float)0.75 / (defense*(float)0.75 + 8 * realDmg));
			//End Traits
			else
				realDmg -= (defense / (defense + 8 * realDmg));	
			//Debug.Log("me pegaron man! :(");
			break;
		case Types.Element.Cold:

			realDmg -= Math.Abs((realDmg * (defensives[ColdRes]/100)));
			realDmg -= Math.Abs((realDmg * (defensives[AllRes]/100)));
			if (!Traits.traits[Traits.NOFREEZE].isActive()){
				if(!freeze)
					chillCount = chillTimer;
				if(!chill){
					if(isCritical){
						Debug.Log("congelado");
						utils[MovementSpeed] = 0;
						anim.speed = 0;
						PlatformerCharacter2D.stopPlayer = true;
						freeze = true;
					}
					else{
						utils[MovementSpeed] -= 2;
						anim.speed -= 0.5f;
					}
					currentAnimSpeed = anim.speed;
				}
				chill = true;
				for(int i=0 ; i<renders.Length-1;i++){
					renders[i].color = new Color (0f, 1f, 1f, 1f);
				}
			}
			break;
		case Types.Element.Fire:
			realDmg -= Math.Abs((realDmg * (defensives[FireRes]/100)));
			realDmg -= Math.Abs((realDmg * (defensives[AllRes]/100)));
			if (!Traits.traits[Traits.NOBURN].isActive()){
				if(isCritical){
					ignitedDmg = (0.2f * realDmg)/5; // 20% del daño infligido en 1 seg
					ignitedCount = ignitedTime;
					if(!ignited)
						StartCoroutine(IgnitedUpdate());
					ignited = true;
				}
			}
			break;
		case Types.Element.Poison:
			realDmg -= Math.Abs((realDmg * (defensives[PoisonRes]/100)));
			realDmg -= Math.Abs((realDmg * (defensives[AllRes]/100)));
			if (!Traits.traits[Traits.NOPOISON].isActive()){
				poisonedDmg = (0.3f * realDmg)/5; // 30% del daño infligido en 1 seg
				poisonedCount = poisonedTimer;
				if(!poisoned)
					StartCoroutine(PoisonedUpdate());
				poisoned = true;
			}
			break;
		case Types.Element.Lightning:
			realDmg -= Math.Abs((realDmg * (defensives[LightRes]/100)));
			realDmg -= Math.Abs((realDmg * (defensives[AllRes]/100)));
			if (!Traits.traits[Traits.NOSHOCK].isActive()){
				if(isCritical){
					for(int i=0 ; i<renders.Length-1;i++){
						renders[i].color = new Color (0.75f, 0.6f, 1f, 1f);
					}
					shockCount = shockTimer;
					shock = true;
				}
			}
			break;
		default:
			Debug.LogError("El ataque no es de ningun tipo!!!");
			break;
		}
		//Debug.Log ("daño con defensa:" + realDmg);
		//Begin Traits
		if (Traits.traits[Traits.MPDAMAGE].isActive ()) {
			if (currentMana >= realDmg/4)
				currentMana -= realDmg/4;
			realDmg -= realDmg/4;
		}
		//End Traits
		if (realDmg < 0)
			realDmg = 0;
		currentHealth -= realDmg;
		ui.TakeDamage (realDmg);
		return true;
	}

	public void stopBlocking(){
		anim.SetBool ("Blocking",false);
	}

	public void stopEvading(){
		anim.SetBool ("Evading",false);
	}
}
