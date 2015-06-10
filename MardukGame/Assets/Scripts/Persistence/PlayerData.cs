using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PlayerData{


	public  double currentExp;
	public  int lvl;
	public  double nextLevelExp;
	public  double oldNextLevelExp;
	public float[] atributes;
	public float[] offensives;
	public float[] defensives;
	public float[] utils;

	public  SerializableItem equipedArmour;
	public  SerializableItem equipedWeapon;
	public  SerializableItem equipedHelmet;
	public  SerializableItem equipedShield;

	public  List<SerializableItem> inventory;
	public  int inventoryCantItems;

}
