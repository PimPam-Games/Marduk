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
		/*p.offensives [p.IncreasedAttackSpeed] += increasedAttackSpeed;
		p.offensives [p.IncreasedCastSpeed] += increasedCastSpeed;
		p.utils [p.MovementSpeed] += increasedMovementSpeed;
		p.offensives[p.MaxMana] += increasedMaxMana;
		p.offensives[p.ManaPerSec] += increasedManaRegen;*/
	}
	
	public override void EquipSkill ()
	{
		if(equipped)
			return;
		p.offensives [p.IncreasedAttackSpeed] += increasedAttackSpeed;
		p.offensives [p.IncreasedCastSpeed] += increasedCastSpeed;
		p.utils [p.MovementSpeed] += increasedMovementSpeed;
		p.offensives[p.MaxMana] += increasedMaxMana;
		p.offensives[p.ManaPerSec] += increasedManaRegen;
		equipped = true;
		
	}	

	public override void UnequipSkill ()
	{
		if(!equipped)
			return;
		p.offensives [p.IncreasedAttackSpeed] -= increasedAttackSpeed;
		p.offensives [p.IncreasedCastSpeed] -= increasedCastSpeed;
		p.utils [p.MovementSpeed] -= increasedMovementSpeed;
		p.offensives[p.MaxMana] -= increasedMaxMana;
		p.offensives[p.ManaPerSec] -= increasedManaRegen;
		equipped = false;
	}	

	public override void RemoveSkill(){
		p.offensives [p.IncreasedAttackSpeed] -= increasedAttackSpeed;
		p.offensives [p.IncreasedCastSpeed] -= increasedCastSpeed;
		p.utils [p.MovementSpeed] -= increasedMovementSpeed;
		p.offensives[p.MaxMana] -= increasedMaxMana;
		p.offensives[p.ManaPerSec] -= increasedManaRegen;
		
		base.RemoveSkill ();
	}

	public override string ToolTip ()
	{
		string tooltip = "";
		tooltip =  base.ToolTip();
		tooltip += "<color=red>----------------------------------</color> \n";
		if(increasedAttackSpeed > 0)
			tooltip += "Increased Attack Speed: %" + increasedAttackSpeed + "\n";
		if(increasedCastSpeed > 0)
			tooltip += "Increased Cast Speed: %" + increasedCastSpeed + "\n";
		if(increasedManaRegen > 0)
			tooltip += "Increased Mana Regeneration: %" + increasedManaRegen + "\n";
		if(increasedMaxMana > 0)
			tooltip += "Increased Max Mana: %" + increasedMaxMana + "\n";
		return tooltip;
	}
}
