using UnityEngine;
using System.Collections;

public class MeleeSkill : SpellStats {

	public float initSacrifiedLife = 0, initconvertedDmg = 0 , initDmgMultiplier = 100;
	public float sacrifiedLifePerLvl = 0; //porcentaje
	public float dmgMultiplierPerLvl = 0;
	public Types.Element elementToConvert = Types.Element.None;
	public float convertedDmg = 0;

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
}
