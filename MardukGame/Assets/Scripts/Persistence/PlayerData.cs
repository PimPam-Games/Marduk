﻿using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class PlayerData{

	public  int atributesPoints;
	public  int strAddedPoints;
	public  int vitAddedPoints;
	public  int spiAddedPoints;
	public  int dexAddedPoints;

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
	public  SerializableItem equipedBelt;
	public  SerializableItem equipedAmulet;
	public  SerializableItem equipedRingL;
	public  SerializableItem equipedRingR;

	public  List<SerializableItem> inventory;
	public  int inventoryCantItems;

}
