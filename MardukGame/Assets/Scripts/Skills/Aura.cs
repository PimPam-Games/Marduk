using UnityEngine;
using System.Collections;
using p = PlayerStats;

public class Aura : SpellStats {

	public float increasedAttackSpeed = 0;
	public float increasedCastSpeed = 0;
	public float increasedMovementSpeed = 0;
	public float increasedManaRegen = 0;
	public float increasedMaxMana = 0;

	// Use this for initialization
	void Start () {
		p.offensives [p.IncreasedAttackSpeed] += increasedAttackSpeed;
		p.offensives [p.IncreasedCastSpeed] += increasedCastSpeed;
		p.utils [p.MovementSpeed] += increasedMovementSpeed;
		p.offensives[p.MaxMana] += increasedMaxMana;
		p.offensives[p.ManaPerSec] += increasedManaRegen;
	}
	

	public override void RemoveSkill(){
		p.offensives [p.IncreasedAttackSpeed] -= increasedAttackSpeed;
		p.offensives [p.IncreasedCastSpeed] -= increasedCastSpeed;
		p.utils [p.MovementSpeed] -= increasedMovementSpeed;
		p.offensives[p.MaxMana] -= increasedMaxMana;
		p.offensives[p.ManaPerSec] -= increasedManaRegen;
		
		base.RemoveSkill ();
	}
}
