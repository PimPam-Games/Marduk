using UnityEngine;
using System.Collections;
using p = PlayerStats;

public class SpellStats : MonoBehaviour {

	public Vector2 force;
	public bool flipProjectile;
	public GameObject projectile;
	public string spellName;
	public string nameForSave; //el nombre que se guarda del skill es para poder instanciarlo despues
	public float castPerSecond;

	public Types.SkillsTypes type;
	public float manaCost;
	public float manaReserved;     //para auras
	public float lifeRegenPerSecond; 
	public float movementX;      //para movimiento
	public float movementY;
	public float moveTime; //tiempo en el que tiene que estar activado el skill de movimieto
	public float animSpeed = 0;
	private float castDelay;
	private float cdTimer; 


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		castDelay = 1 / (castPerSecond + castPerSecond * (p.offensives[p.IncreasedCastSpeed]/100)); //1 / (p.offensives [p.BaseAttacksPerSecond] + (p.offensives [p.BaseAttacksPerSecond] * (p.offensives [p.IncreasedAttackSpeed]/100)));
		if(castDelay >= 0.8f)
			animSpeed = 0;
		if(castDelay < 0.8f && castDelay >= 0.5f)
			animSpeed = 1;
		if(castDelay < 0.5f && castDelay >= 0.3f)
			animSpeed = 2;
		if(castDelay < 0.3f && castDelay >= 0.15f)
			animSpeed = 6;
		if(castDelay < 0.15f)
			animSpeed = 8;
		cdTimer -= Time.deltaTime;
	}

	public float CDtimer{
		get {return cdTimer;}
	}

	public void ActivateCoolDown(){
		if(cdTimer <= 0)
			cdTimer = castDelay;
	}

	public void RemoveSkill(){
		if(type == Types.SkillsTypes.Aura)
			p.defensives [p.LifePerSecond] -= lifeRegenPerSecond;
		Destroy (this.gameObject);
	}
}
