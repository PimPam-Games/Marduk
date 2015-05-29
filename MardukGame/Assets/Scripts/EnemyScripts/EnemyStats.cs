using UnityEngine;
using System.Collections;
using p = PlayerStats;

public class EnemyStats : MonoBehaviour {

	public EnemyBarController healthBar;
	public EnemyBarController healthBarbg;
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
	private Animator anim;

	public GameObject blood;
	public bool isDead = false;


	// Use this for initialization
	void Start () {
		damage = new Tuple<float, float> (2, 5);
		//magicDmg = new Tuple<float, float> (0,0);
		currHealth = maxHealth;
		anim = GetComponent<Animator> ();
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
		Instantiate (blood, transform.position, transform.rotation);
		GetComponent<EnemyMovement> ().Knock (0.3f); // tiempo que el enemigo deja de caminar cuando se lo golpea
		GetComponent<Animator> ().SetBool ("hit",true);
		float realDmg = dmg;
		switch (type){
			case Types.Element.None:
				realDmg -= (armour / (armour + 8 * realDmg));	
			//	Debug.Log("noneeee");
				break;
			case Types.Element.Cold:
				Debug.Log("cold damage");
				realDmg -= Mathf.Abs((realDmg * coldRes));
				break;
			case Types.Element.Fire:
				realDmg -= Mathf.Abs((realDmg * fireRes));
				Debug.Log("fire damage");
				break;
			case Types.Element.Poison:
				realDmg -= Mathf.Abs((realDmg * poisonRes));
				Debug.Log("poison damage");
				break;
			case Types.Element.Lightning:
				realDmg -= Mathf.Abs((realDmg * lightRes));
				Debug.Log("lightning damage");
				break;
			default:
				Debug.LogError("todo maaaal");
				break;
		}
		if (realDmg < 0)
			realDmg = 0;
		currHealth -= realDmg;
		UpdateHealthBar ();
		if (currHealth < 0) {
			isDead = true;
			GetComponent<ItemGenerator>().CreateItem(transform.position, transform.rotation);
			GameObject.Find ("GameMainController").GetComponent<GameController> ().deadEnemies.Add (this.name); //agrega ese enemigo a la lista de muertos

			anim.SetBool("IsDead", true);
			GetComponent<BoxCollider2D>().enabled = false;
			healthBar.Hide();
			healthBarbg.Hide();
			GetComponent<EnemyMovement>().maxSpeed = 0;
			GetComponent<EnemyMovement>().followPlayer = false;
			GetComponent<EnemyMovement>().flipDelay = 99999999;
			StartCoroutine(EnemyDying());
			//Destroy (this.gameObject);
		}
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

	private void UpdateHealthBar(){

		float porcentOfHp = currHealth / maxHealth;
		float hpBarLength = porcentOfHp * 100;
		healthBar.hpTexture.pixelInset = new Rect(healthBar.hpTexture.pixelInset.x,healthBar.hpTexture.pixelInset.y, hpBarLength, healthBar.hpTexture.pixelInset.height);
	}

}
