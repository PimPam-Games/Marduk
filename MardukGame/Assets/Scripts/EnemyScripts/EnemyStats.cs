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
	[SerializeField] private float initCritChance = 0.05f; //5% prob de critico
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
	public float critChance = 25; 
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
	public AudioSource deathSound;
	private bool alertSoundPlayed; //si ya se ejecuto el sonido del bicho
	private Animator anim;
	private Rigidbody2D rb;
	public string enemyName;
	public GameObject blood;
	public bool isDead = false;
	private LevelSettings zoneSettings;
	public double exp; //experiencia que da el bicho cuando lo matan
	private bool playAlertSound;
	public Renderer rend;
	public SpriteRenderer spriteRend;
	private float initAnimSpeed;
	EnemyIAMovement enemyMove;
	EnemyRangedAttack rangedAttack;				
	/* status ailments variables */
	private bool chill = false;
	private float chillTimer = 1f;
	private float chillCount = 0;

	private float shockTimer = 2f;
	private float shockCount = 0; 
	private bool shock = false; 

	private bool poisoned = false;
	private float poisonedCount = 0;
	private float poisonedDmg = 0;
	private float poisonedTime = 2f; //2 segundos

	private bool ignited = false;
	private float ignitedCount = 0;
	private float ignitedTime = 4f; //4 segundos
	private float ignitedDmg = 0;
	private bool itemCreated = false;

	private float DotSkillTimer = 0;
	private int t = 0; //para soluciona bug de thorns que se llama dos veces

	public bool isBoss = false;
	public float Accuracy{
		get {return accuracy;}
	}

	// Use this for initialization
	void Start () {
		if(!isBoss)
			zoneSettings = GameObject.Find ("LevelController").GetComponent<LevelSettings>();
		anim = GetComponent<Animator> ();
		rb = GetComponent<Rigidbody2D> ();
		if(!isBoss){
			rend = GetComponent<Renderer> ();
			spriteRend = GetComponent<SpriteRenderer> ();
		}
		enemyMove = GetComponent<EnemyIAMovement>();
		rangedAttack = GetComponent<EnemyRangedAttack>();
		initAnimSpeed = anim.speed;
		CalculateStats ();
	}

	void OnEnable(){
		//StartCoroutine (AlertSoundPlay ());
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
		critChance = initCritChance;
		currHealth = maxHealth;
		if(rangedAttack != null){
			foreach(GameObject plauncher in rangedAttack.pLaunchers){
				plauncher.GetComponent<ProjectileLauncher>().SetDamage(minDamage,maxDamage);
			}
		}
	}

	/*IEnumerator AlertSoundPlay(){
		while (true) {
			yield return new WaitForSeconds (0.4f);
			if(rend.isVisible){
				alertSound.Play();
				yield return new WaitForSeconds (2.4f);
			}
		}
	}*/

	// Update is called once per frame
	void Update () {
		t = 0;
		if(!isBoss){
			if (zoneSettings.enemiesLvl != lvl) {
				lvl = zoneSettings.enemiesLvl;
				CalculateStats();
			}
		}
		chillUpdate ();
		shockUpdate ();
		if (rend.isVisible && !alertSoundPlayed) {
			alertSound.Play ();
			alertSoundPlayed = true;
		}
		if (!rend.isVisible)
			alertSoundPlayed = false;
		
	}

	private void chillUpdate(){
		//Debug.Log(spriteRend.color);
		chillCount -= Time.deltaTime;
		if (chillCount <= 0 && chill) {
			chill = false;
			anim.speed = initAnimSpeed;
			enemyMove.Walk();
			enemyMove.currentSpeed = enemyMove.initMaxSpeed;
			enemyMove.maxSpeed = enemyMove.initMaxSpeed;
			//for(int i=0 ; i<renders.Length-1;i++){
			spriteRend.color = new Color (1f, 1f, 1f, 1f);
		//	}
		}
	}

	private void shockUpdate(){
		shockCount -= Time.deltaTime;
		if (shockCount <= 0 && shock) {
			shock = false;
			spriteRend.color = new Color (1f, 1f, 1f, 1f);
		}
	}

	IEnumerator PoisonedUpdate () {
		spriteRend.color = new Color (0f, 1f, 0f, 1f);
		while (poisonedCount >= 0 && !isDead) {
			yield return new WaitForSeconds (0.2f);
			poisonedCount -= 0.2f;
			currHealth -= poisonedDmg;
			if (currHealth <= 0){
				currHealth = 0;
				isDead = true;	
				StartCoroutine (EnemyDying ());
			}
			ui.UpdateHealthBar (currHealth,maxHealth,enemyName,lvl);
		}
		poisoned = false;
		spriteRend.color = new Color (1f, 1f, 1f, 1f);
	}

	IEnumerator IgnitedUpdate () {
		spriteRend.color = new Color (1f, 0.1f, 0f, 1f);
		while (ignitedCount >= 0 && !isDead) {
			yield return new WaitForSeconds (0.2f);
			ignitedCount -= 0.2f;
			currHealth -= ignitedDmg;
			if (currHealth <= 0){
				currHealth = 0;
				if(!isDead){
					isDead = true;	
					StartCoroutine (EnemyDying ());	
				}
			}
			ui.UpdateHealthBar (currHealth,maxHealth,enemyName,lvl);
		}
		ignited = false;
		spriteRend.color = new Color (1f, 1f, 1f, 1f);
	}

	void OnTriggerEnter2D(Collider2D col){ //si le pego al jugador le resto la vida
		t++;
		if(col.gameObject.tag == "Player" && p.isDead)
			Physics2D.IgnoreCollision(col.gameObject.GetComponent<Collider2D>(), GetComponent<Collider2D>());
		if (col.gameObject.tag == "Player" && !p.isDead) {
			bool isCrit = false;
			float dmgDealt = Random.Range(minDamage,maxDamage);
			float[] critDmgProb = {1 - critChance, critChance };
			if(Utils.Choose(critDmgProb) != 0){
				isCrit = true;
				dmgDealt *= 2; //si es critico lo multiplico por 2 al daño del enemigo
			}
			if(p.defensives[p.Thorns] > 0){			
				if(t == 1){
					Hit (p.defensives[p.Thorns], Types.Element.None, false);
				}
			}
			col.gameObject.GetComponent<PlayerStats>().Hit(dmgDealt, elem,Accuracy, isCrit);
			//begin traits
			if (p.isBlocking){
				if (Traits.traits[Traits.BLOCKDMG].isActive())
					Hit (p.defensives[p.Thorns]/2, Types.Element.None, false);
				p.isBlocking = false;
			}
			//end traits
			if(col.transform.position.x < this.transform.position.x)
				col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(true);
			else
				col.gameObject.GetComponent<PlatformerCharacter2D>().knockBackPlayer(false);
			PlatformerCharacter2D.skillBtnPressed = -1; //hace que el jugador deje de castear el skill
		}
	}

	void OnTriggerStay2D(Collider2D col){

		if( col.gameObject.tag == "DotSkill"){
			PlayerProjStats proj =  col.GetComponent<PlayerProjStats>();
			if(proj == null){
				return;
			}
			if(proj.elem == Types.Element.Fire){
				
				ignitedDmg = Random.Range(proj.minDmg,proj.maxDmg); 
				Debug.Log("daño quedamado " + ignitedDmg);
				ignitedCount = 0.41f;
				if(!ignited){
					StartCoroutine(IgnitedUpdate());
					ignited = true;
				}
				//DotSkillTimer = 0.42f;
				
			}
		}
	}

	public bool Hit(float dmg, Types.Element type, bool isCritical){
		if (dmg == 0 || isDead) {
			return false;
		}
		//Debug.Log ("damage: " + dmg + " Type: " + type);
		float playerAccuracy = p.offensives [p.Accuracy] + p.offensives [p.Accuracy] * p.offensives [p.IncreasedAccuracy]/100;
		float chanceToEvade = (float)System.Math.Round((float)(1 - playerAccuracy / (playerAccuracy + System.Math.Pow((double)(evasion / 4),0.8))),2 );
		//Begin Traits
		if (Traits.traits[Traits.ACCURACY].isActive ()) {
			chanceToEvade = 0;
			isCritical = false;
		}
		//End Traits
		float[] cteProbs = {1 - chanceToEvade, chanceToEvade};
		if (Utils.Choose (cteProbs) != 0 && Types.Element.None == type) { //solamente se pueden evadir ataques fisicos
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
			if(shock){
				realDmg *= 1.5f; //cuando esta shokeado aumenta el daño recibido de cualquier tipo
			}
			//Begin Traits
			if (Traits.traits[Traits.MDAMAGE].isActive()){
				if (type!=Types.Element.None)
					realDmg *= 1.5f;
			}
			if (Traits.traits[Traits.SEFFECT].isActive()){
				float[] statusProb = {0.8f, 0.2f};
				if (Utils.Choose (statusProb) != 0) {
					float[] effectProb = {0.25f, 0.25f, 0.25f, 0.25f};
					//int i = Random.Range (0, 3);
					int i = Utils.Choose (effectProb);
					switch (i) {
					case 0:
						//Debug.Log ("cold damage");
						Debug.Log("cold Damage");
						realDmg -= Mathf.Abs ((realDmg * (coldRes/100)));
						chillCount = chillTimer;
						if(!chill){
							if(enemyMove != null){
								if(isCritical){
									enemyMove.StopWalk();
								}else{
									enemyMove.currentSpeed = enemyMove.maxSpeed / 2; //algunos enemigos usan current speed y otros maxSpeed
									enemyMove.maxSpeed = enemyMove.maxSpeed / 2;     //asi que actualizo las dos
								}
							}
							if(isCritical)
								anim.speed = 0; // se congela si es critico
							else{
								anim.speed -= 0.5f;
								//Debug.Log(anim.speed);
							}
						}
						chill = true;
						//for(int i=0 ; i<renders.Length-1;i++){
						spriteRend.color = new Color (0f, 1f, 1f, 1f);
						break;
					case 1:
						realDmg -= Mathf.Abs ((realDmg * (fireRes/100)));
						if(isCritical){
							ignitedDmg = (0.2f * realDmg)/5; // 20% del daño infligido en 1 seg
							ignitedCount = ignitedTime;
							if(!ignited)
								StartCoroutine(IgnitedUpdate());
							ignited = true;
						}
						break;
					case 2:
						realDmg -= Mathf.Abs ((realDmg * (poisonRes/100)));
						poisonedDmg = (0.10f * realDmg)/5; // 10% del daño infligido en 1 seg
						poisonedCount = poisonedTime;
						if(!poisoned)
							StartCoroutine(PoisonedUpdate());
						poisoned = true;
						break;
					case 3:
						realDmg -= Mathf.Abs ((realDmg * (lightRes/100)));
						if(isCritical){
							spriteRend.color = new Color (0.75f, 0.6f, 1f, 1f);
							shockCount = shockTimer;
							shock = true;
						}
						break;
					default:
						Debug.LogError ("todo maaaal");
						break;
					}
				}
			}
			//End Traits

			switch (type) {
			case Types.Element.None:
				realDmg -= (armour / (armour + 8 * realDmg));	
				break;
			case Types.Element.Cold:
				//Debug.Log ("cold damage");
				Debug.Log("cold Damage");
				realDmg -= Mathf.Abs ((realDmg * (coldRes/100)));
				chillCount = chillTimer;
				if(!chill){
					if(enemyMove != null){
						if(isCritical){
							enemyMove.StopWalk();
						}else{
							enemyMove.currentSpeed = enemyMove.maxSpeed / 2; //algunos enemigos usan current speed y otros maxSpeed
							enemyMove.maxSpeed = enemyMove.maxSpeed / 2;     //asi que actualizo las dos
						}
					}
					if(isCritical)
						anim.speed = 0; // se congela si es critico
					else{
						anim.speed -= 0.5f;
						//Debug.Log(anim.speed);
					}
				}
				chill = true;
				//for(int i=0 ; i<renders.Length-1;i++){
				spriteRend.color = new Color (0f, 1f, 1f, 1f);
				break;
			case Types.Element.Fire:
				realDmg -= Mathf.Abs ((realDmg * (fireRes/100)));
				if(isCritical){
					ignitedDmg = (0.2f * realDmg)/5; // 20% del daño infligido en 1 seg
					ignitedCount = ignitedTime;
					if(!ignited)
						StartCoroutine(IgnitedUpdate());
					ignited = true;
				}
				break;
			case Types.Element.Poison:
				realDmg -= Mathf.Abs ((realDmg * (poisonRes/100)));
				poisonedDmg = (0.10f * realDmg)/5; // 10% del daño infligido en 1 seg
				poisonedCount = poisonedTime;
				if(!poisoned)
					StartCoroutine(PoisonedUpdate());
				poisoned = true;
				break;
			case Types.Element.Lightning:
				realDmg -= Mathf.Abs ((realDmg * (lightRes/100)));
				if(isCritical){
					spriteRend.color = new Color (0.75f, 0.6f, 1f, 1f);
					shockCount = shockTimer;
					shock = true;
				}
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
		deathSound.Play ();
		rb.gravityScale = 3;
		if (!itemCreated) {
			itemCreated = true;
			float[] dropItemProb = {0.5f,0.5f}; //50% de chance de tirar un item al morir
			if(Utils.Choose (dropItemProb) == 0)
				GetComponent<ItemGenerator> ().CreateItem (transform.position, transform.rotation);
			float[] dropOrbProb = {0.65f,0.35f};
			if(Utils.Choose(dropOrbProb)==1){
				float[] typeOrbProb = {0.5f,0.3f,0.2f}; //0 chico, 1 mediano, 2 grande
				Instantiate(GameController.lifeOrbs[Utils.Choose(typeOrbProb)],this.transform.position,this.transform.rotation);
			}
		}
		//GameObject.Find ("GameMainController").GetComponent<GameController> ().deadEnemies.Add (this.name); //agrega ese enemigo a la lista de muertos
		
		anim.SetBool ("IsDead", true);
		GetComponent<BoxCollider2D> ().enabled = false;
		p.UpdateExp (exp);
		if(!isBoss){
			SpriteRenderer sprite = GetComponent<SpriteRenderer> ();
		
			yield return new WaitForSeconds (0.5f);
			while (sprite.color.a > 0) {
				sprite.color = new Color (1f, 1f, 1f, sprite.color.a - 0.1f);
				yield return new WaitForSeconds (0.2f);
			}
			Destroy (this.gameObject);
		}
		
	}

	public void SelfDestruction(){
		Destroy (this.gameObject);
	}
}
