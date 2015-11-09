using UnityEngine;
using System.Collections;
using System;

[Serializable]
public class SerializableSpell {

	public int inventoryPositionX = -2; //posicion del item en el inventario
	public int inventoryPositionY = -2; 
	public int idSlotEquipped = -1;
	public string spellName = "";
	public int lvl = 0;
	public double currentExp = 0; 
	public double oldNextLevelExp = 0;

	public SerializableSpell(SpellStats spell){
		this.inventoryPositionX = spell.InventoryPositionX;
		this.inventoryPositionY = spell.InventoryPositionY;
		this.spellName = spell.nameForSave;
		this.lvl = spell.Lvl;
		this.currentExp = spell.CurrentExp;
		this.oldNextLevelExp = spell.OldNextLevelExp;
		this.idSlotEquipped = spell.IdSlotEquipped;
	}

}
