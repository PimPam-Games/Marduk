using UnityEngine;
using System.Collections;
using p = PlayerStats;

public class SpellStats : MonoBehaviour {

	public Vector2 force;
	public bool flipProjectile;
	public GameObject projectile;
	public string spellName;
	public string nameForSave; //el nombre que se guarda del skill es para poder instanciarlo despues
	public float coolDown;
	public Types.SkillsTypes type;
	public float manaCost;
	public float manaReserved;     //para auras
	public float lifeRegenPerSecond; 
	public float movementX;      //para movimiento
	public float movementY;
	public float moveTime; //tiempo en el que tiene que estar activado el skill de movimieto

	private float cdTimer; 


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		cdTimer -= Time.deltaTime;
	}

	public float CDtimer{
		get {return cdTimer;}
	}

	public void ActivateCoolDown(){
		if(cdTimer <= 0)
			cdTimer = coolDown;
	}

	public void RemoveSkill(){
		if(type == Types.SkillsTypes.Aura)
			p.defensives [p.LifePerSecond] -= lifeRegenPerSecond;
		Destroy (this.gameObject);
	}
}
