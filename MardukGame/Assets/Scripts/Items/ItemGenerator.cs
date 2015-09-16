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
		Object weap =  Resources.Load("Weapons/Arming Sword", typeof(UnityEngine.Object));
		GameObject newWeapon = (GameObject)Instantiate (weap,position,rotation);
		Item newItem = newWeapon.GetComponent<Item> ();
		newItem.Rarity = RarityTypes.Normal;
		newItem.type = ItemTypes.Weapon;
		newItem.Offensives [p.MinDmg] = 1;
		newItem.Offensives [p.MaxDamge] = 2;	
		newItem.Offensives [p.BaseAttacksPerSecond] = 1.15f;
		DontDestroyOnLoad (newWeapon);
	}

	public void CreateItem(Vector3 position, Quaternion rotation){
		//crea una nueva arma
		int i = Random.Range (0, weaponList.Length);
		GameObject newWeapon = (GameObject)Instantiate (weaponList [i],position,rotation);
		//newWeapon.GetComponent<Rigidbody2D> ().AddForce (new Vector2(0,250));
		Item newItem = newWeapon.GetComponent<Item> ();
		float[] rarityProb = {0.6f,0.3f,0.09f,0.01f}; // 60% normal, %30 magico, %9 raro , %1 unico hay que ver que onda aca
		int newRarity = Utils.Choose(rarityProb); 
		newItem.Rarity = (RarityTypes) newRarity; // 0 = normal, 1 = magico , 2 = raro , 3 = unico

		if (newItem.Rarity == RarityTypes.Magic || newItem.Rarity == RarityTypes.Rare) {
			int optionDef = Random.Range(0,p.CantDefensives);
			int optionAtr = Random.Range(0,p.CantAtributes);
			float[] offensivesProb = {0.7f,0.3f}; // 60% normal, %30 magico, %9 raro , %1 unico hay que ver que onda aca
			int choice = Utils.Choose(offensivesProb); //a esto hay que sacarlo despues
			if(choice == 1)//a esto hay que sacarlo despues
				newItem.Offensives[p.IncreasedAttackSpeed] = (float)Random.Range (5, 30); //a esto hay que sacarlo despues
			newItem.Atributes[optionAtr] = Random.Range (5, 10);

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
		/*if(newItem.Rarity == RarityTypes.Rare)
			newItem.Defensives [Random.Range(0,p.CantDefensives)] = Random.Range (5, 15);*/

		DontDestroyOnLoad (newWeapon);

		//crea una nueva armadura
	}


}
