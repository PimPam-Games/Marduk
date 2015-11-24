using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using p = PlayerStats;

public class Traits: MonoBehaviour
{
	public const int CantTraits = 20;
	public const int MPDAMAGE = 0, MPREGEN = 1, MPLEECH = 2, ACCURACY = 3, PDAMAGE = 4, MDAMAGE= 5, ASPEED = 6, LOWHPDAMAGE = 7, NOFREEZE = 8, NOSHOCK = 9, 
	NOPOISON = 10, NOBURN = 11, t12 = 12, t13 = 13,t14 = 14, t15= 15, t16 = 16, t17 = 17, t18 = 18, t19 = 19;
	public static Trait[] traits;

	public static float[] atributes;
	public static float[] offensives;
	public static float[] defensives;
	public static float[] utils;
	
	void Awake(){
		traits = new Trait[CantTraits];
		traits [MPDAMAGE] = new Trait("MPDAMAGE",1,"25% of incoming damage applies on MP instead of HP");
		traits [MPREGEN] = new Trait("MPREGEN",1,"Half MP, double MP regeneration"); //este hay que verlo
		traits [MPLEECH] = new Trait("MPLEECH",1,"Life on hit applies to MP instead of HP");
		traits [ACCURACY] = new Trait("ACCURACY",1,"100% accuracy, no critical hit chance");
		traits [PDAMAGE] = new Trait("PDAMAGE",1,"+50% physical damage, -25% defense");
		traits [MDAMAGE] = new Trait("MDAMAGE",1,"+50% magic damage, -30 to all resistances");
		traits [ASPEED] = new Trait("ASPEED",1,"+20% attack speed limit");
		traits [LOWHPDAMAGE] = new Trait("LOWHPDAMAGE",1,"+25% damage when below 30% HP");
		traits [NOFREEZE] = new Trait("NOFREEZE",1,"Can't be frozen");
		traits [NOSHOCK] = new Trait("NOSHOCK",1,"Can't be shocked");
		traits [NOPOISON] = new Trait("NOPOISON",1,"Can't be poisoned");
		traits [NOBURN] = new Trait("NOBURN",1,"Can't be burned");
		traits [t12] = new Trait("t12",2,"Can't evade attacks, +100% critical chance");
		traits [t13] = new Trait("t13",2,"+10 to resistances limit");
		traits [t14] = new Trait("t14",2,"+10% chance on hit to cause a random status effect");
		traits [t15] = new Trait("t15",2,"Double hit");
		traits [t16] = new Trait("t16",2,"10% chance to counterattack with one of your skills when struck");
		traits [t17] = new Trait("t17",2,"Attacks cause area damage");
		traits [t18] = new Trait("t18",2,"Your thorns damage causes bleeding");
		traits [t19] = new Trait("t19",2,"Half HP, critical hit chance applies to certain damage chance");

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
		default :
			break;
		}
		traits [tName].setActive (false);
	}
}

