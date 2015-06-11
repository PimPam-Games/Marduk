using UnityEngine;
using System.Collections;
using System;
using pi = PlayerItems;
using expUi = ExpUiController;

public class PlayerStats : MonoBehaviour {


	public const int CantAtributes = 4, CantOffensives = 13, CantDefensives = 11, CantUtils = 2;
	public const int Strength = 0, Dextery = 1, Vitality = 2, Spirit = 3; //atributes
	public const int MinDmg = 0, MaxDamge = 1, MinMgDmg = 2, MaxMgDmg = 3 ,CritChance = 4, CritDmgMultiplier = 5, Accuracy = 6, StunChance = 7, BleedChance = 8, CertainStrChance = 9, ManaPerSec = 10, MaxMana = 11, AttackRate = 12; //offensives
	public const int MaxHealth = 0 ,Defense = 1, ColdRes = 2, FireRes = 3, LightRes = 4, PoisonRes = 5, BlockChance = 6, Evasiveness = 7, Thorns = 8, LifePerHit = 9, LifePerSecond = 10;  //defensives
	public const int MovementSpeed = 0, MagicFind = 1;//utils

	public const float InitMoveSpeed = 5;
	public const float InitAttackRate = 0.8f;
	public const float InitMaxHealth = 45;
	public const float InitMinDmg = 1;
	public const float InitMaxDmg = 2;
	public const float InitMana = 45; // esto no se va a ver en la barra de mana todavia

	public static float currentHealth;
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


	public static bool ghostMode;
	public float ghostModeTime = 1f;
	private float ghostModeCount;

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
		StartCoroutine(LifeRegeneration());

		//StartCoroutine (GhostModeRender());
	}
	
	// Update is called once per frame
	void Update () {
		ghostModeCount -= Time.deltaTime;
		if (ghostModeCount <= 0 && ghostMode) {
			ghostMode = false;
			for(int i=0 ; i<renders.Length-1;i++){
				renders[i].color = new Color (1f, 1f, 1f, 1f);
			}
		}
		if (currentHealth <= 0 && !isDead) {
			isDead = true;
			StartCoroutine(PlayerDying());
			//gameObject.GetComponentInChildren<SpriteRenderer>().enabled = false ;//esto es provisorio HAY QUE CAMBIARLO!
		}
	}

	public static void LoadAtributes(){ //actualiza los atributos con los puntos añadidos, se llama cuando se carga un juego guardado
		atributes [Strength] = strAddedPoints;
		atributes [Vitality] = vitAddedPoints;
		atributes [Spirit] = spiAddedPoints;
		atributes [Dextery] = dexAddedPoints;

		defensives [MaxHealth] = atributes [Vitality] * 3 + InitMaxHealth; 
		offensives [MinDmg] = atributes [Strength] * 0.25f + InitMinDmg;
		offensives [MaxDamge] = atributes [Strength] * 0.25f + InitMaxDmg;
		offensives [MaxMana] = atributes [Spirit] * 3;
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
				break;
			case 2:
				atributes [Vitality]++;
				defensives [MaxHealth] += 3;
				break;
			case 3:
				atributes [Spirit]++;
				break;
		}

	}


	IEnumerator PlayerDying () {
		anim.SetBool("IsDead", true);
		playerDeathSound.Play ();
		yield return new WaitForSeconds (2.5f);
		anim.SetBool("IsDead", false);
		isDead = false;
		readyToRespawn = true;

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
		offensives [AttackRate] = InitAttackRate;
		lvl = 1;
		currentExp = 0;
		oldNextLevelExp = 0;
		nextLevelExp = ExpFormula ();

	}

	public void RespawnStats(){ //Restaura los valores predeterminados del jugador
		currentHealth = defensives [MaxHealth];
		isDead = false;
		StartCoroutine (LifeRegeneration ());
		gameObject.GetComponentInChildren<SpriteRenderer>().enabled = true;
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
		Debug.Log ("currExp " + currentExp + ", " + "nextLevelExp " + nextLevelExp + ", " + "lvl " + lvl );
	}

	public void Hit(float dmg, Types.Element type){ //se llama cuando un enemigo le pega al jugador
		if (ghostMode == true) {
			return;
		}
		ghostMode = true;
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
			//Debug.Log("cold damage");
			realDmg -= Math.Abs((realDmg * (defensives[ColdRes]/100)));
			break;
		case Types.Element.Fire:
			realDmg -= Math.Abs((realDmg * (defensives[FireRes]/100)));
			//Debug.Log("fire damage");
			break;
		case Types.Element.Poison:
			realDmg -= Math.Abs((realDmg * (defensives[PoisonRes]/100)));
			//Debug.Log("poison damage");
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
	}
}
