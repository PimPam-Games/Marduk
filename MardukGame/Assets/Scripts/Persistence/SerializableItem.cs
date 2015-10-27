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
		inventoryPositionX = it.InventoryPositionX;
		inventoryPositionY = it.InventoryPositionY;
		isEquipped = it.IsEquipped;
	}

	public string itemName;
	public RarityTypes rarity;
	public ItemTypes type;
	public float[] atributes;
	public float[] offensives;
	public float[] defensives;
	public float[] utils;
	public int inventoryPositionX; //posicion del item en el inventario
	public int inventoryPositionY;
	public bool isEquipped; 

}
