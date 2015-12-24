using UnityEngine;
using System.Collections;
using p = PlayerStats;

public class ItemGenerator :MonoBehaviour{



	private Object[] weaponList;
	private Object[] uniqueList;
	private Object[] skillList;


	void Awake(){
		weaponList = Resources.LoadAll("Weapons", typeof(Object));
		uniqueList = Resources.LoadAll ("Unique", typeof(Object));
		skillList = Resources.LoadAll ("SkillItems", typeof(Object));
	}
	void Update(){

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
		GameObject newWeapon;
		Item newItem;
		//float[] rarityProb = {0.25f,0.25f,0.25f,0.25f};
		//float[] rarityProb = {0.51f,0.2f,0.08f,0.01f}; // 61% normal, %30 magico, %8 raro , %1 unico hay que ver que onda aca
		//int newRarity = Utils.Choose (rarityProb); 
		int newRarity = Utils.ChooseItem(); 
		if (newRarity != 3) { //no es unico
			if(newRarity == 4){ // es un skill
				int i = Random.Range (0, skillList.Length);
				newWeapon = (GameObject)Instantiate (skillList [i], position, rotation);
				newItem = newWeapon.GetComponent<Item> ();
				DontDestroyOnLoad(newWeapon);
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
			int numAffixes = 1; // no se que onda esto, pero da la impresion de que a veces no entra por los casos de abajo
			if (newItem.Rarity == RarityTypes.Magic)
				numAffixes = Random.Range (1, 3);
			if (newItem.Rarity == RarityTypes.Rare)
				numAffixes = Random.Range (3, 5);
			if (newItem.Rarity == RarityTypes.Unique){
				numAffixes = 2;
				if (newItem.itemName == "Babeblade"){
					newItem.Defensives [p.MaxHealth] = Random.Range (50, 101);
					newItem.Defensives [p.AllRes] = 5;
					newItem.Offensives [p.IncreasedMgDmg] = (float)Random.Range (15, 21);
				}
				if (newItem.itemName == "Death Omen"){
					newItem.Offensives [p.IncreasedCritChance] = (float)Random.Range (25, 31);
					newItem.Defensives [p.AllRes] = -15;
					newItem.Defensives [p.Thorns] = 5;
				}
				if (newItem.itemName == "Cold Zero"){
					newItem.Offensives [p.IncreasedCritChance] = 15;
					newItem.Defensives [p.ColdRes] = 30;
					newItem.Offensives [p.IncreasedCastSpeed] = (float)Random.Range (10, 21);
				}
				if (newItem.itemName == "Sandblast"){
					newItem.Atributes [p.Vitality] = 15;
					newItem.Offensives [p.IncreasedAttackSpeed] = (float)Random.Range (12, 16);
				}
				if (newItem.itemName == "Viper"){
					newItem.Atributes [p.Dextery] = 15;
					newItem.Defensives [p.PoisonRes] = 20;
					newItem.Offensives [p.IncreasedAttackSpeed] = (float)Random.Range (8, 11);
				}
				if (newItem.itemName == "Dracule"){
					newItem.Defensives [p.LifePerHit] = (float)System.Math.Round (Random.Range (2f, 4f), 2);
					newItem.Offensives [p.IncreasedMgDmg] = 15;
				}
				if (newItem.itemName == "Enlil's Breath"){
					newItem.Defensives [p.ColdRes] = 15;
					newItem.Defensives [p.LightRes] = 15;
					newItem.Utils [p.AllAttr] = 10;
				}
				if (newItem.itemName == "Solar Edge"){
					newItem.Utils [p.AllAttr] = 10;
					newItem.Defensives [p.FireRes] = 30;
					newItem.Offensives [p.IncreasedCritChance] = (float)Random.Range (15, 21);
				}
				if (newItem.itemName == "Titan's Might"){
					newItem.Atributes [p.Strength] = 30;
					newItem.Offensives [p.IncreasedDmg] = (float)Random.Range (15, 21);
				}
				if (newItem.itemName == "Golden Champion"){
					newItem.Atributes [p.Strength] = 20;
					newItem.Offensives [p.IncreasedCritChance] = 30;
				}
				if (newItem.itemName == "The Berserker"){
					newItem.Atributes [p.Dextery] = 30;
					newItem.Offensives [p.IncreasedAttackSpeed] = (float)Random.Range (10, 16);
				}
				if (newItem.itemName == "The Untouchable"){
					newItem.Atributes [p.Strength] = 15;
					newItem.Offensives [p.IncreasedDmg] = (float)Random.Range (10, 16);
					newItem.Defensives [p.IncreasedDefense] = 25;
				}
				if (newItem.itemName == "Dark Will"){
					newItem.Atributes [p.Dextery] = 20;
					newItem.Defensives [p.Thorns] = (float)Random.Range (7, 11);
				}
				if (newItem.itemName == "Golden Cap"){
					newItem.Offensives [p.IncreasedMgDmg] = 20;
					newItem.Offensives [p.IncreasedCastSpeed] = 12;
					newItem.Defensives [p.AllRes] = 10;
				}
				if (newItem.itemName == "Crusader's Wrath"){
					newItem.Offensives [p.IncreasedCritChance] = (float)Random.Range (10, 15);
					newItem.Utils [p.AllAttr] = 10;
				}
				if (newItem.itemName == "Templar's Faith"){
					newItem.Atributes [p.Vitality] = 15;
					newItem.Atributes [p.Spirit] = 15;
					newItem.Defensives [p.AllRes] = 15;
				}
				if (newItem.itemName == "The Magi"){
					newItem.Offensives [p.IncreasedMgDmg] = 20;
					newItem.Offensives [p.IncreasedCastSpeed] = 20;
				}
				if (newItem.itemName == "The Contender"){
					newItem.Atributes [p.Vitality] = 12;
					newItem.Atributes [p.Strength] = 12;
					newItem.Defensives[p.IncreasedEvasion] = 15;
				}
				if (newItem.itemName == "Ice Dragon's Defense"){
					newItem.Defensives [p.ColdRes] = 25;
					newItem.Utils [p.AllAttr] = 5;
					newItem.Defensives[p.IncreasedDefense] = 35;
				}
				if (newItem.itemName == "The Wall"){
					newItem.Defensives [p.MaxHealth] = Random.Range (30, 40);
					newItem.Defensives [p.AllRes] = 15;
				}
				if (newItem.itemName == "Thunderstrike"){
					newItem.Defensives [p.LightRes] = 20;
					newItem.Offensives [p.IncreasedAttackSpeed] = (float)Random.Range (10, 15);
					newItem.Utils [p.AllAttr] = 5;
				}
				if (newItem.itemName == "Conqueror's Ambition"){
					newItem.Defensives [p.MaxHealth] = Random.Range (20, 30);
					newItem.Utils [p.AllAttr] = 15;
				}
				if (newItem.itemName == "Vicious Hunter"){
					newItem.Defensives [p.LifePerHit] = (float)System.Math.Round (Random.Range (1f, 2f), 2);
					newItem.Offensives [p.IncreasedCritChance] = (float)Random.Range (10, 16);
				}
				if (newItem.itemName == "The Redeemer"){
					newItem.Offensives [p.IncreasedMgDmg] = 10;
					newItem.Atributes [p.Spirit] = 20;
					newItem.Defensives [p.AllRes] = 10;
				}
				if (newItem.itemName == "Head Crusher"){
					newItem.Offensives [p.IncreasedCritChance] = (float)Random.Range (20, 31);
					newItem.Atributes [p.IncreasedDmg] = 20;
				}
				if (newItem.itemName == "Ogre's Mace"){
					newItem.Defensives [p.PoisonRes] = 15;
					newItem.Defensives [p.Thorns] = 7;
				}
				if (newItem.itemName == "Holy Sceptre"){
					newItem.Atributes [p.Dextery] = 20;
					newItem.Atributes [p.Spirit] = 20;
					newItem.Defensives [p.AllRes] = 20;
				}
				if (newItem.itemName == "Black Skull"){
					newItem.Offensives [p.IncreasedMgDmg] = 10;
					newItem.Atributes [p.Spirit] = 15;
					newItem.Offensives [p.IncreasedCastSpeed] = (float)Random.Range (10, 21);
				}
				if (newItem.itemName == "The Raider"){
					newItem.Atributes [p.Dextery] = 30;
					newItem.Defensives [p.LifePerHit] = (float)System.Math.Round (Random.Range (1f, 2f), 2);
					newItem.Offensives [p.IncreasedAttackSpeed] = 10;
				}
			}

			for (int j=0; j<numAffixes; j++) {
				int defAtrOff = Random.Range (0, 3);
				if (defAtrOff == 0) { //defensive
					bool ok = false;
					while (ok == false && j<=(int)newItem.Defensives.Length) {
						int optionDef = Random.Range (0, p.CantDefensives);
						if (newItem.Defensives [optionDef] > 0)
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
				}
				if (defAtrOff == 1) { //attribute
					bool ok = false;
					while (ok == false && j<=(int)newItem.Atributes.Length) {
						int optionAtr = Random.Range (0, p.CantAtributes);
						if (newItem.Atributes [optionAtr] > 0)
							continue;
						ok = true;
						newItem.Atributes [optionAtr] = Random.Range (5, 11);
					}
				}
				if (defAtrOff == 2) { //offensive
					bool ok = false;
					while (ok == false && j<=(int)newItem.Offensives.Length) {
						int optionOff = Random.Range (11, p.CantOffensives); //empieza desde 11 por que las  de antes son las esatadisticas basicas
						if (newItem.Offensives [optionOff] > 0)
							continue;
						ok = true;
						if (optionOff == p.IncreasedCritChance)
							newItem.Offensives [p.IncreasedCritChance] = (float)Random.Range (3, 7); 
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
				}
			}

		}
		DontDestroyOnLoad (newWeapon);
	}


}
