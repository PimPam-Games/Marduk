using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class SerializableItem{

	public SerializableItem(Item it){
		itemName = it.itemName;
		rarity = it.Rarity;
		type = it.type;
		atributes = it.Atributes;
		offensives = it.Offensives;
		defensives = it.Defensives;
		utils = it.Utils;
	}

	public string itemName;
	public RarityTypes rarity;
	public ItemTypes type;
	public float[] atributes;
	public float[] offensives;
	public float[] defensives;
	public float[] utils;


}
