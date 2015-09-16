using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using p = PlayerStats;
using pItems = PlayerItems;

public class Persistence : MonoBehaviour {

	public const int CantSlots = 5;

	public static int CantSavedGames(){
		if (File.Exists (Application.persistentDataPath + "/savedGames.dat")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file =  File.Open(Application.persistentDataPath + "/savedGames.dat", FileMode.Open);
			SavedGamesData data = (SavedGamesData)bf.Deserialize(file);
			file.Close();
			return data.cantSavedGames;
		}
		return -1;
	}

	public static void Delete(string nameToDelete){
		SavedGamesData data;
		SavedGamesData dataAux;
		FileStream file;
		dataAux = new SavedGamesData ();
		data = new SavedGamesData ();
		data.slots = new string[CantSlots];
		if (File.Exists (Application.persistentDataPath + "/savedGames.dat")) {
			BinaryFormatter bf = new BinaryFormatter();
			file =  File.Open(Application.persistentDataPath + "/savedGames.dat", FileMode.Open);
			dataAux = (SavedGamesData)bf.Deserialize(file);
			file.Close ();
			file = File.Create (Application.persistentDataPath + "/savedGames.dat");
			data.slots = dataAux.slots;
			data.cantSavedGames = dataAux.cantSavedGames;
			int i;
			for (i =0; i<data.cantSavedGames; i++) {
				if(data.slots[i] == nameToDelete){

					Debug.Log("personaje eliminado de la lista");
					break;
				}
			}
			int j;
			for(j = i; j<data.cantSavedGames-1; j++){
				data.slots[j] = data.slots[j+1];
			}
			data.slots[data.cantSavedGames-1] = null;
			data.cantSavedGames--;
			bf.Serialize (file,data);
			file.Close ();
		}
		if (File.Exists (Application.persistentDataPath + "/" + nameToDelete + ".dat")) {
			File.Delete (Application.persistentDataPath + "/" + nameToDelete + ".dat");
			Debug.Log("Archivo del personaje eliminado");
		}
	}

	public static void AddSavedGame(string newName ){
		BinaryFormatter bf = new BinaryFormatter ();
		SavedGamesData data;
		SavedGamesData dataAux;
		FileStream file;
		data = new SavedGamesData ();
		data.slots = new string[CantSlots];
		if (!File.Exists (Application.persistentDataPath + "/savedGames.dat")) {
			file = File.Create (Application.persistentDataPath + "/savedGames.dat");
		} else {
			file =  File.Open(Application.persistentDataPath + "/savedGames.dat", FileMode.Open);
			dataAux = (SavedGamesData)bf.Deserialize(file);
			file.Close ();
			file = File.Create (Application.persistentDataPath + "/savedGames.dat");
			data.slots = dataAux.slots;
			data.cantSavedGames = dataAux.cantSavedGames;
		}
		for (int i =0; i<data.cantSavedGames; i++) {
			if(data.slots[i] == newName){
				Debug.Log("El nombre ya existe");
				bf.Serialize (file,data);
				file.Close ();
				return;
			}
		}
		data.slots [data.cantSavedGames] = newName;
		data.cantSavedGames++;
		Debug.Log (data.cantSavedGames);
		bf.Serialize (file,data);
		file.Close ();
	}

	public static string[] GetSavedGames(){
		if (File.Exists (Application.persistentDataPath + "/savedGames.dat")) {
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file =  File.Open(Application.persistentDataPath + "/savedGames.dat", FileMode.Open);
			SavedGamesData data = (SavedGamesData)bf.Deserialize(file);
			file.Close();
			return data.slots;
		}
		return null;
	}

	public static void Save(){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/"+p.playerName + ".dat");
		PlayerData data = new PlayerData ();
		data.atributes = p.atributes;
		data.defensives = p.defensives;
		data.offensives = p.offensives;
		data.utils = p.utils;
		data.lvl = p.lvl;
		data.currentExp = p.currentExp;
		data.oldNextLevelExp = p.oldNextLevelExp;
		data.nextLevelExp = p.nextLevelExp;

		data.playerName = p.playerName;
		data.strAddedPoints = p.strAddedPoints;
		data.spiAddedPoints = p.spiAddedPoints;
		data.dexAddedPoints = p.dexAddedPoints;
		data.vitAddedPoints = p.vitAddedPoints;
		data.atributesPoints = p.atributesPoints;

		if(pItems.EquipedWeapon != null)
			data.equipedWeapon = new SerializableItem(pItems.EquipedWeapon);
		if(pItems.EquipedHelmet != null)
			data.equipedHelmet = new SerializableItem(pItems.EquipedHelmet);
		if(pItems.EquipedShield != null)
			data.equipedShield = new SerializableItem(pItems.EquipedShield);
		if(pItems.EquipedArmour != null)
			data.equipedArmour = new SerializableItem(pItems.EquipedArmour);
		if(pItems.EquipedBelt != null)
			data.equipedBelt = new SerializableItem(pItems.EquipedBelt);
		if(pItems.EquipedAmulet != null)
			data.equipedAmulet = new SerializableItem(pItems.EquipedAmulet);
		if(pItems.EquipedRingL != null)
			data.equipedRingL = new SerializableItem(pItems.EquipedRingL);
		if(pItems.EquipedRingR != null)
			data.equipedRingR = new SerializableItem(pItems.EquipedRingR);
		List<SerializableItem> inv = new List<SerializableItem>();
		foreach(Item it in pItems.Inventory){
			inv.Add(new SerializableItem(it));
		}
		data.inventory = inv;
		data.inventoryCantItems = pItems.inventoryCantItems;

		GameObject player = GameObject.FindGameObjectWithTag("Player");
		string[] playerSkills = new string[4];
		if (player != null) { //para guardar los skills del jugador
			for (int i = 0; i < playerSkills.Length; i++) {
				SpellStats ps = player.gameObject.GetComponent<PlatformerCharacter2D>().playerSkills[i];

				if(ps != null){
					playerSkills[i] = ps.nameForSave;
				}else
					playerSkills[i] = null;
			}
		}
		data.skillsNames = playerSkills;
		bf.Serialize (file,data);
		file.Close ();
	}

	public static void Load(string characterName){

		if(File.Exists(Application.persistentDataPath + "/"+characterName + ".dat")){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file =  File.Open(Application.persistentDataPath + "/"+characterName + ".dat", FileMode.Open);
			PlayerData data = (PlayerData)bf.Deserialize(file);
			file.Close();
			/*p.atributes = data.atributes ;
			p.defensives = data.defensives;
			p.offensives = data.offensives;*/
			//p.utils = data.utils;
			p.playerName = data.playerName;
			p.strAddedPoints = data.strAddedPoints;
			p.spiAddedPoints = data.spiAddedPoints;
			p.dexAddedPoints = data.dexAddedPoints;
			p.vitAddedPoints = data.vitAddedPoints;
			p.atributesPoints = data.atributesPoints;

			p.LoadAtributes();

			p.lvl = data.lvl;
			p.currentExp = data.currentExp;
			p.oldNextLevelExp = data.oldNextLevelExp;
			p.nextLevelExp = data.nextLevelExp;
			ExpUiController.UpdateExpBar(p.currentExp,p.oldNextLevelExp,p.nextLevelExp);
			if(data.equipedArmour != null)
				pItems.EquipedArmour = GenerateItem(data.equipedArmour);
			else
				pItems.EquipedArmour = null;
			if(data.equipedHelmet != null)
				pItems.EquipedHelmet= GenerateItem(data.equipedHelmet);
			else
				pItems.EquipedHelmet = null;
			if(data.equipedShield != null)
				pItems.EquipedShield = GenerateItem (data.equipedShield);
			else
				pItems.EquipedShield = null;
			if(data.equipedWeapon!= null)
				pItems.EquipedWeapon = GenerateItem (data.equipedWeapon);
			else
				pItems.EquipedWeapon = null;
			if(data.equipedBelt!= null)
				pItems.EquipedBelt = GenerateItem (data.equipedBelt);
			else
				pItems.EquipedBelt = null;
			if(data.equipedAmulet!= null)
				pItems.EquipedAmulet = GenerateItem (data.equipedAmulet);
			else
				pItems.EquipedAmulet = null;
			if(data.equipedRingR!= null)
				pItems.EquipedRingR = GenerateItem (data.equipedRingR);
			else
				pItems.EquipedRingR = null;
			if(data.equipedRingL!= null)
				pItems.EquipedRingL = GenerateItem (data.equipedRingL);
			else
				pItems.EquipedRingL = null;
			List<Item> inv = new List<Item>();
			foreach(SerializableItem it in data.inventory){
				inv.Add(GenerateItem(it));
			}
			pItems.Inventory = inv;
			pItems.inventoryCantItems = data.inventoryCantItems;
			GameObject spGO = GameObject.FindGameObjectWithTag("SpellsPanel");
			SpellsPanel sp = spGO.GetComponent<SpellsPanel>();
			if(data.skillsNames == null)
				return;
			for(int i = 0; i < data.skillsNames.Length; i++){
				if(data.skillsNames[i] != null)
				{

					sp.AddSpell(data.skillsNames[i]);
				}
			}
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
		PlatformerCharacter2D.playerItemsGO.Add (newWeapon);
		newWeapon.SetActive (false);
		return newItem;
	}

}
