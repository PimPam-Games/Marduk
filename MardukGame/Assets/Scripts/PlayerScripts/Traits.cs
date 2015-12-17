using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using p = PlayerStats;

public class Traits: MonoBehaviour
{
	public const int CantTraits = 21;
	public const int MPDAMAGE = 0, MPREGEN = 1, MPLEECH = 2, ACCURACY = 3, PDAMAGE = 4, MDAMAGE= 5, SEFFECT = 6, LOWHPDAMAGE = 7, NOFREEZE = 8, NOSHOCK = 9, 
	NOPOISON = 10, NOBURN = 11, FIREDAMAGE = 12, COLDDAMAGE = 13,LIGHTDAMAGE = 14, POISONDAMAGE= 15, CRITACC = 16, BLOCKDMG = 17, HIGHMP = 18, LOWHPCRIT = 19, ANTIAIR = 20;
	public static Trait[] traits;

	public static float[] atributes;
	public static float[] offensives;
	public static float[] defensives;
	public static float[] utils;
	
	void Awake(){
		traits = new Trait[CantTraits];
		traits [MPDAMAGE] = new Trait("MPDAMAGE",1,"25% of incoming damage applies on MP instead of HP");
		traits [MPREGEN] = new Trait("MPREGEN",1,"Half MP, double MP regeneration");
		traits [MPLEECH] = new Trait("MPLEECH",1,"Life on hit applies to MP instead of HP");
		traits [ACCURACY] = new Trait("ACCURACY",1,"100% accuracy, 0% hit chance");
		traits [PDAMAGE] = new Trait("PDAMAGE",1,"+50% physical damage, -25% defense");
		traits [MDAMAGE] = new Trait("MDAMAGE",1,"+50% magic damage, -30 to all resistances");
		traits [SEFFECT] = new Trait("SEFFECT",1,"+20% chance on hit to cause a random status effect");
		traits [LOWHPDAMAGE] = new Trait("LOWHPDAMAGE",1,"+25% damage while below 30% HP");
		traits [NOFREEZE] = new Trait("NOFREEZE",1,"Can't be frozen");
		traits [NOSHOCK] = new Trait("NOSHOCK",1,"Can't be shocked");
		traits [NOPOISON] = new Trait("NOPOISON",1,"Can't be poisoned");
		traits [NOBURN] = new Trait("NOBURN",1,"Can't be burned");
		traits [FIREDAMAGE] = new Trait("FIREDAMAGE",1,"10% of damage dealt is added as fire damage");
		traits [COLDDAMAGE] = new Trait("COLDDAMAGE",1,"10% of damage dealt is added as cold damage");
		traits [LIGHTDAMAGE] = new Trait("LIGHTDAMAGE",1,"10% of damage dealt is added as lightning damage");
		traits [POISONDAMAGE] = new Trait("POISONDAMAGE",1,"10% of damage dealt is added as poison damage");
		traits [CRITACC] = new Trait("CRITACC",1,"-50% Accuracy, +100% critical chance");
		traits [BLOCKDMG] = new Trait("BLOCKDMG",1,"Deal half of your thorns damage when blocking an attack");
		traits [HIGHMP] = new Trait("HIGHMP",1,"25% reduced damage taken while above 80% MP");
		traits [LOWHPCRIT] = new Trait("LOWHPCRIT",1,"100% critical chance while below 15% HP");
		traits [ANTIAIR] = new Trait("ANTIAIR",1,"+20% damage to flying enemies");
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
				case MDAMAGE:
					p.defensives[p.AllRes] -= 30;
					break;
				case CRITACC:
					p.offensives[p.IncreasedAccuracy] -= 50;
					p.offensives[p.IncreasedCritChance] += 100;
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
		case MDAMAGE:
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

