using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using p = PlayerStats;

public class PlayerItems: MonoBehaviour {

	private static Transform weapon;
	private static Transform rangedWeapon;
	private static Transform quiver;
	private static Transform arrow;
	private static SpriteRenderer rangedWeaponRenderer;
	private static SpriteRenderer weaponRenderer;
	private static SpriteRenderer quiverRenderer;
	public static SpriteRenderer arrowRenderer;
	private static ReSkinAnimation reSkin;
	public static int InventoryMaxSize = 35;

	private static List<Item> inventory = new List<Item>();
	private static List<SpellStats> spellsInvetory = new List<SpellStats>(); // para guardar los skills que estan en el inventario

	private static Item equipedArmour;
	private static Item equipedWeapon;
	private static Item equipedHelmet;
	private static Item equipedShield;
	private static Item equipedBelt;
	private static Item equipedAmulet;
	private static Item equipedRingL;
	private static Item equipedRingR;

	
	public static int inventoryCantItems = 0;
	public static int CantTeleporters = 6;
	public static bool[] playerTeleporters;

	public static List<Item> Inventory{
		get {return inventory;}
		set{inventory = value;}
	}

	public static List<SpellStats> SpellsInvetory{		
		get {return spellsInvetory;}
		set{spellsInvetory = value;}
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
					if(oldWeapon!=null) p.atributes[i] -= oldWeapon.Atributes[i] + oldWeapon.Utils[p.AllAttr];
					if(equipedWeapon!=null) p.atributes[i] += equipedWeapon.Atributes[i]+ equipedWeapon.Utils[p.AllAttr];
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
				rangedWeaponRenderer.sprite = null;
				quiverRenderer.enabled = false;
				p.offensives[p.BaseAttacksPerSecond] = 1;
				return;
			}
			else
				p.offensives[p.BaseAttacksPerSecond] = equipedWeapon.Offensives[p.BaseAttacksPerSecond];
			if(equipedWeapon.type == ItemTypes.Weapon){
				weaponRenderer.sprite = equipedWeapon.sprite;
				rangedWeaponRenderer.sprite = null;
				quiverRenderer.enabled = false;
				Destroy(weapon.GetComponent<PolygonCollider2D>());
				weapon.gameObject.AddComponent<PolygonCollider2D>();
			}
			else{
				rangedWeaponRenderer.sprite = equipedWeapon.sprite;
				quiverRenderer.enabled = true;
				weaponRenderer.sprite = null;
			}
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
					if(oldArmour!=null) p.atributes[i] -= oldArmour.Atributes[i]+ oldArmour.Utils[p.AllAttr];
					if(equipedArmour!=null) p.atributes[i] += equipedArmour.Atributes[i]+ equipedArmour.Utils[p.AllAttr];
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
			else{
				switch (equipedArmour.Name){
				case "Milanese armour":
					reSkin.ReSkinArmour("milanesearmor");
					break;
				case "Gothic armour":
					reSkin.ReSkinArmour("gothicarmour");
					break;
				case "Plated mail":
					reSkin.ReSkinArmour("platedmail");
					break;
				case "Golden champion":
					reSkin.ReSkinArmour("uniqueArmor1");
					break;
				case "The berserker":
					reSkin.ReSkinArmour("uniqueArmor2");
					break;
				case "Templar's faith":
					reSkin.ReSkinArmour("uniqueArmor3");
					break;
				default:
					reSkin.ReSkinArmour(equipedArmour.Name);
				break;
				}

			}
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
					if(oldHelmet!=null) p.atributes[i] -= oldHelmet.Atributes[i]+ oldHelmet.Utils[p.AllAttr];
					if(equipedHelmet!=null) p.atributes[i] += equipedHelmet.Atributes[i] + equipedHelmet.Utils[p.AllAttr];
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
			else{
				switch (equipedHelmet.Name){
				case "Dark will":
					reSkin.ReSkinHelmet("uniqueHelm1");
					break;
				case "The untouchable":
					reSkin.ReSkinHelmet("uniqueHelm2");
					break;
				case "Crusader's wrath":
					reSkin.ReSkinHelmet("uniqueHelm3");
					break;
				case "Golden cap":
					reSkin.ReSkinHelmet("uniqueHelm4");
					break;
				default:
					reSkin.ReSkinHelmet(equipedHelmet.Name);
					break;
				}

			}
				
			UpdateStats(oldHelmet,equipedHelmet);
		}
	}

	public static Item EquipedAmulet{
		get{return equipedAmulet;}
		set{
			Item oldAmulet = equipedAmulet;
			equipedAmulet = value;
			
			for(int i = 0; i<p.offensives.Length; i++){ //el arreglo mas largo es offensives
				if(oldAmulet!=null) p.offensives[i] -= oldAmulet.Offensives[i];
				if(equipedAmulet!=null) p.offensives[i] += equipedAmulet.Offensives[i];
				if(i<p.atributes.Length){
					if(oldAmulet!=null) p.atributes[i] -= oldAmulet.Atributes[i]+ oldAmulet.Utils[p.AllAttr];
					if(equipedAmulet!=null) p.atributes[i] += equipedAmulet.Atributes[i] + equipedAmulet.Utils[p.AllAttr];
				}
				if(i<p.CantDefensives){
					if(oldAmulet!=null) p.defensives[i] -= oldAmulet.Defensives[i];
					if(equipedAmulet!=null)p.defensives[i] += equipedAmulet.Defensives[i];
				}
				if(i< p.CantUtils){
					if(oldAmulet!=null) p.utils[i] -= oldAmulet.Utils[i];
					if(equipedAmulet!=null)p.utils[i] += equipedAmulet.Utils[i];
				}
			}
		
			UpdateStats(oldAmulet,equipedAmulet);
		}
	}

	public static Item EquipedRingR{
		get{return equipedRingR;}
		set{
			Item oldRingR = equipedRingR;
			equipedRingR = value;
			
			for(int i = 0; i<p.offensives.Length; i++){ //el arreglo mas largo es offensives
				if(oldRingR!=null) p.offensives[i] -= oldRingR.Offensives[i];
				if(equipedRingR!=null) p.offensives[i] += equipedRingR.Offensives[i];
				if(i<p.atributes.Length){
					if(oldRingR!=null) p.atributes[i] -= oldRingR.Atributes[i]+ oldRingR.Utils[p.AllAttr];
					if(equipedRingR!=null) p.atributes[i] += equipedRingR.Atributes[i]+ equipedRingR.Utils[p.AllAttr];
				}
				if(i<p.CantDefensives){
					if(oldRingR!=null) p.defensives[i] -= oldRingR.Defensives[i];
					if(equipedRingR!=null)p.defensives[i] += equipedRingR.Defensives[i];
				}
				if(i< p.CantUtils){
					if(oldRingR!=null) p.utils[i] -= oldRingR.Utils[i];
					if(equipedRingR!=null)p.utils[i] += equipedRingR.Utils[i];
				}
			}
			/*if(equipedAmulet==null)
				reSkin.ReSkinHelmet("head");
			else
				reSkin.ReSkinHelmet(equipedAmulet.Name);*/
			UpdateStats(oldRingR,equipedRingR);
		}
	}

	public static Item EquipedRingL{
		get{return equipedRingL;}
		set{
			Item oldRingL = equipedRingL;
			equipedRingL = value;
			
			for(int i = 0; i<p.offensives.Length; i++){ //el arreglo mas largo es offensives
				if(oldRingL!=null) p.offensives[i] -= oldRingL.Offensives[i];
				if(equipedRingL!=null) p.offensives[i] += equipedRingL.Offensives[i];
				if(i<p.atributes.Length){
					if(oldRingL!=null) p.atributes[i] -= oldRingL.Atributes[i]+ oldRingL.Utils[p.AllAttr];
					if(equipedRingL!=null) p.atributes[i] += equipedRingL.Atributes[i]+ equipedRingL.Utils[p.AllAttr];
				}
				if(i<p.CantDefensives){
					if(oldRingL!=null) p.defensives[i] -= oldRingL.Defensives[i];
					if(equipedRingL!=null)p.defensives[i] += equipedRingL.Defensives[i];
				}
				if(i< p.CantUtils){
					if(oldRingL!=null) p.utils[i] -= oldRingL.Utils[i];
					if(equipedRingL!=null)p.utils[i] += equipedRingL.Utils[i];
				}
			}
			/*if(equipedAmulet==null)
				reSkin.ReSkinHelmet("head");
			else
				reSkin.ReSkinHelmet(equipedAmulet.Name);*/
			UpdateStats(oldRingL,equipedRingL);
		}
	}

	public static Item EquipedBelt{
		get{return equipedBelt;}
		set{
			Item oldBelt = equipedBelt;
			equipedBelt = value;
			
			for(int i = 0; i<p.offensives.Length; i++){ //el arreglo mas largo es offensives
				if(oldBelt!=null) p.offensives[i] -= oldBelt.Offensives[i];
				if(equipedBelt!=null) p.offensives[i] += equipedBelt.Offensives[i];
				if(i<p.atributes.Length){
					if(oldBelt!=null) p.atributes[i] -= oldBelt.Atributes[i]+ oldBelt.Utils[p.AllAttr];
					if(equipedBelt!=null) p.atributes[i] += equipedBelt.Atributes[i]+ equipedBelt.Utils[p.AllAttr];
				}
				if(i<p.CantDefensives){
					if(oldBelt!=null) p.defensives[i] -= oldBelt.Defensives[i];
					if(equipedBelt!=null)p.defensives[i] += equipedBelt.Defensives[i];
				}
				if(i< p.CantUtils){
					if(oldBelt!=null) p.utils[i] -= oldBelt.Utils[i];
					if(equipedBelt!=null)p.utils[i] += equipedBelt.Utils[i];
				}
			}
			/*if(equipedBelt==null)
				reSkin.ReSkinHelmet("head");
			else
				reSkin.ReSkinHelmet(equipedBelt.Name);*/
			UpdateStats(oldBelt,equipedBelt);
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
					if(oldShield!=null) p.atributes[i] -= oldShield.Atributes[i]+ oldShield.Utils[p.AllAttr];
					if(equipedShield!=null) p.atributes[i] += equipedShield.Atributes[i]+ equipedShield.Utils[p.AllAttr];
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
			else{
				switch (equipedShield.Name){
				case "The contender":
					reSkin.ReSkinShield("uniqueShield1");
					break;
				case "Ice dragon's defense":
					reSkin.ReSkinShield("uniqueShield2");
					break;
				case "The wall":
					reSkin.ReSkinShield("uniqueShield3");
					break;
				default:
					reSkin.ReSkinShield(equipedShield.Name);
					break;
				}
			}
			UpdateStats(oldShield,equipedShield);
		}
	}

	private static void UpdateStats(Item oldItem, Item newItem){
		if (oldItem != null) {
			p.defensives [p.MaxHealth] -= oldItem.Atributes [p.Vitality] * 3; //resta la vitalidad vieja
			p.offensives [p.MinDmg] -= oldItem.Atributes [p.Strength] * 0.25f;
			p.offensives [p.MaxDamge] -= oldItem.Atributes [p.Strength] * 0.25f;
			p.offensives [p.MaxMana] -= oldItem.Atributes [p.Spirit] * 3;
			//p.offensives[p.MgDmg] -= oldItem.Atributes[p.Spirit] * 0.25f;
			p.offensives[p.ManaPerSec] -= oldItem.Atributes[p.Spirit] * 0.1f;
			p.defensives[p.Evasiveness] -= oldItem.Atributes[p.Dextery] * 2;
			p.offensives[p.Accuracy] -= oldItem.Atributes[p.Dextery] * 2;
		}
		if (newItem != null) {
			p.defensives [p.MaxHealth] += newItem.Atributes [p.Vitality] * 3; //un putno de vitalidad son 3 de vida
			p.offensives [p.MinDmg] += newItem.Atributes [p.Strength] * 0.25f;
			p.offensives [p.MaxDamge] += newItem.Atributes [p.Strength] * 0.25f; //4 de fuerza aumenta uno de daño fisico
			p.offensives [p.MaxMana] += newItem.Atributes [p.Spirit] * 3; // uno de espiritu da 3 de mana
			//p.offensives[p.MgDmg] += newItem.Atributes[p.Spirit] * 0.25f; // 4 de espiritu da 1 de daño magico
			p.offensives[p.ManaPerSec] += newItem.Atributes[p.Spirit] * 0.1f; // cada 10 de espiritu 1 de mana regen por segundo
			p.defensives[p.Evasiveness] += newItem.Atributes[p.Dextery] * 2;
			p.offensives[p.Accuracy] += newItem.Atributes[p.Dextery] * 2;
			//utils [MovementSpeed] = InitMoveSpeed + (InitMoveSpeed * porcentaje / 100 )
		}
	}


	// Use this for initialization
	void Awake () {
		reSkin = transform.Find ("Graphics").GetComponent<ReSkinAnimation>();
		playerTeleporters = new bool[CantTeleporters];
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
				Debug.LogError ("Weapon does not found in Player Graphics!");
		}
		if (rangedWeaponRenderer == null) {
			rangedWeapon = transform.Find ("Graphics/body/back_arm/back_forearm/Bow");
			if (rangedWeapon != null) {
				rangedWeaponRenderer = rangedWeapon.GetComponent<SpriteRenderer> ();
			} else
				Debug.LogError ("RangedWeapon does not found in Player Graphics!");
		}
		if (quiverRenderer == null) {
			quiver = transform.Find ("Graphics/body/Quiver");
			if (quiver != null) {
				quiverRenderer = quiver.GetComponent<SpriteRenderer> ();
			} else
				Debug.LogError ("quiver does not found in Player Graphics!");
		}
		if (arrowRenderer == null) {
			arrow = transform.Find ("Graphics/body/front_arm/front_forearm/Arrow");
			if (arrow != null) {
				arrowRenderer = arrow.GetComponent<SpriteRenderer> ();
			} else
				Debug.LogError ("arrow does not found in Player Graphics!");
		}
	}
}
