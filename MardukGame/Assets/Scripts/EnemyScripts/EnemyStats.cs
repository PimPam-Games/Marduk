using UnityEngine;
using System.Collections;
using p = PlayerStats;
using ui = EnemyHealthUiController;

public class EnemyStats : MonoBehaviour {

	public int lvl = 1; //nivel actual del bicho
	[SerializeField] public float currHealth;
	[SerializeField] public float initMaxHealth = 10;
	[SerializeField] public float initMinDamage = 2;
	[SerializeField] public float initMaxDamage = 5;

	[SerializeField] private float initArmour = 4;
	[SerializeField] private float initColdRes = 0;
	[SerializeField] private float initFireRes = 0;
	[SerializeField] private float initLightRes = 0;
	[SerializeField] private float initPoisonRes = 1;
	[SerializeField] private float initEvasion = 30; //30 
	[SerializeField] private float initAccuracy = 25; //25 por ahi deberia ser la base
	[SerializeField] public Types.Element elem ;

    private float maxHealth = 10;
	public float minDamage = 2;
	public float maxDamage = 5;
	
	private float armour = 4;
	private float coldRes = 0;
	private float fireRes = 0;
	private float lightRes = 0;
	private float poisonRes = 1;
	private float evasion = 30; //30 
	private float accuracy = 25; //25 por ahi deberia ser la base

	[SerializeField] public float minDmgPerLvl = 0;
	[SerializeField] public float maxDmgPerLvl = 0;
	[SerializeField] public float healthPerLvl = 10;
	[SerializeField] private float armourPerLvl = 0;
	[SerializeField] private float coldResPerLvl = 0;
	[SerializeField] private float fireResPerLvl = 0;
	[SerializeField] private float lightResPerLvl = 0;
	[SerializeField] private float poisonResPerLvl = 0;
	[SerializeField] private float evasionPerLvl = 0;
	[SerializeField] private float accuracyPerLvl = 0;

	public float blockChance = 0;

	public AudioSource alertSound;
	private Animator anim;
	private Rigidbody2D rb;
	public string enemyName;
	public GameObject blood;
	public bool isDead = false;
	private LevelSettings zoneSettings;
	public double exp; //experiencia que da el bicho cuando lo matan
	private bool playAlertSound;
	private Renderer rend;

	public float Accuracy{
		get {return accuracy;}
	}

	// Use this for initialization
	void Start () {
		zoneSettings = GameObject.Find ("LevelController").GetComponent<LevelSettings>();
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		rend = GetComponent<Renderer> ();

		CalculateStats ();
	}

	void OnEnable(){
		StartCoroutine (AlertSoundPlay ());
	}

	void OnDisable(){

	}

	private void CalculateStats(){
		minDamage = initMinDamage + (lvl-1) * minDmgPerLvl;
		maxDamage = initMaxDamage + (lvl-1) * maxDmgPerLvl;
		armour = initArmour + (lvl-1) * armourPerLvl;
		coldRes = initColdRes + (lvl-1) * coldResPerLvl;
		fireRes = initFireRes + (lvl-1) * fireResPerLvl;
		lightRes = initLightRes + (lvl-1) * lightResPerLvl;
		poisonRes = initPoisonRes + (lvl-1) * poisonResPerLvl;
		evasion = initEvasion + (lvl-1) * evasionPerLvl;
		maxHealth = initMaxHealth + (lvl-1) * healthPerLvl;
		accuracy = initAccuracy + (lvl-1) * accuracyPerLvl;
		currHealth = maxHealth;
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
		if (zoneSettings.enemiesLvl != lvl) {
			lvl = zoneSettings.enemiesLvl;
			CalculateStats();
		}
	}

	void OnTriggerEnter2D(Collider2D col){ //si le pego al jugador le resto la vida
		if(col.gameObject.tag == "Player" && p.isDead)
			Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		if (col.gameObject.tag == "Player" && !p.isDead) {
			float dmgDealt = Random.Range(minDamage,maxDamage);
				if(p.defensives[p.Thorns] > 0)
					Hit (p.defensives[p.Thorns], Types.Element.None);
				col.gameObject.GetComponent<PlayerStats>().Hit(dmgDealt, elem,Accuracy);

				if(col.transform.position.x < this.transform.position.x)
					col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(true);
				else
					col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(false);

		}
	}

	public bool Hit(float dmg, Types.Element type){

		float chanceToEvade = (float)System.Math.Round((float)(1 - p.offensives [p.Accuracy] / (p.offensives [p.Accuracy] + System.Math.Pow((double)(evasion / 4),0.8))),2 );
		float[] cteProbs = {1 - chanceToEvade, chanceToEvade};
		if (Utils.Choose (cteProbs) != 0) {
			//anim.SetBool ("Blocking", true);
			Debug.Log ("El enemigo Evadio el ataque! ");
			ui.UpdateHealthBar (currHealth,maxHealth,enemyName,lvl);
			return false;
		}

		float[] blockProb = {1 - blockChance, blockChance};
		if (Utils.Choose (blockProb) != 0) { 
			anim.SetBool ("Blocking", true);
			Debug.Log ("El enemigo Bloqueo el ataque! ");
		} else {
			Instantiate (blood, new Vector3(transform.position.x,transform.position.y,-4), transform.rotation); // lo creo mas cerca de la camara para que no lo tape el background
			float realDmg = dmg;
			switch (type) {
			case Types.Element.None:
				realDmg -= (armour / (armour + 8 * realDmg));	
				break;
			case Types.Element.Cold:
				//Debug.Log ("cold damage");
				realDmg -= Mathf.Abs ((realDmg * (coldRes/100)));
				break;
			case Types.Element.Fire:
				realDmg -= Mathf.Abs ((realDmg * (fireRes/100)));
				//Debug.Log ("fire damage");
				break;
			case Types.Element.Poison:
				realDmg -= Mathf.Abs ((realDmg * (poisonRes/100)));
				//Debug.Log ("poison damage");
				break;
			case Types.Element.Lightning:
				realDmg -= Mathf.Abs ((realDmg * (lightRes/100)));
				//Debug.Log ("lightning damage" + realDmg);
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

		ui.UpdateHealthBar (currHealth,maxHealth,enemyName,lvl);
		return true;
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
}
