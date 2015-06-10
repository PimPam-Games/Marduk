using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using p = PlayerStats;
using pItems = PlayerItems;

public class Persistence : MonoBehaviour {

	public static void Save(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerInfo.dat");
		PlayerData data = new PlayerData ();
		data.atributes = p.atributes;
		data.defensives = p.defensives;
		data.offensives = p.offensives;
		data.utils = p.utils;
		data.lvl = p.lvl;
		data.currentExp = p.currentExp;
		data.oldNextLevelExp = p.oldNextLevelExp;
		data.nextLevelExp = p.nextLevelExp;

		if(pItems.EquipedWeapon != null)
			data.equipedWeapon = new SerializableItem(pItems.EquipedWeapon);
		if(pItems.EquipedHelmet != null)
			data.equipedHelmet = new SerializableItem(pItems.EquipedHelmet);
		if(pItems.EquipedShield != null)
			data.equipedShield = new SerializableItem(pItems.EquipedShield);
		if(pItems.EquipedArmour != null)
			data.equipedArmour = new SerializableItem(pItems.EquipedArmour);
		List<SerializableItem> inv = new List<SerializableItem>();
		foreach(Item it in pItems.Inventory){
			inv.Add(new SerializableItem(it));
		}
		data.inventory = inv;
		data.inventoryCantItems = pItems.inventoryCantItems;

		bf.Serialize (file,data);
		file.Close ();
	}

	public static void Load(){
		if(File.Exists(Application.persistentDataPath + "/playerInfo.dat")){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file =  File.Open(Application.persistentDataPath + "/playerInfo.dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close();
			/*p.atributes = data.atributes ;
			p.defensives = data.defensives;
			p.offensives = data.offensives;*/
			p.utils = data.utils;
			p.lvl = data.lvl;
			p.currentExp = data.currentExp;
			p.oldNextLevelExp = data.oldNextLevelExp;
			p.nextLevelExp = data.nextLevelExp;
			ExpUiController.UpdateExpBar(p.currentExp,p.oldNextLevelExp,p.nextLevelExp);
			if(data.equipedArmour != null)
				pItems.EquipedArmour = GenerateItem(data.equipedArmour);
			if(data.equipedHelmet != null)
				pItems.EquipedHelmet= GenerateItem(data.equipedHelmet);
			if(data.equipedShield != null)
				pItems.EquipedShield = GenerateItem (data.equipedShield);
			if(data.equipedWeapon!= null)
				pItems.EquipedWeapon = GenerateItem (data.equipedWeapon);
			List<Item> inv = new List<Item>();
			foreach(SerializableItem it in data.inventory){
				inv.Add(GenerateItem(it));
			}
			pItems.Inventory = inv;
			pItems.inventoryCantItems = data.inventoryCantItems;

		}
	}

	private static Item GenerateItem(SerializableItem i){

		UnityEngine.Object obj  = Resources.Load("Weapons/" + i.itemName, typeof(UnityEngine.Object));
		GameObject newWeapon = (GameObject)Instantiate (obj,new Vector3(-500,-500,500),new Quaternion(1,1,1,1));
		Item newItem = newWeapon.GetComponent<Item> ();

		newItem.Rarity = i.rarity;
		newItem.Type = i.type;
		newItem.Atributes = i.atributes;
		newItem.Defensives = i.defensives;
		newItem.Offensives = i.offensives;
		newItem.Utils = i.utils;
		DontDestroyOnLoad (newItem);
		newWeapon.SetActive (false);
		return newItem;
	}

}
