using UnityEngine;
using System.Collections;
using p = PlayerStats;
using System;

public class SpellStats : MonoBehaviour {


	private int inventoryPositionX = -2; //posicion del item en el inventario
	private int inventoryPositionY = -2; 
	public string spellName;
	public string nameForSave; //el nombre que se guarda del skill es para poder instanciarlo despues
	public float castPerSecond;
	protected float castDelay;

	public Types.SkillsTypes type;
	public Types.SkillsRequirements[] requeriments;
	public float manaCost;
	public float manaReserved;     //para auras


	public float animSpeed = 0;

	protected float cdTimer; 

	[SerializeField] protected float initManaCost;

	//[SerializeField] private float initLifeRegen;

	[SerializeField] protected float manaCostPerLvl;

	//[SerializeField] private float lifeRegenPerLvl;

	protected  double currentExp;
	protected  int lvl;
	protected  double nextLevelExp;
	protected  double oldNextLevelExp;

	public int InventoryPositionX{
		get {return inventoryPositionX;}
		set {inventoryPositionX = value;}
	}
	
	public int InventoryPositionY{
		get {return inventoryPositionY;}
		set {inventoryPositionY = value;}
	}

	public double CurrentExp{
		get {return currentExp;}
		set {currentExp = value;}
	}
	public int Lvl{
		get {return lvl;}
		set {lvl = value;}
	}
	public double NextLevelExp{
		get {return nextLevelExp;}
		set {nextLevelExp = value;}
	}
	public double OldNextLevelExp{
		get {return oldNextLevelExp;}
		set {oldNextLevelExp = value;}
	}

	protected virtual void Awake(){
		lvl = 1;
		currentExp = 0;
		oldNextLevelExp = 0;
		nextLevelExp = SpellExpFormula ();

	}

	// Use this for initialization
	void Start () {
		CalculateStats ();
	}
	
	// Update is called once per frame
	protected virtual void Update () {

		cdTimer -= Time.deltaTime;
	}

	protected virtual void CalculateStats (){

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

	public virtual void RemoveSkill(){
		//if(type == Types.SkillsTypes.Aura)
			//p.defensives [p.LifePerSecond] -= lifeRegenPerSecond;
		Destroy (this.gameObject);
	}
}
