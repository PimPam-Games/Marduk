﻿using UnityEngine;
using System.Collections;
using p = PlayerStats;
using ui = EnemyHealthUiController;

public class EnemyStats : MonoBehaviour {

	public int lvl;
	[SerializeField] public float currHealth;
	[SerializeField] public float maxHealth = 10;
	[SerializeField] public Tuple<float,float> damage;
	//[SerializeField] private Tuple<float,float> magicDmg;
	[SerializeField] private float armour = 4;
	[SerializeField] private float coldRes = 0;
	[SerializeField] private float fireRes = 0;
	[SerializeField] private float lightRes = 0;
	[SerializeField] private float poisonRes = 1;
	[SerializeField] public Types.Element elem ;
	public float blockChance = 0;

	public AudioSource alertSound;
	private Animator anim;
	private Rigidbody2D rb;
	public string enemyName;

	public GameObject blood;
	public bool isDead = false;

	public double exp;
	private Renderer rend;


	// Use this for initialization
	void Start () {
		damage = new Tuple<float, float> (2, 5);
		//magicDmg = new Tuple<float, float> (0,0);
		currHealth = maxHealth;
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		rend = GetComponent<Renderer> ();
		StartCoroutine (AlertSoundPlay ());

	}

	IEnumerator AlertSoundPlay(){
		while (true) {
			yield return new WaitForSeconds (0.4f);
			if(rend.isVisible){
				alertSound.Play();
				yield return new WaitForSeconds (2.4f);
			}
		}
	}

	// Update is called once per frame
	void Update () {
		
	}

	void OnTriggerEnter2D(Collider2D col){ //si le pego al jugador le resto la vida
		if(col.gameObject.tag == "Player" && p.isDead)
			Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		if (col.gameObject.tag == "Player" && !p.isDead) {
			float dmgDealt = Random.Range(damage.First,damage.Second);
				if(p.defensives[p.Thorns] > 0)
					Hit (p.defensives[p.Thorns], Types.Element.None);
				col.gameObject.GetComponent<PlayerStats>().Hit(dmgDealt, elem);

				if(col.transform.position.x < this.transform.position.x)
					col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(true);
				else
					col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(false);

		}
	}

	public void Hit(float dmg, Types.Element type){
		Instantiate (blood, new Vector3(transform.position.x,transform.position.y,-4), transform.rotation); // lo creo mas cerca de la camara para que no lo tape el background
		float[] blockProb = {1 - blockChance, blockChance};
		if (Utils.Choose (blockProb) != 0) { 
			anim.SetBool ("Blocking", true);
			Debug.Log ("El enemigo Bloqueo el ataque! ");
		} else {
			float realDmg = dmg;
			switch (type) {
			case Types.Element.None:
				realDmg -= (armour / (armour + 8 * realDmg));	
				//	Debug.Log("noneeee");
				break;
			case Types.Element.Cold:
				Debug.Log ("cold damage");
				realDmg -= Mathf.Abs ((realDmg * coldRes));
				break;
			case Types.Element.Fire:
				realDmg -= Mathf.Abs ((realDmg * fireRes));
				Debug.Log ("fire damage");
				break;
			case Types.Element.Poison:
				realDmg -= Mathf.Abs ((realDmg * poisonRes));
				Debug.Log ("poison damage");
				break;
			case Types.Element.Lightning:
				realDmg -= Mathf.Abs ((realDmg * lightRes));
				Debug.Log ("lightning damage");
				break;
			default:
				Debug.LogError ("todo maaaal");
				break;
			}
			if (realDmg < 0)
				realDmg = 0;
			currHealth -= realDmg;
			//UpdateHealthBar ();
			if (currHealth < 0) {
				isDead = true;
				rb.gravityScale = 3;
				GetComponent<ItemGenerator> ().CreateItem (transform.position, transform.rotation);
				//GameObject.Find ("GameMainController").GetComponent<GameController> ().deadEnemies.Add (this.name); //agrega ese enemigo a la lista de muertos

				anim.SetBool ("IsDead", true);
				GetComponent<BoxCollider2D> ().enabled = false;
				p.UpdateExp (exp);
				StartCoroutine (EnemyDying ());
				//Destroy (this.gameObject);
			}
			if (currHealth < 0)
				currHealth = 0;
		}
		ui.UpdateHealthBar (currHealth,maxHealth,enemyName);
	}

	IEnumerator EnemyDying () {
		SpriteRenderer sprite = GetComponent<SpriteRenderer> ();
		yield return new WaitForSeconds (0.5f);
		while (sprite.color.a > 0) {
			sprite.color = new Color (1f, 1f, 1f, sprite.color.a - 0.1f);
			yield return new WaitForSeconds (0.2f);
		}
		Destroy (this.gameObject);
	}

	/*private void UpdateHealthBar(){

		float porcentOfHp = currHealth / maxHealth;
		float hpBarLength = porcentOfHp * 100;
		healthBar.hpTexture.pixelInset = new Rect(healthBar.hpTexture.pixelInset.x,healthBar.hpTexture.pixelInset.y, hpBarLength, healthBar.hpTexture.pixelInset.height);
	}*/

}
