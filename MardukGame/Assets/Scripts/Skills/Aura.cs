using UnityEngine;
using System.Collections;
using p = PlayerStats;

public class Aura : SpellStats {

	public float increasedAttackSpeed;
	public float increasedCastSpeed;
	public float increasedMovementSpeed;

	// Use this for initialization
	void Start () {
		p.offensives [p.IncreasedAttackSpeed] += increasedAttackSpeed;
		p.offensives [p.IncreasedCastSpeed] += increasedCastSpeed;
		p.utils [p.MovementSpeed] += increasedMovementSpeed;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public override void RemoveSkill(){
		p.offensives [p.IncreasedAttackSpeed] -= increasedAttackSpeed;
		p.offensives [p.IncreasedCastSpeed] -= increasedCastSpeed;
		p.utils [p.MovementSpeed] -= increasedMovementSpeed;
		base.RemoveSkill ();
	}
}
