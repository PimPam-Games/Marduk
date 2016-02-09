using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using p = PlayerStats;

public class Traits: MonoBehaviour
{
	public const int CantTraits = 25;
	public const int MPDAMAGE = 0, MPREGEN = 1, MPLEECH = 2, ACCURACY = 3, PDAMAGE = 4, MDAMAGE= 5, SEFFECT = 16, LOWHPDAMAGE = 7, NOFREEZE = 17, NOSHOCK = 18, 
	NOPOISON = 19, NOBURN = 20, FIREDAMAGE = 8, COLDDAMAGE = 9,LIGHTDAMAGE = 10, POISONDAMAGE= 11, CRITACC = 6, BLOCKDMG = 12, HIGHMP = 13, LOWHPCRIT = 14, ANTIAIR = 15,
	SWORDDMG = 21, MACEDMG = 22, AXEDMG = 23, BOWDMG = 24, FIREMDMG = 25, ICEMDMG = 26, LIGHTMDMG = 27, POISONMDMG = 28;
	public static Trait[] traits;

	public static float[] atributes;
	public static float[] offensives;
	public static float[] defensives;
	public static float[] utils;
	
	void Awake(){
		traits = new Trait[CantTraits];
		traits [MPDAMAGE] = new Trait("Spirit Guard",2,"25% of incoming damage applies on MP instead of HP");
		traits [MPREGEN] = new Trait("Focus",2,"Half MP, double MP regeneration");
		traits [MPLEECH] = new Trait("Energy Thirst",2,"Life on hit applies to MP instead of HP");
		traits [ACCURACY] = new Trait("Restraint",2,"100% accuracy, 0% Critical hit chance");
		traits [PDAMAGE] = new Trait("Berserker",2,"+50% physical damage, -25% defense");
		traits [MDAMAGE] = new Trait("Magic Overflow",2,"+50% magic damage, -30 to all resistances");
		traits [SEFFECT] = new Trait("Pandora's Box",6,"+20% chance on hit to cause a random status effect");
		traits [LOWHPDAMAGE] = new Trait("Fatal Awareness",4,"+25% damage while below 30% HP");
		traits [NOFREEZE] = new Trait("Freeze Immunity",6,"Can't be frozen");
		traits [NOSHOCK] = new Trait("Shock Immunity",6,"Can't be shocked");
		traits [NOPOISON] = new Trait("Poison Immunity",6,"Can't be poisoned");
		traits [NOBURN] = new Trait("Burn Immunity",6,"Can't be burned");
		traits [FIREDAMAGE] = new Trait("Fire Funneling",4,"10% of damage dealt is added as fire damage");
		traits [COLDDAMAGE] = new Trait("Cold Funneling",4,"10% of damage dealt is added as cold damage");
		traits [LIGHTDAMAGE] = new Trait("Lightning Funneling",4,"10% of damage dealt is added as lightning damage");
		traits [POISONDAMAGE] = new Trait("Poison Funneling",4,"10% of damage dealt is added as poison damage");
		traits [CRITACC] = new Trait("Recklessness",2,"-50% Accuracy, +100% critical chance");
		traits [BLOCKDMG] = new Trait("Thorned Shield",4,"Deal half of your thorns damage when blocking an attack");
		traits [HIGHMP] = new Trait("Energy Barrier",4,"25% reduced damage taken while above 80% MP");
		traits [LOWHPCRIT] = new Trait("Killer Instinct",4,"100% critical chance while below 15% HP");
		traits [ANTIAIR] = new Trait("Bird Hunter",4,"+20% damage to flying enemies");
		traits [SWORDDMG] = new Trait("Swordsman",1,"+10% physical damage when wielding a sword");
		traits [MACEDMG] = new Trait("Maceman",2,"+10% physical damage when wielding a mace");
		traits [AXEDMG] = new Trait("Axeman",2,"+10% physical damage when wielding a axe");
		traits [BOWDMG] = new Trait("Bowman",2,"+10% physical damage when wielding a bow");
		traits [FIREMDMG] = new Trait("Fire Mage",2,"+10% magic damage for fire spells");
		traits [ICEMDMG] = new Trait("Ice Mage",2,"+10% magic damage for ice spells");
		traits [LIGHTMDMG] = new Trait("Lightning Mage",2,"+10% magic damage for lightning spells");
		traits [POISONMDMG] = new Trait("Poison Mage",2,"+10% magic damage for poison spells");
		/*traits [t15] = new Trait("t15",2,"Double hit");
		traits [t16] = new Trait("t16",2,"10% chance to counterattack with one of your skills when struck");
		traits [t17] = new Trait("t17",2,"Attacks cause area damage");
		traits [t18] = new Trait("t18",2,"Your thorns damage causes bleeding");
		traits [t19] = new Trait("t19",2,"Half HP, critical hit chance applies to certain damage chance");*/

		/*atributes = new float[p.CantAtributes];
		offensives = new float[p.CantOffensives];
		defensives = new float[p.CantDefensives];
		utils = new float[p.CantUtils]; */
	}

	public static void init(){
		for (int i=0; i<CantTraits; i++) {
			if (Traits.traits [i].isActive ())
				activate (i);
		}
	}

	public static void reset(){
		for (int i=0; i<CantTraits; i++) {
			if (Traits.traits [i].isActive ())
				deactivate (i);
		}
	}
	public static void activate(int tName){
			if (p.passivePoints >= traits [tName].getCost ()) {
				p.passivePoints -= traits [tName].getCost ();
				switch (tName) {
				case PDAMAGE:
					p.offensives[p.IncreasedDmg] += 50;
					p.defensives[p.Defense] *= 0.75f;
					break;
				case MDAMAGE:
					p.offensives[p.IncreasedMgDmg] += 50;
					p.defensives[p.AllRes] -= 30;
					break;
				case CRITACC:
					p.offensives[p.IncreasedAccuracy] -= 50;
					p.defensives[p.IncreasedCritChance] += 100;
					break;
				default :
					break;
				}
				traits [tName].setActive (true);
			}
		
	}
	public static void deactivate(int tName){
		p.passivePoints += traits [tName].getCost ();
		switch (tName) {
		case PDAMAGE:
			p.offensives[p.IncreasedDmg] -= 50;
			p.defensives[p.Defense] *= 1.25f;
			break;
		case MDAMAGE:
			p.offensives[p.IncreasedMgDmg] -= 50;
			p.defensives[p.AllRes] += 30;
			break;
		case CRITACC:
			p.offensives[p.IncreasedAccuracy] += 50;
			p.offensives[p.IncreasedCritChance] -= 100;
			break;
		default :
			break;
		}
		traits [tName].setActive (false);
	}
}

