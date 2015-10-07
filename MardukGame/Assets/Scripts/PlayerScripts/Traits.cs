using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using p = PlayerStats;

public class Traits: MonoBehaviour
{
	public const int CantTraits = 4;
	public const int NoRemorse = 0, OneWithNature = 1, IronSkin = 2, Trascendance = 3;
	public static Trait[] traits;

	public static float[] atributes;
	public static float[] offensives;
	public static float[] defensives;
	public static float[] utils;
	
	void Awake(){
		traits = new Trait[CantTraits];
		traits [NoRemorse] = new Trait("No remorse",2,"+10% physical damage");
		traits [OneWithNature] = new Trait("One with nature",2,"+5 to all resistances");
		traits [IronSkin] = new Trait("Iron skin",2,"+10% defense");
		traits [Trascendance] = new Trait("Trascendence",2,"+10 spirit");
		atributes = new float[p.CantAtributes];
		offensives = new float[p.CantOffensives];
		defensives = new float[p.CantDefensives];
		utils = new float[p.CantUtils]; 
	}

	public static void init(){
		for (int i=0; i<CantTraits; i++) {
			activate (i);
		}
	}

	public static void reset(){
		for (int i=0; i<CantTraits; i++) {
			deactivate (i);
		}
	}
	public static void activate(int tName){
		if (!traits [tName].isActive()) {
			if (p.passivePoints >= traits [tName].getCost ()) {
				p.passivePoints -= traits [tName].getCost ();
				switch (tName) {
				case NoRemorse: 
					offensives [p.IncreasedDmg] += 10;
					break;
				case OneWithNature: 
					defensives [p.AllRes] += 5;
					break;
				case IronSkin: 
					defensives [p.Defense] += 10;
					break;
				case Trascendance: 
					atributes [p.Spirit] += 10;
					break;
				default :
					break;
				}
				traits [tName].setActive (true);
			}
		}
		
	}
	public static void deactivate(int tName){
		if (traits [tName].isActive()) {
			p.passivePoints += traits [tName].getCost ();
			switch (tName) {
			case NoRemorse: 
				offensives [p.IncreasedDmg] -= 10;
				break;
			case OneWithNature: 
				defensives [p.AllRes] -= 5;
				break;
			case IronSkin: 
				defensives [p.Defense] -= 10;
				break;
			case Trascendance: 
				atributes [p.Spirit] -= 10;
				break;
			default :
				break;
			}
			traits [tName].setActive (false);
		}
	}
}

