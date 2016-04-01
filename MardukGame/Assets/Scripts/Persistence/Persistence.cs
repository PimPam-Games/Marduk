﻿using System.Collections.Generic;
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

	public static bool AddSavedGame(string newName ){
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
				return false;
			}
		}
		data.slots [data.cantSavedGames] = newName;
		data.cantSavedGames++;
		//Debug.Log (data.cantSavedGames);
		bf.Serialize (file,data);
		file.Close ();
        return true;
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
		data.playerTraits = (Trait[])Traits.traits.Clone();
		for (int i = 0; i<Traits.CantTraits; i++)
			data.playerTraits [i] = Traits.traits [i].Clone ();
		Traits.reset ();
		data.passivePoints = p.passivePoints;

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
		List<SerializableItem> inv = new List<SerializableItem>(); // guarda todos los items del inventario
		foreach(Item it in pItems.Inventory){
			inv.Add(new SerializableItem(it));
		}
		data.inventory = inv;

		List<SerializableSpell> skillsInv = new List<SerializableSpell>(); //guarda todos los skills del inventario
		foreach(SpellStats sp in pItems.SpellsInvetory){
			skillsInv.Add(new SerializableSpell(sp));
		}
		data.spellInv = skillsInv;

		data.inventoryCantItems = pItems.inventoryCantItems;
		data.teleporters = pItems.playerTeleporters;

        data.invTutorialShowed = TutorialController.invTutorialShowed;
        data.traitsTutorialShowed = TutorialController.traitsTutorialShowed;
        data.attributesTutorialShowed = TutorialController.attributesTutorialShowed;
        data.attackTutorialShowed = TutorialController.attackTutorialShowed;
        data.grabTutorialShowed = TutorialController.grabTutorialShowed;
        data.moveTutorialShowed = TutorialController.moveTutorialShowed;

        bf.Serialize (file,data);
		file.Close ();
		data.spellInv = null;
		ClearPlayer();
	}

	private static void ClearPlayer(){
		p.playerName = "";
		p.strAddedPoints = 0;
		p.spiAddedPoints =0;
		p.dexAddedPoints = 0;
		p.vitAddedPoints = 0;
		p.atributesPoints = 0;
		Traits.traits = null;
		p.passivePoints = 0;			
		p.lvl = 1;
		p.currentExp = 0;
		p.oldNextLevelExp = 0;
		p.nextLevelExp = 0;
		pItems.EquipedArmour = null;
		pItems.EquipedHelmet = null;
		pItems.EquipedShield = null;
		pItems.EquipedWeapon = null;
		pItems.EquipedBelt = null;
		pItems.EquipedAmulet = null;
		pItems.EquipedRingR = null;
		pItems.EquipedRingL = null;
		List<Item> inv = new List<Item>();
		List<SpellStats> skillsInv = new List<SpellStats>();
		pItems.Inventory = inv;
		pItems.SpellsInvetory = skillsInv;
		pItems.playerTeleporters = null;
        TutorialController.invTutorialShowed = false;
        TutorialController.traitsTutorialShowed = false;
        TutorialController.attributesTutorialShowed = false;
        TutorialController.attackTutorialShowed = false;
        TutorialController.grabTutorialShowed = false;
        TutorialController.moveTutorialShowed = false;
        for (int i = 0; i < PlatformerCharacter2D.playerSkills.Length; i++){
			PlatformerCharacter2D.playerSkills[i] = null;
			PlatformerCharacter2D.playerSupportSkills[i] = null;
		}
		GameObject spGO = GameObject.FindGameObjectWithTag("SpellsPanel");
		if(spGO != null){
			SpellsPanel sp = spGO.GetComponent<SpellsPanel>();
			if(sp != null)
				sp.HasChanged();
		}
	}

	public static void Load(string characterName){

		if(File.Exists(Application.persistentDataPath + "/"+characterName + ".dat")){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file =  File.Open(Application.persistentDataPath + "/"+characterName + ".dat", FileMode.Open);
			PlayerData data = null;
			data = (PlayerData)bf.Deserialize(file);
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
			Traits.traits = data.playerTraits;
			p.passivePoints = data.passivePoints;

			p.LoadAtributes();
			Traits.init ();

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
			
			pItems.playerTeleporters = data.teleporters;
			GameObject spGO = GameObject.FindGameObjectWithTag("SpellsPanel");
			SpellsPanel sp = spGO.GetComponent<SpellsPanel>();
			//Debug.Log(data.spellInv.ToString());
			sp.LoadSkills(data.spellInv);

			LoadCurrentPlayer.showIntro = false;
            
            TutorialController.invTutorialShowed = data.invTutorialShowed;
            TutorialController.traitsTutorialShowed = data.traitsTutorialShowed;
            TutorialController.attributesTutorialShowed = data.attributesTutorialShowed;
            TutorialController.attackTutorialShowed = data.attackTutorialShowed;
            TutorialController.grabTutorialShowed = data.grabTutorialShowed;
            TutorialController.moveTutorialShowed = data.moveTutorialShowed;
            TutorialController.EnableTutorial(true);
        }
		else{
			LoadCurrentPlayer.showIntro = true;
            TutorialController.EnableTutorial(true);
        }
	}

	public static void SavePreferences(int resolution, int quality){
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/videoPreferences.dat");
		PreferencesData data = new PreferencesData();
		data.resolutionIndex = resolution;
		data.quality = quality;
		bf.Serialize (file,data);
		file.Close ();
	}

	public static PreferencesData LoadPreferences(){
		if(File.Exists(Application.persistentDataPath + "/videoPreferences.dat")){
			BinaryFormatter bf = new BinaryFormatter();
			FileStream file =  File.Open(Application.persistentDataPath + "/videoPreferences.dat", FileMode.Open);
			PreferencesData data = null;
			data = (PreferencesData)bf.Deserialize(file);
			file.Close();
			return data;
		}
		return null;
	}

	private static Item GenerateItem(SerializableItem i){
        UnityEngine.Object obj = null;
        switch (i.type)
        {
            case ItemTypes.Helmet:
                obj = Resources.Load("ItemPrefabs/Helmets/" + i.itemName, typeof(UnityEngine.Object));
                break;
            case ItemTypes.Weapon:
                switch (i.weaponType)
                {
                    case WeaponTypes.Sword:
                        obj = Resources.Load("ItemPrefabs/Swords/" + i.itemName, typeof(UnityEngine.Object));
                        break;
                    case WeaponTypes.Axe:
                        obj = Resources.Load("ItemPrefabs/Axes/" + i.itemName, typeof(UnityEngine.Object));
                        break;
                    case WeaponTypes.Mace:
                        obj = Resources.Load("ItemPrefabs/Maces/" + i.itemName, typeof(UnityEngine.Object));
                        break;
                    case WeaponTypes.Wand:
                        obj = Resources.Load("ItemPrefabs/MagicWeapons/" + i.itemName, typeof(UnityEngine.Object));
                        break;
                }
                break;
            case ItemTypes.TwoHandedWeapon:
                obj = Resources.Load("ItemPrefabs/Polearms/" + i.itemName, typeof(UnityEngine.Object));
                break;
            case ItemTypes.Belt:
                obj = Resources.Load("ItemPrefabs/Belts/" + i.itemName, typeof(UnityEngine.Object));
                break;
            case ItemTypes.RangedWeapon:
                obj = Resources.Load("ItemPrefabs/Bows/" + i.itemName, typeof(UnityEngine.Object));
                break;
            case ItemTypes.Ring:
                obj = Resources.Load("ItemPrefabs/AmuletsAndRings/" + i.itemName, typeof(UnityEngine.Object));
                break;
            case ItemTypes.Amulet:
                obj = Resources.Load("ItemPrefabs/AmuletsAndRings/" + i.itemName, typeof(UnityEngine.Object));
                break;
            case ItemTypes.Shield:
                obj = Resources.Load("ItemPrefabs/Shields/" + i.itemName, typeof(UnityEngine.Object));
                break;
            case ItemTypes.Armour:
                obj = Resources.Load("ItemPrefabs/Armours/" + i.itemName, typeof(UnityEngine.Object));
                break;
        }
		 
		if(obj == null)
			obj  = Resources.Load("Unique/" + i.itemName, typeof(UnityEngine.Object));
		GameObject newWeapon = (GameObject)Instantiate (obj,new Vector3(-500,-500,500),new Quaternion(1,1,1,1));
		Item newItem = newWeapon.GetComponent<Item> ();
        newItem.itemRank = i.itemRank;
		newItem.Rarity = i.rarity;
		newItem.Type = i.type;
		newItem.Atributes = i.atributes;
		newItem.Defensives = i.defensives;
		newItem.Offensives = i.offensives;
		newItem.Utils = i.utils;
		newItem.InventoryPositionX = i.inventoryPositionX;
		newItem.InventoryPositionY = i.inventoryPositionY;
		newItem.IsEquipped = i.isEquipped;
		DontDestroyOnLoad (newItem);
		PlatformerCharacter2D.playerItemsGO.Add (newWeapon);
		newWeapon.SetActive (false);
		return newItem;
	}

}
