using UnityEngine;
using System.Collections;
using p = PlayerStats;
using g = GameController;

public class ItemGenerator :MonoBehaviour{

	public const int CantUniques = 32;
	public const int Babeblade = 0, DeathOmen = 1, ColdZero = 2, Sandblast = 3, Viper = 4, Dracule = 5, EnlilBreath = 6, SolarEdge = 7, TitanMight = 8, GoldenChampion = 9, 
	Berserker = 10, Untouchable = 11, DarkWill = 12, GoldenCap = 13, CrusaderWrath = 14, TemplarFaith= 15, Magi = 16, Contender = 17, IceDragon = 18, Wall = 19, Thunderstrike = 20,
	ConquerorAmbition = 21, ViciousHunter = 22, Redeemer = 23, HeadCrusher = 24, OgreMace = 25, HolySceptre = 26, BlackSkull = 27, Raider = 28, YbabEbab = 29, AbbadonTreasure = 30,
	NergalHate = 31;

	private Object[] uniqueList;
	private Object[] skillList;


	void Awake(){
		uniqueList = Resources.LoadAll ("Unique", typeof(Object));
		skillList = Resources.LoadAll ("SkillItems", typeof(Object));
	}

	public void createInitWeapon(Vector3 position, Quaternion rotation){
		/*Object weap =  Resources.Load("Weapons/Arming sword", typeof(UnityEngine.Object));
		GameObject newWeapon = (GameObject)Instantiate (weap,position,rotation);
		Item newItem = newWeapon.GetComponent<Item> ();
		newItem.Rarity = RarityTypes.Normal;
		newItem.type = ItemTypes.Weapon;
		newItem.Offensives [p.MinDmg] = 2;
		newItem.Offensives [p.MaxDamge] = 4;	
		newItem.Offensives [p.BaseAttacksPerSecond] = 1.15f;
        newItem.GenerateBaseAffixes(1);
        if (newItem.auraRend != null) {
			newItem.auraRend.sprite = GameController.auraRenders[0]; //el color del aura del item, dependiendo si es magico, normal , etc
		}
		DontDestroyOnLoad (newWeapon);*/
	}
	//Calcula un afijo de acuerdo al nivel del item
	private float calculateStat(float baseStat, int rank){
		return (baseStat * Mathf.Floor (0.2f * rank + 1));
	}
	public void CreateItem(Vector3 position, Quaternion rotation, EnemyStats eStats){
		//crea una nueva arma
		GameObject newWeapon = null;
		Item newItem = null;
        //float[] rarityProb = {0.25f,0.25f,0.25f,0.25f};
        //float[] rarityProb = {0.51f,0.2f,0.08f,0.01f}; // 61% normal, %30 magico, %8 raro , %1 unico hay que ver que onda aca
        //int newRarity = Utils.Choose (rarityProb); 
        float[] dropItemProb = { 0.75f, 0.25f }; //chance de tirar un item cuando no es un enemigo el que lo tira por ejemplo un cofre
        if (eStats != null) {
            switch (eStats.enemyType)//calcula la chance de dropear segun el tipo de enemigo
            {
                case Types.EnemyTypes.Common:
                    dropItemProb[0] = 0.18f; dropItemProb[1] = 0.82f; 
                    break;
                case Types.EnemyTypes.Champion:
                    dropItemProb[0] = 0.4f; dropItemProb[1] = 0.6f;
                    break;
                case Types.EnemyTypes.MiniBoss:
                    dropItemProb[0] = 0.85f; dropItemProb[1] = 0.15f;
                    break;
                case Types.EnemyTypes.Boss:
                    dropItemProb[0] = 0.99f; dropItemProb[1] = 0.01f;
                    break;
            }           
        }
        if (Utils.Choose(dropItemProb) != 0)
            return;
        int newRarity = 0; // 0 = normal, 1 = magico , 2 = raro , 3 = unico
        if (eStats != null)
            newRarity = Utils.ChooseItem(eStats.enemyType);
        else
            newRarity = Utils.ChooseItem(Types.EnemyTypes.Champion); // si es un cofre es como si fuera un enemigo tipo champion
        /* calcula el nivel del item*/
        int newItemLevel = 1;
        if (eStats != null)
        {
            int minValue = eStats.lvl - 3;
            if (minValue <= 0)
                minValue = 1;
            newItemLevel = Random.Range(minValue, eStats.lvl + 1); //el +1 es por que el int max es exclusivo en random.Range
        }
        /* -----------------------------------------*/
       // Debug.Log("item level " + newItemLevel);
        if (newRarity != 3) { //no es unico
			if(newRarity == 4){ // es un skill
				int i = Random.Range (0, skillList.Length);
				newWeapon = (GameObject)Instantiate (skillList [i], position, rotation);
				newItem = newWeapon.GetComponent<Item> ();
				//DontDestroyOnLoad(newWeapon);
				return;
			}
			else{ //no es skill
				int i = Random.Range (0, g.ItemsList.Length); //tira un random para elegir el tipo de item
                int j = Random.Range(0, g.ItemsList[i].Length); //elige el item dentro del tipo elegido
                newWeapon = (GameObject)Instantiate (g.ItemsList[i][j], position, rotation);
				newItem = newWeapon.GetComponent<Item> ();
				if ((newItem.Type == ItemTypes.Amulet || newItem.Type == ItemTypes.Ring) && newRarity == 0) {
					newRarity = 1; // 1 = magic., Los anillos y amuletos no son nunca normales
				}
                newItem.GenerateBaseAffixes(newItemLevel);
			}
		} else { //es unico
			int i = Random.Range (0, uniqueList.Length);
			newWeapon = (GameObject)Instantiate (uniqueList [i], position, rotation);
			newItem = newWeapon.GetComponent<Item> ();
            newItem.GenerateBaseAffixes(newItemLevel);
        }
		newItem.Rarity = (RarityTypes) newRarity; // 0 = normal, 1 = magico , 2 = raro , 3 = unico
		if (newItem.auraRend != null) {
			newItem.auraRend.sprite = GameController.auraRenders[newRarity]; //el color del aura del item, dependiendo si es magico, normal , etc
		}
		int rank = newItem.itemRank;
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
					newItem.Defensives [p.MaxHealth] = calculateStat(Random.Range (50, 101),rank);
					newItem.Defensives [p.AllRes] = calculateStat(5,rank);
					newItem.Offensives [p.IncreasedMgDmg] = calculateStat((float)Random.Range (15, 21),rank);
					break;
				case DeathOmen:
					newItem.Offensives [p.IncreasedCritChance] = calculateStat((float)Random.Range (25, 31),rank);
					newItem.Defensives [p.AllRes] = calculateStat(-15,rank);
					newItem.Defensives [p.Thorns] = calculateStat(5,rank);
					break;
				case ColdZero:
					newItem.Offensives [p.IncreasedCritChance] = calculateStat(15,rank);
					newItem.Defensives [p.ColdRes] = calculateStat(30,rank);
					newItem.Offensives [p.IncreasedCastSpeed] = calculateStat((float)Random.Range (10, 21),rank);
					break;
				case Sandblast:
					newItem.Atributes [p.Vitality] = calculateStat(15,rank);
					newItem.Offensives [p.IncreasedAttackSpeed] = calculateStat ((float)Random.Range (12, 16),rank);
					break;
				case Viper:
					newItem.Atributes [p.Dextery] = calculateStat(15,rank);
					newItem.Defensives [p.PoisonRes] = calculateStat(20,rank);
					newItem.Offensives [p.IncreasedAttackSpeed] = calculateStat ((float)Random.Range (8, 11),rank);
					break;
				case Dracule:
					newItem.Defensives [p.LifePerHit] = calculateStat ((float)System.Math.Round (Random.Range (2f, 4f), 2),rank);
					newItem.Offensives [p.IncreasedMgDmg] = calculateStat(15,rank);
					break;
				case EnlilBreath:
					newItem.Defensives [p.ColdRes] = calculateStat(15,rank);
					newItem.Defensives [p.LightRes] = calculateStat(15,rank);
					newItem.Utils [p.AllAttr] = calculateStat(10,rank);
					break;
				case SolarEdge:
					newItem.Utils [p.AllAttr] = calculateStat(10,rank);
					newItem.Defensives [p.FireRes] = calculateStat(30,rank);
					newItem.Offensives [p.IncreasedCritChance] = calculateStat((float)Random.Range (15, 21),rank);
					break;
				case TitanMight:
					newItem.Atributes [p.Strength] = calculateStat(30,rank);
					newItem.Offensives [p.IncreasedDmg] = calculateStat((float)Random.Range (15, 21),rank);
					break;
				case GoldenChampion:
					newItem.Atributes [p.Strength] = calculateStat(20,rank);
					newItem.Offensives [p.IncreasedCritChance] = calculateStat(30,rank);
					break;
				case Berserker:
					newItem.Atributes [p.Dextery] = calculateStat(30,rank);
					newItem.Offensives [p.IncreasedAttackSpeed] = calculateStat((float)Random.Range (10, 16),rank);
					break;
				case Untouchable:
					newItem.Atributes [p.Strength] = calculateStat(15,rank);
					newItem.Offensives [p.IncreasedDmg] = calculateStat((float)Random.Range (10, 16),rank);
					newItem.Defensives [p.IncreasedDefense] = calculateStat(25,rank);
					break;
				case DarkWill:
					newItem.Atributes [p.Dextery] = calculateStat(20,rank);
					newItem.Defensives [p.Thorns] = calculateStat((float)Random.Range (7, 11),rank);
					break;
				case GoldenCap:
					newItem.Offensives [p.IncreasedMgDmg] = calculateStat(20,rank);
					newItem.Offensives [p.IncreasedCastSpeed] = calculateStat(12,rank);
					newItem.Defensives [p.AllRes] = calculateStat(10,rank);
					break;
				case CrusaderWrath:
					newItem.Offensives [p.IncreasedCritChance] = calculateStat((float)Random.Range (10, 15),rank);
					newItem.Utils [p.AllAttr] = calculateStat(10,rank);
					break;
				case TemplarFaith:
					newItem.Atributes [p.Vitality] = calculateStat(15,rank);
					newItem.Atributes [p.Spirit] = calculateStat(15,rank);
					newItem.Defensives [p.AllRes] = calculateStat(15,rank);
					break;
				case Magi:
					newItem.Offensives [p.IncreasedMgDmg] = calculateStat(20,rank);
					newItem.Offensives [p.IncreasedCastSpeed] = calculateStat(20,rank);
					break;
				case Contender:
					newItem.Atributes [p.Vitality] = calculateStat(12,rank);
					newItem.Atributes [p.Strength] = calculateStat(12,rank);
					//newItem.Defensives[p.IncreasedEvasion] = 15;
					break;
				case IceDragon:
					newItem.Defensives [p.ColdRes] = calculateStat(25,rank);
					newItem.Utils [p.AllAttr] = calculateStat(5,rank);
					newItem.Defensives[p.IncreasedDefense] = calculateStat(35,rank);
					break;
				case Wall:
					newItem.Defensives [p.MaxHealth] = calculateStat(Random.Range (30, 40),rank);
					newItem.Defensives [p.AllRes] = calculateStat(15,rank);
					break;
				case Thunderstrike:
					newItem.Defensives [p.LightRes] = calculateStat(20,rank);
					newItem.Offensives [p.IncreasedAttackSpeed] = calculateStat((float)Random.Range (10, 15),rank);
					newItem.Utils [p.AllAttr] = calculateStat(5,rank);
					break;
				case ConquerorAmbition:
					newItem.Defensives [p.MaxHealth] = calculateStat(Random.Range (20, 30),rank);
					newItem.Utils [p.AllAttr] = calculateStat(15,rank);
					break;
				case ViciousHunter:
					newItem.Defensives [p.LifePerHit] = calculateStat((float)System.Math.Round (Random.Range (1f, 2f), 2),rank);
					newItem.Offensives [p.IncreasedCritChance] = calculateStat((float)Random.Range (10, 16),rank);
					break;
				case Redeemer:
					newItem.Offensives [p.IncreasedMgDmg] = calculateStat(10,rank);
					newItem.Atributes [p.Spirit] = calculateStat(20,rank);
					newItem.Defensives [p.AllRes] = calculateStat(10,rank);
					break;
				case HeadCrusher:
					newItem.Offensives [p.IncreasedCritChance] = calculateStat((float)Random.Range (20, 31),rank);
					newItem.Offensives [p.IncreasedDmg] = calculateStat(20,rank);
					break;
				case OgreMace:
					newItem.Defensives [p.PoisonRes] = calculateStat(15,rank);
					newItem.Defensives [p.Thorns] = calculateStat(7,rank);
					break;
				case HolySceptre:
					newItem.Atributes [p.Dextery] = calculateStat(20,rank);
					newItem.Atributes [p.Spirit] = calculateStat(20,rank);
					newItem.Defensives [p.AllRes] = calculateStat(20,rank);
					break;
				case BlackSkull:
					newItem.Offensives [p.IncreasedMgDmg] = calculateStat(10,rank);
					newItem.Atributes [p.Spirit] = calculateStat(15,rank);
					newItem.Offensives [p.IncreasedCastSpeed] = calculateStat((float)Random.Range (10, 21),rank);
					break;
				case Raider:
					newItem.Atributes [p.Dextery] = 30;
					newItem.Defensives [p.LifePerHit] = calculateStat((float)System.Math.Round (Random.Range (1f, 2f), 2),rank);
					newItem.Offensives [p.IncreasedAttackSpeed] = calculateStat(10,rank);
					break;
				case YbabEbab:
					newItem.Utils [p.AllAttr] = calculateStat(20,rank);
					//newItem.Defensives [p.IncreasedEvasion] = 30;
					break;
				case AbbadonTreasure:
					newItem.Utils [p.AllAttr] = calculateStat(15,rank);
					newItem.Defensives [p.LifePerHit] = calculateStat((float)System.Math.Round (Random.Range (1f, 2f), 2),rank);
					newItem.Defensives [p.AllRes] = calculateStat(-25,rank);
					break;
				case NergalHate:
					newItem.Defensives [p.PoisonRes] = calculateStat(30,rank);
					newItem.Offensives [p.IncreasedMgDmg] = calculateStat(10,rank);
					newItem.Offensives [p.IncreasedAttackSpeed] = calculateStat(10,rank);
					break;
				default:
					break;
				}



			}

			//generator of random affixes
			for (int j=0; j<numAffixes; j++) {
				int defAtrOff = Random.Range (0, 3);
				bool ok = false;
				switch (defAtrOff) {
				case 0:
					while (ok == false) {
						int optionDef = Random.Range (0, p.CantDefensives);
						if(newItem == null){
							Debug.LogError("Error: new item null!!");
							return;
						}
						if (newItem.Defensives [optionDef] > 0 || optionDef == 1 || optionDef == 6) //si ya esta usada esta opcion en el item
							continue;
						ok = true;
						if (optionDef == p.LifePerSecond)
							newItem.Defensives [optionDef] = calculateStat((float)System.Math.Round (Random.Range (0.4f, 1f), 2),rank);
						if (optionDef == p.Thorns)
							newItem.Defensives [optionDef] = calculateStat((float)System.Math.Round (Random.Range (0.2f, 2f), 2),rank);
						if (optionDef >= p.ColdRes && optionDef <= p.PoisonRes)
							newItem.Defensives [optionDef] = calculateStat(Random.Range (2, 6),rank);
						if (optionDef == p.MaxHealth)
							newItem.Defensives [optionDef] = calculateStat(Random.Range (5, 11),rank);
						if (optionDef == p.IncreasedDefense)
							newItem.Defensives [optionDef] = calculateStat(Random.Range (5, 11),rank);
						if (optionDef == p.AllRes)
							newItem.Defensives [optionDef] = calculateStat(Random.Range (2, 4),rank);
						//if (optionDef == p.IncreasedEvasion)
						//	newItem.Defensives [optionDef] = Random.Range (5, 11);
						if (optionDef == p.LifePerHit)
							newItem.Defensives [optionDef] = calculateStat((float)System.Math.Round (Random.Range (0.5f, 2f), 2),rank);
					}
					break;
				case 1:
					while (ok == false) {
						int optionAtr = Random.Range (0, p.CantAtributes);
						if(newItem == null){
							Debug.LogError("Error: new item null!!");
							return;
						}
						if (newItem.Atributes [optionAtr] > 0)
							continue;
						ok = true;
						newItem.Atributes [optionAtr] = calculateStat(Random.Range (5, 11),rank);
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
							newItem.Offensives [p.IncreasedCritChance] = calculateStat((float)Random.Range (10, 16),rank); 
						if (optionOff == p.IncreasedAttackSpeed) 
							newItem.Offensives [p.IncreasedAttackSpeed] = calculateStat((float)Random.Range (5, 8),rank);
						if (optionOff == p.IncreasedCastSpeed) 
							newItem.Offensives [p.IncreasedCastSpeed] = calculateStat((float)Random.Range (5, 8),rank); 
						if (optionOff == p.IncreasedMgDmg)
							newItem.Offensives [p.IncreasedMgDmg] = calculateStat((float)Random.Range (5, 11),rank);
						if (optionOff == p.IncreasedDmg)
							newItem.Offensives [p.IncreasedDmg] = calculateStat((float)Random.Range (5, 11),rank);
						//if (optionOff == p.IncreasedAccuracy)
						//	newItem.Offensives [p.IncreasedAccuracy] = (float)Random.Range (5, 11);						
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
