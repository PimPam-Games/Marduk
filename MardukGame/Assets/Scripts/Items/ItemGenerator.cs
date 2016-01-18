using UnityEngine;
using System.Collections;
using p = PlayerStats;

public class ItemGenerator :MonoBehaviour{

	public const int CantUniques = 32;
	public const int Babeblade = 0, DeathOmen = 1, ColdZero = 2, Sandblast = 3, Viper = 4, Dracule = 5, EnlilBreath = 6, SolarEdge = 7, TitanMight = 8, GoldenChampion = 9, 
	Berserker = 10, Untouchable = 11, DarkWill = 12, GoldenCap = 13, CrusaderWrath = 14, TemplarFaith= 15, Magi = 16, Contender = 17, IceDragon = 18, Wall = 19, Thunderstrike = 20,
	ConquerorAmbition = 21, ViciousHunter = 22, Redeemer = 23, HeadCrusher = 24, OgreMace = 25, HolySceptre = 26, BlackSkull = 27, Raider = 28, YbabEbab = 29, AbbadonTreasure = 30,
	NergalHate = 31;

	private Object[] weaponList;
	private Object[] uniqueList;
	private Object[] skillList;


	void Awake(){
		weaponList = Resources.LoadAll("Weapons", typeof(Object));
		uniqueList = Resources.LoadAll ("Unique", typeof(Object));
		skillList = Resources.LoadAll ("SkillItems", typeof(Object));
	}

	public void createInitWeapon(Vector3 position, Quaternion rotation){
		Object weap =  Resources.Load("Weapons/Arming sword", typeof(UnityEngine.Object));
		GameObject newWeapon = (GameObject)Instantiate (weap,position,rotation);
		Item newItem = newWeapon.GetComponent<Item> ();
		newItem.Rarity = RarityTypes.Normal;
		newItem.type = ItemTypes.Weapon;
		newItem.Offensives [p.MinDmg] = 1;
		newItem.Offensives [p.MaxDamge] = 2;	
		newItem.Offensives [p.BaseAttacksPerSecond] = 1.15f;
		if (newItem.auraRend != null) {
			newItem.auraRend.sprite = GameController.auraRenders[0]; //el color del aura del item, dependiendo si es magico, normal , etc
		}
		DontDestroyOnLoad (newWeapon);
	}

	public void CreateItem(Vector3 position, Quaternion rotation){
		//crea una nueva arma
		GameObject newWeapon = null;
		Item newItem = null;
		//float[] rarityProb = {0.25f,0.25f,0.25f,0.25f};
		//float[] rarityProb = {0.51f,0.2f,0.08f,0.01f}; // 61% normal, %30 magico, %8 raro , %1 unico hay que ver que onda aca
		//int newRarity = Utils.Choose (rarityProb); 
		int newRarity = Utils.ChooseItem(); 
		if (newRarity != 3) { //no es unico
			if(newRarity == 4){ // es un skill
				int i = Random.Range (0, skillList.Length);
				newWeapon = (GameObject)Instantiate (skillList [i], position, rotation);
				newItem = newWeapon.GetComponent<Item> ();
				//DontDestroyOnLoad(newWeapon);
				return;
			}
			else{ //no es skill
				int i = Random.Range (0, weaponList.Length);
				newWeapon = (GameObject)Instantiate (weaponList [i], position, rotation);
				newItem = newWeapon.GetComponent<Item> ();
			}
		} else { //es unico
			int i = Random.Range (0, uniqueList.Length);
			newWeapon = (GameObject)Instantiate (uniqueList [i], position, rotation);
			newItem = newWeapon.GetComponent<Item> ();
		}
		newItem.Rarity = (RarityTypes) newRarity; // 0 = normal, 1 = magico , 2 = raro , 3 = unico
		if (newItem.auraRend != null) {
			newItem.auraRend.sprite = GameController.auraRenders[newRarity]; //el color del aura del item, dependiendo si es magico, normal , etc
		}

		if (newItem.Rarity != RarityTypes.Normal) {
			int numAffixes = 1; 
			if (newItem.Rarity == RarityTypes.Magic)
				numAffixes = Random.Range (1, 3);
			if (newItem.Rarity == RarityTypes.Rare)
				numAffixes = Random.Range (3, 5);
			if (newItem.Rarity == RarityTypes.Unique){
				numAffixes = 1;
				switch (newItem.uniqueIndex){
				case Babeblade:
					newItem.Defensives [p.MaxHealth] = Random.Range (50, 101);
					newItem.Defensives [p.AllRes] = 5;
					newItem.Offensives [p.IncreasedMgDmg] = (float)Random.Range (15, 21);
					break;
				case DeathOmen:
					newItem.Offensives [p.IncreasedCritChance] = (float)Random.Range (25, 31);
					newItem.Defensives [p.AllRes] = -15;
					newItem.Defensives [p.Thorns] = 5;
					break;
				case ColdZero:
					newItem.Offensives [p.IncreasedCritChance] = 15;
					newItem.Defensives [p.ColdRes] = 30;
					newItem.Offensives [p.IncreasedCastSpeed] = (float)Random.Range (10, 21);
					break;
				case Sandblast:
					newItem.Atributes [p.Vitality] = 15;
					newItem.Offensives [p.IncreasedAttackSpeed] = (float)Random.Range (12, 16);
					break;
				case Viper:
					newItem.Atributes [p.Dextery] = 15;
					newItem.Defensives [p.PoisonRes] = 20;
					newItem.Offensives [p.IncreasedAttackSpeed] = (float)Random.Range (8, 11);
					break;
				case Dracule:
					newItem.Defensives [p.LifePerHit] = (float)System.Math.Round (Random.Range (2f, 4f), 2);
					newItem.Offensives [p.IncreasedMgDmg] = 15;
					break;
				case EnlilBreath:
					newItem.Defensives [p.ColdRes] = 15;
					newItem.Defensives [p.LightRes] = 15;
					newItem.Utils [p.AllAttr] = 10;
					break;
				case SolarEdge:
					newItem.Utils [p.AllAttr] = 10;
					newItem.Defensives [p.FireRes] = 30;
					newItem.Offensives [p.IncreasedCritChance] = (float)Random.Range (15, 21);
					break;
				case TitanMight:
					newItem.Atributes [p.Strength] = 30;
					newItem.Offensives [p.IncreasedDmg] = (float)Random.Range (15, 21);
					break;
				case GoldenChampion:
					newItem.Atributes [p.Strength] = 20;
					newItem.Offensives [p.IncreasedCritChance] = 30;
					break;
				case Berserker:
					newItem.Atributes [p.Dextery] = 30;
					newItem.Offensives [p.IncreasedAttackSpeed] = (float)Random.Range (10, 16);
					break;
				case Untouchable:
					newItem.Atributes [p.Strength] = 15;
					newItem.Offensives [p.IncreasedDmg] = (float)Random.Range (10, 16);
					newItem.Defensives [p.IncreasedDefense] = 25;
					break;
				case DarkWill:
					newItem.Atributes [p.Dextery] = 20;
					newItem.Defensives [p.Thorns] = (float)Random.Range (7, 11);
					break;
				case GoldenCap:
					newItem.Offensives [p.IncreasedMgDmg] = 20;
					newItem.Offensives [p.IncreasedCastSpeed] = 12;
					newItem.Defensives [p.AllRes] = 10;
					break;
				case CrusaderWrath:
					newItem.Offensives [p.IncreasedCritChance] = (float)Random.Range (10, 15);
					newItem.Utils [p.AllAttr] = 10;
					break;
				case TemplarFaith:
					newItem.Atributes [p.Vitality] = 15;
					newItem.Atributes [p.Spirit] = 15;
					newItem.Defensives [p.AllRes] = 15;
					break;
				case Magi:
					newItem.Offensives [p.IncreasedMgDmg] = 20;
					newItem.Offensives [p.IncreasedCastSpeed] = 20;
					break;
				case Contender:
					newItem.Atributes [p.Vitality] = 12;
					newItem.Atributes [p.Strength] = 12;
					newItem.Defensives[p.IncreasedEvasion] = 15;
					break;
				case IceDragon:
					newItem.Defensives [p.ColdRes] = 25;
					newItem.Utils [p.AllAttr] = 5;
					newItem.Defensives[p.IncreasedDefense] = 35;
					break;
				case Wall:
					newItem.Defensives [p.MaxHealth] = Random.Range (30, 40);
					newItem.Defensives [p.AllRes] = 15;
					break;
				case Thunderstrike:
					newItem.Defensives [p.LightRes] = 20;
					newItem.Offensives [p.IncreasedAttackSpeed] = (float)Random.Range (10, 15);
					newItem.Utils [p.AllAttr] = 5;
					break;
				case ConquerorAmbition:
					newItem.Defensives [p.MaxHealth] = Random.Range (20, 30);
					newItem.Utils [p.AllAttr] = 15;
					break;
				case ViciousHunter:
					newItem.Defensives [p.LifePerHit] = (float)System.Math.Round (Random.Range (1f, 2f), 2);
					newItem.Offensives [p.IncreasedCritChance] = (float)Random.Range (10, 16);
					break;
				case Redeemer:
					newItem.Offensives [p.IncreasedMgDmg] = 10;
					newItem.Atributes [p.Spirit] = 20;
					newItem.Defensives [p.AllRes] = 10;
					break;
				case HeadCrusher:
					newItem.Offensives [p.IncreasedCritChance] = (float)Random.Range (20, 31);
					newItem.Offensives [p.IncreasedDmg] = 20;
					break;
				case OgreMace:
					newItem.Defensives [p.PoisonRes] = 15;
					newItem.Defensives [p.Thorns] = 7;
					break;
				case HolySceptre:
					newItem.Atributes [p.Dextery] = 20;
					newItem.Atributes [p.Spirit] = 20;
					newItem.Defensives [p.AllRes] = 20;
					break;
				case BlackSkull:
					newItem.Offensives [p.IncreasedMgDmg] = 10;
					newItem.Atributes [p.Spirit] = 15;
					newItem.Offensives [p.IncreasedCastSpeed] = (float)Random.Range (10, 21);
					break;
				case Raider:
					newItem.Atributes [p.Dextery] = 30;
					newItem.Defensives [p.LifePerHit] = (float)System.Math.Round (Random.Range (1f, 2f), 2);
					newItem.Offensives [p.IncreasedAttackSpeed] = 10;
					break;
				case YbabEbab:
					newItem.Utils [p.AllAttr] = 5;
					newItem.Defensives [p.IncreasedEvasion] = 30;
					break;
				case AbbadonTreasure:
					newItem.Utils [p.AllAttr] = 15;
					newItem.Defensives [p.LifePerHit] = (float)System.Math.Round (Random.Range (1f, 2f), 2);
					newItem.Defensives [p.AllRes] = -25;
					break;
				case NergalHate:
					newItem.Defensives [p.PoisonRes] = 30;
					newItem.Offensives [p.IncreasedMgDmg] = 10;
					newItem.Offensives [p.IncreasedAttackSpeed] = 10;
					break;
				default:
					break;
				}



			}

			for (int j=0; j<numAffixes; j++) {
				int defAtrOff = Random.Range (0, 3);
				bool ok = false;
				switch (defAtrOff) {
				case 0:
					while (ok == false) {
						int optionDef = Random.Range (0, p.CantDefensives);
						if (newItem.Defensives [optionDef] > 0 || optionDef == 1 || optionDef == 6) //si ya esta usada esta opcion en el item
							continue;
						ok = true;
						if (optionDef == p.LifePerSecond)
							newItem.Defensives [optionDef] = (float)System.Math.Round (Random.Range (0.4f, 1f), 2);
						if (optionDef == p.Thorns)
							newItem.Defensives [optionDef] = (float)System.Math.Round (Random.Range (0.2f, 2f), 2);
						if (optionDef >= p.ColdRes && optionDef <= p.PoisonRes)
							newItem.Defensives [optionDef] = Random.Range (2, 6);
						if (optionDef == p.MaxHealth)
							newItem.Defensives [optionDef] = Random.Range (5, 11);
						if (optionDef == p.IncreasedDefense)
							newItem.Defensives [optionDef] = Random.Range (5, 11);
						if (optionDef == p.IncreasedEvasion)
							newItem.Defensives [optionDef] = Random.Range (5, 11);
						if (optionDef == p.LifePerHit)
							newItem.Defensives [optionDef] = (float)System.Math.Round (Random.Range (0.5f, 2f), 2);
					}
					break;
				case 1:
					while (ok == false) {
						int optionAtr = Random.Range (0, p.CantAtributes);
						if (newItem.Atributes [optionAtr] > 0)
							continue;
						ok = true;
						newItem.Atributes [optionAtr] = Random.Range (5, 11);
					}
					break;
				case 2:
					while (ok == false) {
						int optionOff = Random.Range (11, p.CantOffensives); //empieza desde 11 por que las  de antes son las esatadisticas basicas
						if(newItem == null){
							Debug.LogError("Error: new item null!!");
							return;
						}
						if (newItem.Offensives [optionOff] > 0)
							continue;
						ok = true;
						if (optionOff == p.IncreasedCritChance)
							newItem.Offensives [p.IncreasedCritChance] = (float)Random.Range (10, 16); 
						if (optionOff == p.IncreasedAttackSpeed) 
							newItem.Offensives [p.IncreasedAttackSpeed] = (float)Random.Range (5, 8);
						if (optionOff == p.IncreasedCastSpeed) 
							newItem.Offensives [p.IncreasedCastSpeed] = (float)Random.Range (5, 8); 
						if (optionOff == p.IncreasedMgDmg)
							newItem.Offensives [p.IncreasedMgDmg] = (float)Random.Range (5, 11);
						if (optionOff == p.IncreasedDmg)
							newItem.Offensives [p.IncreasedDmg] = (float)Random.Range (5, 11);
						if (optionOff == p.IncreasedAccuracy)
							newItem.Offensives [p.IncreasedAccuracy] = (float)Random.Range (5, 11);						
					}
					break;
				default:
					break;
				}

			}

		}
		DontDestroyOnLoad (newWeapon);
	}


}
