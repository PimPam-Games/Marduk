using UnityEngine;
using System.Collections;
using System;
using pi = PlayerItems;
using expUi = ExpUiController;

public class PlayerStats : MonoBehaviour {


	public const int CantAtributes = 4, CantOffensives = 13, CantDefensives = 11, CantUtils = 2;
	public const int Strength = 0, Dextery = 1, Vitality = 2, Spirit = 3; //atributes
	public const int MinDmg = 0, MaxDamge = 1, MgDmg = 2 ,CritChance = 3, CritDmgMultiplier = 4, Accuracy = 5, StunChance = 6, BleedChance = 7, CertainStrChance = 8, ManaPerSec = 9, MaxMana = 10, IncreasedAttackSpeed = 11, BaseAttacksPerSecond = 12; //offensives
	public const int MaxHealth = 0 ,Defense = 1, ColdRes = 2, FireRes = 3, LightRes = 4, PoisonRes = 5, BlockChance = 6, Evasiveness = 7, Thorns = 8, LifePerHit = 9, LifePerSecond = 10;  //defensives
	public const int MovementSpeed = 0, MagicFind = 1;//utils

	public const float InitMoveSpeed = 5;
	public const float InitMaxHealth = 45;
	public const float InitAttacksPerSecond = 1;
	public const float InitMinDmg = 1;
	public const float InitMaxDmg = 2;
	public const float InitMgDmg = 1;
	public const float InitMana = 45; 
	public const float InitManaRegen = 0.2f;
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
	public static double nextLevelExp;
	public static double oldNextLevelExp;
	public static int atributesPoints = 0; //puntos de atributos que quedan por poner cada vez que se pasa de nivel
	public static int strAddedPoints = 0;
	public static int vitAddedPoints = 0;
	public static int spiAddedPoints = 0;
	public static int dexAddedPoints = 0;

	public AudioSource playerDeathSound;
	public AudioSource blockSound;

	public static bool ghostMode;
	public float ghostModeTime = 1f;
	private float ghostModeCount;

	private bool chill = false;
	private float chillTimer = 1f;
	private float chillCount = 0;

	private bool poisoned = false;
	private float poisonedTimer = 1f;
	private float poisonedCount = 0;
	private float poisonedDmg = 0.4f;

	public static string playerName;

	private SpriteRenderer[] renders;

	void Awake(){
		atributes = new float[CantAtributes];
		offensives = new float[CantOffensives];
		defensives = new float[CantDefensives];
		utils = new float[CantUtils];
		anim = GetComponent<Animator> ();
	}

	// Use this for initialization
	void Start () {
		intialStats ();
		isDead = false;
		ui = this.GetComponent<PlayerUIController> ();
		renders = GetComponentsInChildren<SpriteRenderer> ();

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
				if(chill)
					renders[i].color = new Color (0f, 1f, 1f, 1f);
				else{
					if(poisoned)
						renders[i].color = new Color (0, 1f, 0, 1f);
					else
						renders[i].color = new Color (1f, 1f, 1f, 1f);
				}
			}
		}
		if (currentHealth <= 0 && !isDead) {
			isDead = true;
			StartCoroutine(PlayerDying());
			//gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false ;//esto es provisorio HAY QUE CAMBIARLO!
		}
		UpdateMana ();
		chillUpdate ();
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
		offensives[MgDmg] = atributes[Spirit] * 0.25f + InitMgDmg;
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
			anim.speed += 0.5f;
			utils[MovementSpeed] += 2;
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
				offensives[MgDmg] += 0.25f;
				offensives[ManaPerSec] += 0.1f;
				break;
		}

	}


	IEnumerator PlayerDying () {
		anim.SetBool("IsDead", true);
		playerDeathSound.Play ();
		yield return new WaitForSeconds (2.5f);
		anim.SetBool("IsDead", false);
		readyToRespawn = true;
		GameController.previousExit = 0;
		Fading.BeginFadeIn("level1");
	}

	IEnumerator ManaRegeneration () {
		while (!isDead) {
			if (currentMana < offensives [MaxMana])
				currentMana += offensives [ManaPerSec];
			if (currentMana > offensives [MaxMana])
				currentMana = offensives [MaxMana];
			//UpdateMana();
			yield return new WaitForSeconds (1);
		}
	}

	IEnumerator LifeRegeneration () {
		while (!isDead) {
			if (currentHealth < defensives [MaxHealth])
				currentHealth += defensives [LifePerSecond];
			if (currentHealth > defensives [MaxHealth])
				currentHealth = defensives [MaxHealth];
			yield return new WaitForSeconds (1);
		}
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
		StartCoroutine (ManaRegeneration());
		isDead = false;
		StartCoroutine (LifeRegeneration ());
		gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
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
			oldNextLevelExp = nextLevelExp;
			nextLevelExp = ExpFormula();
			atributesPoints += 5;
		}
		expUi.UpdateExpBar (currentExp,oldNextLevelExp,nextLevelExp);
		//Debug.Log ("currExp " + currentExp + ", " + "nextLevelExp " + nextLevelExp + ", " + "lvl " + lvl );
	}

	public static void UpdateMana(){
		ManaUiController.UpdateManaBar (currentMana,offensives[MaxMana]);
	}

	public bool Hit(float dmg, Types.Element type, float accuracy){ //se llama cuando un enemigo le pega al jugador

		if (ghostMode == true) {
			return false;
		}
		if (accuracy > -1) { //si es -1 siempre le pega
			float chanceToEvade = (float)System.Math.Round ((float)(1 - accuracy / (accuracy + System.Math.Pow ((double)(defensives [Evasiveness] / 4), 0.8))), 2);
			float[] cteProbs = {1 - chanceToEvade, chanceToEvade};
			if (Utils.Choose (cteProbs) != 0) {
				if(!anim.GetBool("Attacking") && !anim.GetBool("BowAttacking"))
					anim.SetBool ("Evading", true);
				Debug.Log ("Esquivaste el ataque! ");
				return false;
			}
//			Debug.Log ("player chance To Evade: " + chanceToEvade);
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
			renders[i].color = new Color (1f, 1f, 1f, 0.3f);
		}
		ghostModeCount = ghostModeTime;

		float realDmg = dmg;
		switch (type){
		case Types.Element.None:
			realDmg -= (defensives[Defense] / (defensives[Defense] + 8 * realDmg));	
			//Debug.Log("me pegaron man! :(");
			break;
		case Types.Element.Cold:
			Debug.Log("congelado");
			realDmg -= Math.Abs((realDmg * (defensives[ColdRes]/100)));
			chillCount = chillTimer;
			if(!chill){
				utils[MovementSpeed] -= 2;
				anim.speed -= 0.5f;
			}
			chill = true;
			for(int i=0 ; i<renders.Length-1;i++){
				renders[i].color = new Color (0f, 1f, 1f, 1f);
			}
			break;
		case Types.Element.Fire:
			realDmg -= Math.Abs((realDmg * (defensives[FireRes]/100)));
			//Debug.Log("fire damage");
			break;
		case Types.Element.Poison:
			realDmg -= Math.Abs((realDmg * (defensives[PoisonRes]/100)));
			poisonedDmg = (0.3f * realDmg)/5; // 30% del daño infligido en 1 seg
			poisonedCount = poisonedTimer;
			if(!poisoned)
				StartCoroutine(PoisonedUpdate());
			poisoned = true;
			break;
		case Types.Element.Lightning:
			realDmg -= Math.Abs((realDmg * (defensives[LightRes]/100)));
			//Debug.Log("lightning damage");
			break;
		default:
			Debug.LogError("El ataque no es de ningun tipo!!!");
			break;
		}
		//Debug.Log ("daño con defensa:" + realDmg);
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
