
using UnityEngine;
using System.Collections;
using p = PlayerStats;

public class Item : MonoBehaviour {

	public string itemName;
	private RarityTypes rarity;
	public ItemTypes type;
	public Texture2D icon;
	public Sprite sprite;
	private float[] atributes;
	private float[] offensives;
	private float[] defensives;
	private float[] utils;
	private int inventoryPosition; //posicion del item en el inventario
	private bool isEquipped; //si el item esta equipado o no
	private Rigidbody2D rb;
	private int soundCount = 0; //para que el sonido no se reproduzca 2 veces
	public float[] initMinDamage = new float[2];
	public float[] initMaxDamage =  new float[2];
	public float[] initBaseAttackPerSecond = new float[2];
	public float[] initDefense =  new float[2];
	public float[] initBlockChance =  new float[2];
	public SpriteRenderer auraRend;
	private float moveTimer = 0; //son para que objeto se mueva un poco
	private float moveSpeed = 0.1f;
	public bool itemForSlot = false; //si es true es un item para poner en un slot, si no es un item de los que se pueden agarrar
	public AudioSource itemSound;

	void Awake(){

		
		atributes = new float[p.CantAtributes];
		offensives = new float[p.CantOffensives];
		defensives = new float[p.CantDefensives];
		utils = new float[p.CantUtils];
		if(!itemForSlot){
			itemSound = GetComponent<AudioSource> ();
			rb = GetComponent<Rigidbody2D> ();
			auraRend = transform.GetChild(0).GetComponent<SpriteRenderer>();
		}
		//StartCoroutine (StopMove ());
	}

	void Start(){
		if (type == ItemTypes.Weapon || type == ItemTypes.RangedWeapon) { //el item es un arma
			Offensives [p.MinDmg] = (float)System.Math.Round(Random.Range (initMinDamage[0], initMinDamage[1]),0);
			Offensives [p.MaxDamge] = (float)System.Math.Round(Random.Range (initMaxDamage[0], initMaxDamage[1]),0);
			Offensives [p.BaseAttacksPerSecond] = (float)System.Math.Round(Random.Range (initBaseAttackPerSecond[0], initBaseAttackPerSecond[1]),2);
		} else {
			if(type == ItemTypes.Armour || type == ItemTypes.Helmet || type == ItemTypes.Belt) //el item es amour o casco
				Defensives [p.Defense] = (float)System.Math.Round(Random.Range (initDefense[0], initDefense[1]),0);
			else{ 
				if(type == ItemTypes.Shield){// el item es un escudo
					Defensives[p.Defense] =  (float)System.Math.Round(Random.Range (initDefense[0], initDefense[1]),0);
					Defensives[p.BlockChance] = (float)System.Math.Round(Random.Range(initBlockChance[0],initBlockChance[1]),0);
				}
			}	
		}
	}

	void Update(){
		if(!itemForSlot){
			if (rb.isKinematic == true) {
				if (moveTimer <= 0) {
					moveSpeed *= -1;
					moveTimer = 0.4f;
				}
				moveTimer -= Time.deltaTime;
				rb.velocity = new Vector2 (0, moveSpeed);
			}
		}
	}


	/*IEnumerator StopMove(){
		yield return new WaitForSeconds (3f);
		while (rb.velocity.x > 0.3f) {

			yield return new WaitForSeconds (0.3f);
			rb.velocity = new Vector2 (rb.velocity.x / 2, rb.velocity.y);
		}
		rb.velocity = new Vector2 (0,rb.velocity.y);
		rb.isKinematic = true;
	}*/

	void OnTriggerEnter2D(Collider2D col){
		if (col.gameObject.layer == LayerMask.NameToLayer ("Ground") && soundCount == 0) {
			soundCount++;
			itemSound.Play();
			rb.isKinematic = true;
		}
	}

	public bool IsEquipped{
		get {return isEquipped;}
		set {isEquipped = value;}
	}

	public int InventoryPosition{
		get {return inventoryPosition;}
		set {inventoryPosition = value;}
	}

	public float[] Atributes{
		get {return atributes;}
		set {atributes = value;}
	}

	public float[] Offensives{
		get {return offensives;}
		set {offensives = value;}
	}

	public float[] Defensives{
		get {return defensives;}
		set {defensives = value;}
	}

	public float[] Utils{
		get {return utils;}
		set {utils = value;}
	}

	public Item(){
		this.itemName = "unknow Name";
		this.rarity = RarityTypes.Normal;
		this.type = ItemTypes.Weapon;
	}

	public Item(string name, RarityTypes rarity, Texture2D icon,ItemTypes type ){
		this.itemName = name;
		this.rarity = rarity;
		this.icon = icon;
		this.type = type;

	}

	public string Name{
		get {return itemName;}
		set{itemName = value;}
	}

	public RarityTypes Rarity{
		get{return rarity;}
		set{rarity = value;}
	}

	public Texture2D Icon{
		get{return icon;}
		set{icon = value;}
	}

	public ItemTypes Type{
		get{return type;}
		set{type = value;}
	}

	public string ToolTip(){
		string tooltip;
		if(Rarity == RarityTypes.Magic)
			tooltip = "<color=cyan>" + Name + "</color> \n";
		else if (Rarity == RarityTypes.Rare)
			tooltip = "<color=Yellow>" + Name + "</color> \n";
		else if (Rarity == RarityTypes.Unique)
			tooltip = "<color=orange>" + Name + "</color> \n";
		else
			tooltip = "<color=White>" + Name + "</color> \n";
		tooltip += "Rarity: " + Rarity + "\n" + 
					"Type: " + Type + "\n";
		if (type == ItemTypes.Weapon || type == ItemTypes.RangedWeapon) {
			tooltip += "Damage: " + offensives [p.MinDmg] + " - " + offensives [p.MaxDamge] + "\n" +
			"Attacks per Second: " + offensives [p.BaseAttacksPerSecond] + "\n";
		}
		if (type == ItemTypes.Armour || type == ItemTypes.Helmet || type == ItemTypes.Shield || type == ItemTypes.Belt)
			tooltip += "defense: " + defensives [p.Defense] + "\n";
		if(type == ItemTypes.Shield)
			tooltip += "block chance: " + defensives[p.BlockChance] + "% \n";

		tooltip += "<color=red>----------------- </color> \n";

		if (atributes [p.Strength] > 0)
			tooltip += "+ " + atributes[p.Strength] + " To Strength" + "\n";
		if (atributes [p.Vitality] > 0)
			tooltip += "+ " + atributes[p.Vitality] + " To Vitality" + "\n";
		if (atributes [p.Spirit] > 0)
			tooltip += "+ " + atributes[p.Spirit] + " To Spirit" + "\n";
		if (atributes [p.Dextery] > 0)
			tooltip += "+ " + atributes[p.Dextery] + " To Dexterity" + "\n";
		if(defensives[p.MaxHealth] > 0)
			tooltip += "+ " + defensives[p.MaxHealth] + " To Maximum Life" + "\n";
		if(defensives[p.ColdRes] > 0)
			tooltip += "+ " + defensives[p.ColdRes] + "% To Cold Resistance" + "\n";
		if(defensives[p.FireRes] > 0)
			tooltip += "+ " + defensives[p.FireRes] + "% To Fire Resistance" + "\n";
		if(defensives[p.AllRes] != 0)
			if (defensives[p.AllRes] > 0)
				tooltip += "+ " + defensives[p.AllRes] + "% to all resistances" + "\n";
			else
				tooltip += defensives[p.AllRes] + "% to all resistances" + "\n";
		if(defensives[p.LightRes] > 0)
			tooltip += "+ " + defensives[p.LightRes] + "% To Lightning Resistance" + "\n";
		if(defensives[p.PoisonRes] > 0)
			tooltip += "+ " + defensives[p.PoisonRes] + "% To Poison Resistance" + "\n";
		if(defensives[p.Evasiveness] > 0)
			tooltip += "+ " + defensives[p.Evasiveness] + "% To Evasiveness" + "\n";
		if(defensives[p.Thorns] > 0)
			tooltip += "Reflects " + defensives[p.Thorns] + " Physical Damage \n To Attackers" + "\n";
		if(defensives[p.LifePerHit] > 0)
			tooltip +=  defensives[p.LifePerHit] + " Life Gained Per Hit" + "\n";
		if(defensives[p.LifePerSecond] > 0)
			tooltip +=  defensives[p.LifePerSecond] + "% Life Regenerated Per Second" + "\n";
		if(utils[p.MovementSpeed] > 0)
			tooltip +=  utils[p.MovementSpeed] + "% To Movement Speed" + "\n";
		if(offensives[p.CritChance]>0)
			tooltip +=  offensives[p.CritChance] + "% To Critical Chance" + "\n";
		if(offensives[p.IncreasedAttackSpeed]>0)
			tooltip +=  offensives[p.IncreasedAttackSpeed] + "% Increased Attack Speed" + "\n";
		if(offensives[p.IncreasedCritChance]>0)
			tooltip +=  offensives[p.IncreasedCritChance] + "% Increased Critical Chances" + "\n";
		if(offensives[p.IncreasedMgDmg]>0)
			tooltip +=  offensives[p.IncreasedMgDmg] + "% Increased Magic Damage" + "\n";
		if(offensives[p.IncreasedCastSpeed]>0)
			tooltip +=  offensives[p.IncreasedCastSpeed] + "% Increased Cast Speed" + "\n";
		if (utils[p.AllAttr]>0)
			tooltip += "+ " + utils[p.AllAttr] + " to all attributes";
		return tooltip;
	}

}



public enum RarityTypes{
	Normal,
	Magic,
	Rare,
	Unique
}

public enum ItemTypes{
	Weapon,
	RangedWeapon,
	Helmet,
	Armour,
	Shield,
	Ring,
	Amulet,
	Belt,
	Spell
}