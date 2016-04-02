using UnityEngine;
using System.Collections;
using p = PlayerStats;

/*##################### CREO QUE ESTE SCRIPT NO SE USA  ##################*/
public class RangedWeapon : MonoBehaviour {

	private float attackDelay; // tiempo de espera entre cada ataque
	//private float maxAttackingTime = 0.5f; // tiempo que dura el ataque
	public bool canAttack = false;
	private float attackTimer; 
	public Animator anim = null; 
	public float rangedAnimSpeed = 0;
	// Use this for initialization
	void Start () {
	
	}
	

	// Update is called once per frame
	void Update () {
		attackDelay = 1 / (p.offensives [p.BaseAttacksPerSecond] + (p.offensives [p.BaseAttacksPerSecond] * (p.offensives [p.IncreasedAttackSpeed]/100)));
		if(attackDelay >= 0.8f)
			rangedAnimSpeed = 0;
		if(attackDelay < 0.8f && attackDelay >= 0.5f)
			rangedAnimSpeed = 1;
		if(attackDelay < 0.5f && attackDelay >= 0.3f)
			rangedAnimSpeed = 2;
		if(attackDelay < 0.3f && attackDelay >= 0.15f)
			rangedAnimSpeed = 6;
		if(attackDelay < 0.15f)
			rangedAnimSpeed = 8;
		attackTimer -= Time.fixedDeltaTime;
		if (attackTimer <= 0 && anim.GetBool ("BowAttacking") == false) { //anim.GetBool ("Attacking") == false && 
			canAttack = true;
		}
	}

	public void BowUpdateAttackTimer(){
		attackTimer = attackDelay;
		canAttack = false;
	}
}
