using UnityEngine;
using System.Collections;
using p = PlayerStats;

public class Aura : SpellStats {

	public Sprite sprite;
	public float increasedAttackSpeed = 0;
	public float increasedCastSpeed = 0;
	public float increasedMovementSpeed = 0;
	public float increasedManaRegen = 0;
	public float increasedMaxMana = 0;
	private int auraSpriteIndex = -1; //indica en que  posicion esta ubicado el sprite de esta aura, -1 si no esta equipado

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
		ToggleAuraSprite(true);
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
		ToggleAuraSprite(false);
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

	private void ToggleAuraSprite(bool equip){
		if(equip){
			for(int i = 0; i < p.auraSprites.Length; i++){
				if(p.auraSprites[i].sprite == null){
					p.auraSprites[i].sprite = this.sprite;
					auraSpriteIndex = i;
					return;
				}
			}
		}
		else{
			if(auraSpriteIndex > -1 && auraSpriteIndex < 4) //hay hasta 3 auras
				p.auraSprites[auraSpriteIndex].sprite = null;
		}
	}

	public override string ToolTip ()
	{
		string tooltip = "";
		tooltip =  base.ToolTip();
		tooltip += " <color=#1F45FC> ----------------------------------</color> \n";
		if(string.Compare(spellName,"Anxiety") == 0){
			tooltip += "Casts an aura that increases attack speed and cast speed \n \n";
		}
		if(string.Compare(spellName,"Mana Flows") == 0){
			tooltip += "Casts an aura that increases mana regeneration and maximun mana \n \n";
		}
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
