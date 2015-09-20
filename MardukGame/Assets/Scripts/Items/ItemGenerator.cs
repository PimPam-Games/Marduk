using UnityEngine;
using System.Collections;
using p = PlayerStats;

public class ItemGenerator :MonoBehaviour{



	private Object[] weaponList;


	void Awake(){
		weaponList = Resources.LoadAll("Weapons", typeof(Object));

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
		int i = Random.Range (0, weaponList.Length);
		GameObject newWeapon = (GameObject)Instantiate (weaponList [i],position,rotation);
		//newWeapon.GetComponent<Rigidbody2D> ().AddForce (new Vector2(0,250));
		Item newItem = newWeapon.GetComponent<Item> ();
		float[] rarityProb = {0.25f,0.25f,0.25f,0.25f}; // 60% normal, %30 magico, %9 raro , %1 unico hay que ver que onda aca
		int newRarity = Utils.Choose(rarityProb); 
		newItem.Rarity = (RarityTypes) newRarity; // 0 = normal, 1 = magico , 2 = raro , 3 = unico
		if (newItem.auraRend != null) {
			newItem.auraRend.sprite = GameController.auraRenders[newRarity]; //el color del aura del item, dependiendo si es magico, normal , etc
		}
		if (newItem.Rarity != RarityTypes.Normal) {
			int numAffixes = Random.Range (0,2);
			if (newItem.Rarity == RarityTypes.Magic)
				numAffixes = 1 + numAffixes;
			if (newItem.Rarity == RarityTypes.Rare)
				numAffixes = 3 + numAffixes;
			if (newItem.Rarity == RarityTypes.Unique)
				numAffixes = 5 + numAffixes;
			for (int j=0;j<numAffixes;j++){
				int defAtrOff = Random.Range(0,3);
				if (defAtrOff == 0){ //defensive
					bool ok = false;
					while (ok == false && j<=(int)newItem.Defensives.Length){
						int optionDef = Random.Range(0,p.CantDefensives);
						if (newItem.Defensives[optionDef]>0)
							continue;
						ok = true;
						if(optionDef == p.LifePerSecond)
							newItem.Defensives[optionDef] = (float)System.Math.Round(Random.Range (0.1f, 1f),2);
						if(optionDef == p.Thorns)
							newItem.Defensives[optionDef] = (float)System.Math.Round(Random.Range (0.2f, 2f),2);
						if(optionDef >= p.ColdRes && optionDef <= p.PoisonRes)
							newItem.Defensives[optionDef] = Random.Range(5,16);
						if(optionDef == p.MaxHealth)
							newItem.Defensives[optionDef] = Random.Range(5,21);
						if(optionDef == p.LifePerHit)
							newItem.Defensives[optionDef] = (float)System.Math.Round(Random.Range (0.5f, 2f),2);
					}
				}
				if (defAtrOff == 1){ //attribute
					bool ok = false;
					while (ok == false && j<=(int)newItem.Atributes.Length){
						int optionAtr = Random.Range(0,p.CantAtributes);
						if (newItem.Atributes[optionAtr]>0)
							continue;
						ok = true;
						newItem.Atributes[optionAtr] = Random.Range (5, 10);
					}
				}
				if (defAtrOff == 2){ //offensive
					bool ok = false;
					while (ok == false && j<=(int)newItem.Offensives.Length){
						int optionOff = Random.Range(11,p.CantOffensives); //empieza desde 11 por que las  de antes son las esatadisticas basicas
						if (newItem.Offensives[optionOff]>0)
							continue;
						ok = true;
						if (optionOff == p.IncreasedCritChance)
							newItem.Offensives[p.IncreasedCritChance] = (float)Random.Range (10, 30); 
						if (optionOff == p.IncreasedAttackSpeed ) 
							newItem.Offensives[p.IncreasedAttackSpeed] = (float)Random.Range (5, 30); 
						if (optionOff == p.IncreasedMgDmg)
							newItem.Offensives[p.IncreasedMgDmg] = (float)Random.Range (10, 30);

					}		
				}
			}

		}

		DontDestroyOnLoad (newWeapon);

		//crea una nueva armadura
	}


}
