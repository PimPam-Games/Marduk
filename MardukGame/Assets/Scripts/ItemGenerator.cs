using UnityEngine;
using System.Collections;
using p = PlayerStats;

public class ItemGenerator :MonoBehaviour{

	private const int CantWeapons = 5;

	private Object[] weaponList;

	void Awake(){
		weaponList = Resources.LoadAll("Weapons", typeof(Object));
	}
	void Update(){

	}

	public void createInitWeapon(Vector3 position, Quaternion rotation){
		GameObject newWeapon = (GameObject)Instantiate (weaponList [0],position,rotation);
		Item newItem = newWeapon.GetComponent<Item> ();
		newItem.Rarity = RarityTypes.Normal;
		newItem.type = ItemTypes.Weapon;
		newItem.Offensives [p.MinDmg] = 1;
		newItem.Offensives [p.MaxDamge] = 2;	
		DontDestroyOnLoad (newWeapon);
	}

	public void CreateItem(Vector3 position, Quaternion rotation){
		//crea una nueva arma
		int i = Random.Range (0, weaponList.Length);
		GameObject newWeapon = (GameObject)Instantiate (weaponList [i],position,rotation);
		newWeapon.GetComponent<Rigidbody2D> ().AddForce (new Vector2(0,250));
		Item newItem = newWeapon.GetComponent<Item> ();
		float[] rarityProb = {0.6f,0.3f,0.09f,0.01f}; // 60% normal, %30 magico, %9 raro , %1 unico hay que ver que onda aca
		int newRarity = Choose(rarityProb); 
		newItem.Rarity = (RarityTypes) newRarity; // 0 = normal, 1 = magico , 2 = raro , 3 = unico
		if (newItem.type == ItemTypes.Weapon) {
			newItem.Offensives [p.MinDmg] = Random.Range (1, 3);
			newItem.Offensives [p.MaxDamge] = Random.Range (4, 7);
		}
		else
			newItem.Defensives [p.Defense] = Random.Range (1, 10);
		newItem.Defensives[p.LifePerSecond] = Random.Range (5, 10); //para prueba
		if (newItem.Rarity == RarityTypes.Magic || newItem.Rarity == RarityTypes.Rare) {
			newItem.Defensives [Random.Range(0,p.CantDefensives)] = Random.Range (2, 10);
			newItem.Atributes[p.Vitality] = Random.Range (5, 10);
			newItem.Defensives [Random.Range(0,p.CantDefensives)] = Random.Range (5, 15);
		}
		if(newItem.Rarity == RarityTypes.Rare)
			newItem.Defensives [Random.Range(0,p.CantDefensives)] = Random.Range (5, 15);

		DontDestroyOnLoad (newWeapon);

		//crea una nueva armadura
	}

	int Choose (float[] probs) {
		
		float total = 0;
		
		foreach (float elem in probs) {
			total += elem;
		}
		
		float randomPoint = Random.value * total;
		
		for (int i= 0; i < probs.Length; i++) {
			if (randomPoint < probs[i]) {
				return i;
			}
			else {
				randomPoint -= probs[i];
			}
		}
		return probs.Length - 1;
	}
}
