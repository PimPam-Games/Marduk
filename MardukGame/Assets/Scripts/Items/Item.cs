
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
	private Rigidbody2D rb;
	private int soundCount = 0; //para que el sonido no se reproduzca 2 veces

	public AudioSource itemSound;

	void Awake(){

		itemSound = GetComponent<AudioSource> ();
		atributes = new float[p.CantAtributes];
		offensives = new float[p.CantOffensives];
		defensives = new float[p.CantDefensives];
		utils = new float[p.CantUtils];
		//offensives [p.MaxDamge] = 4;
		rb = GetComponent<Rigidbody2D> ();
		//StartCoroutine (StopMove ());
	}

	void Update(){
		rb.velocity = new Vector2 (0, rb.velocity.y + 00000001); //truco para que el onTriggerStay se llame todo el tiempo
		rb.velocity = new Vector2 (0, rb.velocity.y - 00000001);
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
		}
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
			tooltip = "<color=green>" + Name + "</color> \n";
		else
			tooltip = "<color=White>" + Name + "</color> \n";
		tooltip += "Rarity: " + Rarity + "\n" + 
					"Type: " + Type + "\n";
		if (type == ItemTypes.Weapon)
			tooltip += "damage: " + offensives [p.MinDmg] + " - " + offensives [p.MaxDamge] + "\n";
		if (type == ItemTypes.Armour || type == ItemTypes.Helmet || type == ItemTypes.Shield)
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
			tooltip += "+ " + atributes[p.Dextery] + " To Dextery" + "\n";
		if(defensives[p.MaxHealth] > 0)
			tooltip += "+ " + defensives[p.MaxHealth] + " To Maximum Life" + "\n";
		if(defensives[p.ColdRes] > 0)
			tooltip += "+ " + defensives[p.ColdRes] + "% To Cold Resistance" + "\n";
		if(defensives[p.FireRes] > 0)
			tooltip += "+ " + defensives[p.FireRes] + "% To Fire Resistance" + "\n";
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
			tooltip +=  defensives[p.LifePerSecond] + " Life Regenerated Per Second" + "\n";
		if(utils[p.MovementSpeed] > 0)
			tooltip +=  utils[p.MovementSpeed] + "% To Movement Speed" + "\n";
		if(offensives[p.CritChance]>0)
			tooltip +=  offensives[p.CritChance] + "% To Critical Chance" + "\n";
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
	Helmet,
	Armour,
	Shield,
	Ring,
	Amulet,
	Belt,
	Spell
}