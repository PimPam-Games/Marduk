using UnityEngine;
using System.Collections;
using p = PlayerStats;
using System;

public class SpellStats : MonoBehaviour {


	private int idSlotEquipped = -1; //id del slot en el que esta equipado, -1 si no esta en ninguno
	private int inventoryPositionX = -2; //posicion del item en el inventario
	private int inventoryPositionY = -2; 
	protected bool equipped = false; 
	public string spellName;
	public string nameForSave; //el nombre que se guarda del skill es para poder instanciarlo despues
	public float castPerSecond;
	protected float castDelay;

	public Types.SkillsTypes type;
	public Types.SkillsRequirements[] requeriments;
	//public float manaCost;
	public float manaReserved;     //para auras

	public float animSpeed = 0;

	protected Support supportSkill = null; //el support skill que tiene incorporado este skill
	protected float cdTimer; 

	[SerializeField] protected float initManaCost;

	//[SerializeField] private float initLifeRegen;

	//[SerializeField] protected float manaCostPerLvl;

	//[SerializeField] private float lifeRegenPerLvl;

	protected  double currentExp;
	protected  int lvl = 1;
	protected  double nextLevelExp;
	protected  double oldNextLevelExp;

	public Support SupportSkill{
		get {return supportSkill;}
		set {supportSkill = value;}
	}

	public int InventoryPositionX{
		get {return inventoryPositionX;}
		set {inventoryPositionX = value;}
	}
	
	public int InventoryPositionY{
		get {return inventoryPositionY;}
		set {inventoryPositionY = value;}
	}

	public int IdSlotEquipped{
		get {return idSlotEquipped;}
		set {idSlotEquipped = value;}
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
    public float ManaCost
    {
        get {
            float cost = initManaCost + (p.lvl - 1) * (initManaCost * 50 / 100);
            //Begin Traits
            if (Traits.traits[Traits.MSKILLCOST].isActive() && type == Types.SkillsTypes.Melee)
            {
                cost *= 0.8f;
            }
            //Begin Traits
            if (Traits.traits[Traits.RSKILLCOST].isActive() && type == Types.SkillsTypes.Ranged)
            {
                cost *= 0.8f;
            }
            //End Traits
            //End Traits
            return cost;
        } //formula para calcular el mana
    }

    protected virtual void Awake(){
		lvl = 1;
		currentExp = 0;
		oldNextLevelExp = 0;
		nextLevelExp = SpellExpFormula ();
	}

	// Use this for initialization
	void Start () {
		castDelay = castPerSecond; //si es un skill que depende de la velocidad de casteo del personaje, se sobreescribe el update para cambiar castdelay
		CalculateStats ();
	}
	
	// Update is called once per frame
	protected virtual void Update () {		
		cdTimer -= Time.deltaTime; 
	}

	protected virtual void CalculateStats (){

		//manaCost = initManaCost + (lvl - 1) * manaCostPerLvl;
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
	
	public virtual void EquipSkill(){
	}

	public virtual void UnequipSkill(){
		
	}

	public virtual void RemoveSkill(){
		//if(type == Types.SkillsTypes.Aura)
			//p.defensives [p.LifePerSecond] -= lifeRegenPerSecond;
		Destroy (this.gameObject);
	}

	public virtual string ToolTip(){
		string tooltip = spellName;
		if(supportSkill != null)
			tooltip += "<color=grey>("+ supportSkill.spellName +")</color>";
		tooltip += "\n"; 
		switch(type){
			case Types.SkillsTypes.Aura:
				tooltip += "Type: <color=#1F45FC>" + type + "</color> \n";
				break;
			case Types.SkillsTypes.Melee:
				tooltip += "Type: <color=#F70D1A>" + type + "</color> \n";
				break;
			case Types.SkillsTypes.Ranged:
				tooltip += "Type: <color=green>" + type + "</color> \n";
				break;
			case Types.SkillsTypes.Utility:
				tooltip += "Type: <color=#EAC117>" + type + "</color> \n";
				break;
			case Types.SkillsTypes.Support:
				tooltip += "Type: <color=grey>" + type + "</color> \n";
				break;
		}
		if(Lvl <= 0){ //a veces aparece lvl = 0 cuando deberia ser 1
			lvl = 1;
			CalculateStats();
		}
		//tooltip += "level: " + Lvl.ToString() + "\n";
		if(ManaCost > 0){
			tooltip += "Mana cost: " + System.Math.Round(ManaCost,1) + "\n";
		}
		if(requeriments != null && requeriments.Length > 0 && requeriments[0] != Types.SkillsRequirements.None){ 
			tooltip += "Only works with ";
			for(int i= 0 ; i < requeriments.Length ; i++){
				tooltip += requeriments[i] + "s";
				if(i < requeriments.Length - 1)
					tooltip += ", ";
			}
			tooltip += "\n";			
		}
		if(supportSkill != null){
			tooltip += "<color=grey>----------------------------------</color> \n";
			//tooltip += "<color=grey>"+ supportSkill.spellName +"</color> \n";
			tooltip += "Deals " + supportSkill.damageAdded +"% of extra " + supportSkill.dmgElement + " damage \n";	
		}
		return tooltip;
	}
}
