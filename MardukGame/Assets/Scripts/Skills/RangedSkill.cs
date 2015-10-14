using UnityEngine;
using System.Collections;
using p = PlayerStats;

public class RangedSkill : SpellStats {

	public Vector2 force;
	public bool flipProjectile;
	public GameObject projectile;
	private PlayerProjStats projStats;


	[SerializeField] private float initMinDmg;
	[SerializeField] private float initMaxDmg;
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
			projStats.minDmg = initMinDmg + (lvl - 1) * minDmgPerLvl;
			projStats.maxDmg = initMaxDmg + (lvl-1) * maxDmgPerLvl;
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
	}
}
