using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using p = PlayerStats;

public class PlayerItems: MonoBehaviour {
	
	private static Transform weapon;
    
	private static SpriteRenderer weaponRenderer;
	private static ReSkinAnimation reSkin;
	private static List<Item> inventory = new List<Item>();

	private static Item equipedArmour;
	private static Item equipedWeapon;
	private static Item equipedHelmet;
	private static Item equipedShield;
	public static int InventoryMaxSize = 28;
	public static int inventoryCantItems = 0;

	public static List<Item> Inventory{

		get {return inventory;}
		set{inventory = value;}
	}



	public static Item EquipedWeapon{
		get{return equipedWeapon;}
		set{
			Item oldWeapon = equipedWeapon;
			equipedWeapon = value;

			for(int i = 0; i<p.offensives.Length; i++){ //el arreglo mas largo es offensives
				if(oldWeapon!=null) p.offensives[i] -= oldWeapon.Offensives[i];
				if(equipedWeapon!=null) p.offensives[i] += equipedWeapon.Offensives[i];
				if(i<p.atributes.Length){
					if(oldWeapon!=null) p.atributes[i] -= oldWeapon.Atributes[i];
					if(equipedWeapon!=null) p.atributes[i] += equipedWeapon.Atributes[i];
				}
				if(i<p.CantDefensives){
					if(oldWeapon!=null) p.defensives[i] -= oldWeapon.Defensives[i];
					if(equipedWeapon!=null)p.defensives[i] += equipedWeapon.Defensives[i];
				}
				if(i< p.CantUtils){
					if(oldWeapon!=null) p.utils[i] -= oldWeapon.Utils[i];
					if(equipedWeapon!=null)p.utils[i] += equipedWeapon.Utils[i];
				}
			}
			UpdateStats(oldWeapon,equipedWeapon);
			if(equipedWeapon==null){
				weaponRenderer.sprite = null;
				return;
			}
			weaponRenderer.sprite = equipedWeapon.sprite;
			Destroy(weapon.GetComponent<PolygonCollider2D>());
			weapon.gameObject.AddComponent<PolygonCollider2D>();

		}
	}

	public static Item EquipedArmour{
		get{return equipedArmour;}
		set{
			Item oldArmour = equipedArmour;
			equipedArmour = value;
			
			for(int i = 0; i<p.offensives.Length; i++){ //el arreglo mas largo es offensives
				if(oldArmour!=null) p.offensives[i] -= oldArmour.Offensives[i];
				if(equipedArmour!=null) p.offensives[i] += equipedArmour.Offensives[i];
				if(i<p.atributes.Length){
					if(oldArmour!=null) p.atributes[i] -= oldArmour.Atributes[i];
					if(equipedArmour!=null) p.atributes[i] += equipedArmour.Atributes[i];
				}
				if(i<p.CantDefensives){
					if(oldArmour!=null) p.defensives[i] -= oldArmour.Defensives[i];
					if(equipedArmour!=null)p.defensives[i] += equipedArmour.Defensives[i];
				}
				if(i< p.CantUtils){
					if(oldArmour!=null) p.utils[i] -= oldArmour.Utils[i];
					if(equipedArmour!=null)p.utils[i] += equipedArmour.Utils[i];
				}
			}
			if(equipedArmour==null)
				reSkin.ReSkinArmour("default_armor");
			else
				reSkin.ReSkinArmour(equipedArmour.Name);
			UpdateStats(oldArmour,equipedArmour);
		}
	}

	public static Item EquipedHelmet{
		get{return equipedHelmet;}
		set{
			Item oldHelmet = equipedHelmet;
			equipedHelmet = value;
			
			for(int i = 0; i<p.offensives.Length; i++){ //el arreglo mas largo es offensives
				if(oldHelmet!=null) p.offensives[i] -= oldHelmet.Offensives[i];
				if(equipedHelmet!=null) p.offensives[i] += equipedHelmet.Offensives[i];
				if(i<p.atributes.Length){
					if(oldHelmet!=null) p.atributes[i] -= oldHelmet.Atributes[i];
					if(equipedHelmet!=null) p.atributes[i] += equipedHelmet.Atributes[i];
				}
				if(i<p.CantDefensives){
					if(oldHelmet!=null) p.defensives[i] -= oldHelmet.Defensives[i];
					if(equipedHelmet!=null)p.defensives[i] += equipedHelmet.Defensives[i];
				}
				if(i< p.CantUtils){
					if(oldHelmet!=null) p.utils[i] -= oldHelmet.Utils[i];
					if(equipedHelmet!=null)p.utils[i] += equipedHelmet.Utils[i];
				}
			}
			if(equipedHelmet==null)
				reSkin.ReSkinHelmet("head");
			else
				reSkin.ReSkinHelmet(equipedHelmet.Name);
			UpdateStats(oldHelmet,equipedHelmet);
		}
	}

	public static Item EquipedShield{
		get{return equipedShield;}
		set{
			Item oldShield = equipedShield;
			equipedShield = value;

			for(int i = 0; i<p.offensives.Length; i++){ //el arreglo mas largo es offensives
				if(oldShield!=null) p.offensives[i] -= oldShield.Offensives[i];
				if(equipedShield!=null) p.offensives[i] += equipedShield.Offensives[i];
				if(i<p.atributes.Length){
					if(oldShield!=null) p.atributes[i] -= oldShield.Atributes[i];
					if(equipedShield!=null) p.atributes[i] += equipedShield.Atributes[i];
				}
				if(i<p.CantDefensives){
					if(oldShield!=null) p.defensives[i] -= oldShield.Defensives[i];
					if(equipedShield!=null)p.defensives[i] += equipedShield.Defensives[i];
				}
				if(i< p.CantUtils){
					if(oldShield!=null) p.utils[i] -= oldShield.Utils[i];
					if(equipedShield!=null)p.utils[i] += equipedShield.Utils[i];
				}
			}
			if(equipedShield==null)
				reSkin.ReSkinShield("none");
			else
				reSkin.ReSkinShield(equipedShield.Name);
			UpdateStats(oldShield,equipedShield);
		}
	}

	private static void UpdateStats(Item oldItem, Item newItem){
		if (oldItem != null) {
			p.defensives [p.MaxHealth] -= oldItem.Atributes [p.Vitality] * 3; //resta la vitalidad vieja
			p.offensives [p.MinDmg] -= oldItem.Atributes [p.Strength] * 0.25f;
			p.offensives [p.MaxDamge] -= oldItem.Atributes [p.Strength] * 0.25f;
			p.offensives [p.MaxMana] -= oldItem.Atributes [p.Spirit] * 3;
		}
		if (newItem != null) {
			p.defensives [p.MaxHealth] += newItem.Atributes [p.Vitality] * 3; //un putno de vitalidad son 3 de vida
			p.offensives [p.MinDmg] += newItem.Atributes [p.Strength] * 0.25f;
			p.offensives [p.MaxDamge] += newItem.Atributes [p.Strength] * 0.25f; //4 de fuerza aumenta uno de daño fisico
			p.offensives [p.MaxMana] += newItem.Atributes [p.Spirit] * 3; // uno de espiritu da 3 de mana
			//utils [MovementSpeed] = InitMoveSpeed + (InitMoveSpeed * porcentaje / 100 )
		}
	}

	// Use this for initialization
	void Awake () {

	}
	
	// Update is called once per frame
	void Update () {
		if (reSkin == null) {
			reSkin = transform.Find ("Graphics").GetComponent<ReSkinAnimation>();
		}
		if (weaponRenderer == null) {
			weapon = transform.Find ("Graphics/body/back_arm/back_forearm/weapon");
			if (weapon != null) {
				weaponRenderer = weapon.GetComponent<SpriteRenderer> ();
			} else
				Debug.LogError ("Weapon does not found!");
		}
	}
}
