using UnityEngine;
using System.Collections;
using p = PlayerStats;

public class Nergal : MonoBehaviour {

	public float meleeAttackMinDmg = 10f;
	public float meleeAttackMaxDmg = 25f;
	private Animator anim;
	public PolygonCollider2D RightArmCol;
	public SpriteRenderer RightArmRend;
	private float meleeAttackTimer = 0;
	private float meleeAttackDelay = 10;
	private float novaAttackTimer = 0;
	private float novaAttackDelay = 13;
	public ProjectileLauncher novaLauncher;
	
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		meleeAttackTimer -= Time.deltaTime;
		novaAttackTimer -= Time.deltaTime;
		if(meleeAttackTimer < 0 && !anim.GetBool("Attacking")){
			NergalMeleeAttack();
		}
		if(novaAttackTimer < 0){
			LaunchNova();
		}
	}

	void OnTriggerEnter2D(Collider2D col){ //si le pego al jugador le resto la vida
		if(col.gameObject.tag == "Player" && p.isDead)
			Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		if (col.gameObject.tag == "Player" && !p.isDead) {
			//bool isCrit = false;
			float dmgDealt = Random.Range(meleeAttackMinDmg,meleeAttackMaxDmg);
			//float[] critDmgProb = {1 - critChance, critChance };
			//if(Utils.Choose(critDmgProb) != 0){
			//	isCrit = true;
			//	dmgDealt *= 2; //si es critico lo multiplico por 2 al daño del enemigo
			//}
			col.gameObject.GetComponent<PlayerStats>().Hit(dmgDealt, Types.Element.None,-1, false);			
			if(col.transform.position.x < this.transform.position.x)
				col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(true);
			else
				col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(false);
		}
	}

	public void LaunchNova(){
		novaAttackTimer = novaAttackDelay;
		novaLauncher.LaunchProjectile(null);
	}

	public void NergalMeleeAttack(){
		meleeAttackTimer = meleeAttackDelay;
		anim.SetBool("MeleeAttacking",true);
		RightArmRend.color = new Color (1f, 0f, 0f, 1f);
	}

	public void NergalStopAttack(){
		anim.SetBool("MeleeAttacking",false);
		RightArmRend.color = new Color (1f, 1f, 1f, 1f);
	}

	public void DisableRightArmCol(){
		RightArmCol.enabled = false;

	}

	public void EnableRightArmCol(){
		RightArmCol.enabled = true;
	}
}
