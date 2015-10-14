using UnityEngine;
using System.Collections;
using p = PlayerStats;
using System;

public class SpellStats : MonoBehaviour {

	public Vector2 force;
	public bool flipProjectile;
	public GameObject projectile;
	public string spellName;
	public string nameForSave; //el nombre que se guarda del skill es para poder instanciarlo despues
	public float castPerSecond;

	public Types.SkillsTypes type;
	public float manaCost;
	public float manaReserved;     //para auras
	public float lifeRegenPerSecond; 
	public float movementX;      //para movimiento
	public float movementY;
	public float moveTime; //tiempo en el que tiene que estar activado el skill de movimieto
	public float animSpeed = 0;
	private float castDelay;
	private float cdTimer; 

	[SerializeField] private float initManaCost;
	[SerializeField] private float initMinDmg;
	[SerializeField] private float initMaxDmg;
	[SerializeField] private float initPhysicalDmgMult;
	//[SerializeField] private float initLifeRegen;

	[SerializeField] private float manaCostPerLvl;
	[SerializeField] private float minDmgPerLvl;
	[SerializeField] private float maxDmgPerLvl;
	[SerializeField] private float physicalDmgMultPerLvl;
	//[SerializeField] private float lifeRegenPerLvl;

	public  double currentExp;
	public  int lvl;
	public  double nextLevelExp;
	public  double oldNextLevelExp;

	private PlayerProjStats projStats;

	void Awake(){
		lvl = 1;
		currentExp = 0;
		oldNextLevelExp = 0;
		nextLevelExp = SpellExpFormula ();
		if (projectile != null)
			projStats = projectile.GetComponent<PlayerProjStats> ();

	}

	// Use this for initialization
	void Start () {
		CalculateStats ();
	}
	
	// Update is called once per frame
	void Update () {
		castDelay = 1 / (castPerSecond + castPerSecond * (p.offensives[p.IncreasedCastSpeed]/100)); //1 / (p.offensives [p.BaseAttacksPerSecond] + (p.offensives [p.BaseAttacksPerSecond] * (p.offensives [p.IncreasedAttackSpeed]/100)));
		if(castDelay >= 0.8f)
			animSpeed = 0;
		if(castDelay < 0.8f && castDelay >= 0.5f)
			animSpeed = 1;
		if(castDelay < 0.5f && castDelay >= 0.3f)
			animSpeed = 2;
		if(castDelay < 0.3f && castDelay >= 0.15f)
			animSpeed = 6;
		if(castDelay < 0.15f)
			animSpeed = 8;
		cdTimer -= Time.deltaTime;
	}

	private void CalculateStats(){
		if (projStats != null) {
			projStats.minDmg = initMinDmg + (lvl - 1) * minDmgPerLvl;
			projStats.maxDmg = initMaxDmg + (lvl-1) * maxDmgPerLvl;
			projStats.physicalDmgMult = initPhysicalDmgMult + (lvl-1) * physicalDmgMultPerLvl;
		}
		manaCost = initManaCost + (lvl - 1) * manaCostPerLvl;
	}

	public void UpdateExp(double exp){
		currentExp += exp;
		if (currentExp >= nextLevelExp) {
			lvl++;
			oldNextLevelExp = nextLevelExp;
			nextLevelExp = SpellExpFormula();
			CalculateStats();
		}
		//Debug.Log ("currExp " + currentExp + ", " + "nextLevelExp " + nextLevelExp + ", " + "lvl " + lvl );
	}

	public double SpellExpFormula(){
		return oldNextLevelExp + Math.Pow(1.2,lvl)*100;
	}

	public float CDtimer{
		get {return cdTimer;}
	}

	public void ActivateCoolDown(){
		if(cdTimer <= 0)
			cdTimer = castDelay;
	}

	public void RemoveSkill(){
		if(type == Types.SkillsTypes.Aura)
			p.defensives [p.LifePerSecond] -= lifeRegenPerSecond;
		Destroy (this.gameObject);
	}
}
