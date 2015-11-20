using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using p = PlayerStats;

public class Traits: MonoBehaviour
{
	public const int CantTraits = 20;
	public const int t0 = 0, t1 = 1, t2 = 2, t3 = 3, t4 = 4, t5= 5, t6 = 6, t7 = 7, t8 = 8, t9 = 9, 
	t10 = 10, t11 = 11, t12 = 12, t13 = 13,t14 = 14, t15= 15, t16 = 16, t17 = 17, t18 = 18, t19 = 19;
	public static Trait[] traits;

	public static float[] atributes;
	public static float[] offensives;
	public static float[] defensives;
	public static float[] utils;
	
	void Awake(){
		traits = new Trait[CantTraits];
		traits [t0] = new Trait("t0",2,"25% of incoming damage applies on MP instead of HP");
		traits [t1] = new Trait("t1",2,"Half MP, double MP regeneration");
		traits [t2] = new Trait("t2",2,"Life on hit applies to MP instead of HP");
		traits [t3] = new Trait("t3",2,"100% accuracy, no critical hit chance");
		traits [t4] = new Trait("t4",2,"+50% physical damage, -25% defense");
		traits [t5] = new Trait("t5",2,"+50% magic damage, -30 to all resistances");
		traits [t6] = new Trait("t6",2,"+20% attack speed limit");
		traits [t7] = new Trait("t7",2,"+25% damage when below 30% HP");
		traits [t8] = new Trait("t8",2,"Can't be frozen");
		traits [t9] = new Trait("t9",2,"Can't be shocked");
		traits [t10] = new Trait("t10",2,"Can't be poisoned");
		traits [t11] = new Trait("t11",2,"Can't be burned");
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
				/*switch (tName) {
				case NoRemorse: 
					p.offensives [p.IncreasedDmg] += 10;
					break;
				case OneWithNature: 
					p.defensives [p.AllRes] += 5;
					break;
				case IronSkin: 
					p.defensives [p.Defense] += 10;
					break;
				case Trascendance: 
					p.atributes [p.Spirit] += 10;
					break;
				default :
					break;
				}*/
				traits [tName].setActive (true);
			}
		
	}
	public static void deactivate(int tName){
			p.passivePoints += traits [tName].getCost ();
			/*switch (tName) {
			case NoRemorse: 
				p.offensives [p.IncreasedDmg] -= 10;
				break;
			case OneWithNature: 
				p.defensives [p.AllRes] -= 5;
				break;
			case IronSkin: 
				p.defensives [p.Defense] -= 10;
				break;
			case Trascendance: 
				p.atributes [p.Spirit] -= 10;
				break;
			default :
				break;
			}*/
			traits [tName].setActive (false);
	}
}

