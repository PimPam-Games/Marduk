using UnityEngine;
using System.Collections;

public class MeleeSkill : SpellStats {

	public float initSacrifiedLife = 0, initconvertedDmg = 0 , initDmgMultiplier = 100;
	public float sacrifiedLifePerLvl = 0; //porcentaje
	public float dmgMultiplierPerLvl = 0;
	public Types.Element elementToConvert = Types.Element.None;
	public float convertedDmg = 0;
	public GameObject projectile;

	private float dmgMultiplier = 0;
	private float sacrifiedLife = 0;

	public float SacrifiedLife{
		get {return sacrifiedLife;}
	}

	public float DmgMultiplier{
		get {return dmgMultiplier;}
	}
	

	protected override void Awake() {
		base.Awake();
		CalculateStats();
	}

	protected override void CalculateStats (){
		base.CalculateStats();
		dmgMultiplier = initDmgMultiplier + (lvl-1) * dmgMultiplierPerLvl;		
		sacrifiedLife = initSacrifiedLife + (lvl-1) * sacrifiedLifePerLvl;
	}
	
	protected override void Update () {
		base.Update();
	}

	// Use this for initialization
	void Start () {
		CalculateStats ();
	}

	public override string ToolTip ()
	{
		string tooltip = "";
		tooltip =  base.ToolTip();
		tooltip += "<color=#F70D1A>----------------------------------</color> \n";
		if(string.Compare(spellName,"Sacrifice") == 0){
			tooltip += "Performs a powerful attack that costs life \n \n";
		}
		if(string.Compare(spellName,"Burning Blow") == 0){
			tooltip += "Performs a fire attack that casts a heat wave that deals 55% of your damage when it hits an enemy \n \n";
		}
		if(string.Compare(spellName,"Thunder Blow") == 0){
			tooltip += "Performs a lightning attack that brings down a thunder that deals 65% of your damage when it hits an enemy \n \n";
		}
		if(string.Compare(spellName,"Plant Thrust") == 0){
			tooltip += "Performs a poison attack that unleashes deadly plants that deals 55% of your damage when it hits an enemy \n \n";
		}
		if(sacrifiedLife > 0)
			tooltip += "Sacrificed Life: %" + sacrifiedLife + "\n";
		if(dmgMultiplier > 0)
			tooltip += "Deals " + dmgMultiplier + "% of Base Attack Damage \n";
		return tooltip;
	}
}
