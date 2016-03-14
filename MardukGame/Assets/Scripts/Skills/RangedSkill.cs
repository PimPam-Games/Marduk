using UnityEngine;
using System.Collections;
using p = PlayerStats;

public class RangedSkill : SpellStats {

	public Vector2 force;
	public bool flipProjectile;
	public bool staticProjectile = false;
	public GameObject projectile;
	private PlayerProjStats projStats;
	public bool dontChangeRotation = false; // para que la rotacion no se ajuste a la rotacion del lanzador
	public bool continuosRelease = false; // true si el poder se tira continuamente como incinerate
	public bool drainMana = false; //si debe drenar el mana o gastarlo de una

	public float initMinDmg;
	public float initMaxDmg;
	[SerializeField] private float initPhysicalDmgMult;

	[SerializeField] private float minDmgPerLvl;
	[SerializeField] private float maxDmgPerLvl;
	[SerializeField] private float physicalDmgMultPerLvl;


	

	protected override void Awake() {
		base.Awake();
		if (projectile != null)
			projStats = projectile.GetComponent<PlayerProjStats> ();
	}

	protected override void CalculateStats (){
		base.CalculateStats();
		if (projStats != null) {
			/*projStats.minDmg = initMinDmg; //+ (lvl - 1) * minDmgPerLvl; //en arcos no se usa
			projStats.maxDmg = initMaxDmg; //+ (lvl - 1) * maxDmgPerLvl;	//en arcos no se usa*/ // habia un drama con el daño de los projectiles
			projStats.physicalDmgMult = initPhysicalDmgMult + (lvl-1) * physicalDmgMultPerLvl;
		}
	}

	// Use this for initialization
	void Start () {
		CalculateStats ();
	}
	


	// Update is called once per frame
	protected override void Update () {
		base.Update ();
        castDelay = 1 / (castPerSecond + castPerSecond * (p.offensives[p.IncreasedCastSpeed] / 100));
        /*castDelay = 1 / (castPerSecond + castPerSecond * (p.offensives[p.IncreasedCastSpeed]/100)); //1 / (p.offensives [p.BaseAttacksPerSecond] + (p.offensives [p.BaseAttacksPerSecond] * (p.offensives [p.IncreasedAttackSpeed]/100)));
		if(castDelay >= 0.8f)
			animSpeed = 0;
		if(castDelay < 0.8f && castDelay >= 0.5f)
			animSpeed = 1;
		if(castDelay < 0.5f && castDelay >= 0.3f)
			animSpeed = 2;
		if(castDelay < 0.3f && castDelay >= 0.15f)
			animSpeed = 6;
		if(castDelay < 0.15f)
			animSpeed = 8;*/
    }

	public override string ToolTip ()
	{
		string tooltip = "";
		tooltip =  base.ToolTip();
		tooltip += "<color=green>----------------------------------</color> \n";
		if(string.Compare(spellName,"Multiple Shot") == 0){
			tooltip += "Fires multiple arrows at different targets \n \n";
			tooltip += "2 additional arrows \n";
		}
		if(string.Compare(spellName,"Lightning Ball") == 0){
			tooltip += "Fires a projectile that deals lightning damage \n \n";
		}
		if(string.Compare(spellName,"Poison Ball") == 0){
			tooltip += "Fires a projectile that deals poison damage \n \n";
		}
		if(string.Compare(spellName,"Poison Nova") == 0){
			tooltip += "Casts a ring of Poison around you that deals aoe damage \n \n";
		}
        tooltip += "Cast per second: " + System.Math.Round(1/castDelay,2).ToString() + "\n";
		if(initPhysicalDmgMult > 0)
			tooltip += "Deals " + projStats.physicalDmgMult + "% of Base Attack Damage \n";
		if(initMaxDmg > 0){
			double mindmg = initMinDmg + p.offensives[p.MagicDmg];
			mindmg = System.Math.Round(mindmg + mindmg * p.offensives[p.IncreasedMgDmg]/100,1);
			double maxdmg = initMaxDmg + p.offensives[p.MagicDmg];
			maxdmg = System.Math.Round(maxdmg + maxdmg * p.offensives[p.IncreasedMgDmg]/100,1);
			tooltip += "Damage: " + mindmg.ToString() +  " - " + maxdmg.ToString() + "\n";
		}
		return tooltip;
	}
}
