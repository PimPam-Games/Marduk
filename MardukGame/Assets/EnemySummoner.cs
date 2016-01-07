using UnityEngine;
using System.Collections;
using g=GameController;
public class EnemySummoner : MonoBehaviour {

	public float summonDelay;
	public float maxDistance;
	private float summonTimer;
	private GameObject target;
	[SerializeField] Animator anim;
	private EnemyStats stats;
	public GameObject enemyToInvoke;
	public Transform summonPos;
	public bool isEnemy = true; //falso si es un objeto de la scena pero no es un enemigo
	// Use this for initialization
	void Start () {
		stats = GetComponent<EnemyStats> ();
		target = GameObject.FindGameObjectWithTag ("Player");	
	}
	
	// Update is called once per frame
	void Update () {
		if(isEnemy)
			LivingEnemy();
		else
			SceneObject();
	}

	public void LivingEnemy(){
		if (stats.isDead || enemyToInvoke == null || summonPos == null)
			return;
		float distance = Vector3.Distance (target.transform.position, transform.position);
		summonTimer -= Time.deltaTime;
		if(summonTimer <= 0 && !anim.GetBool("Summoning") && !anim.GetBool("Attacking") && distance <= maxDistance){
			anim.SetBool("Summoning", true);
		}
	}

	public void SceneObject(){
		if (enemyToInvoke == null || summonPos == null)
			return;
		if(target.transform.position.y > this.transform.position.y) //si el jugador esta mas alto que el generador, no hace nada
			return;
		float distance = Vector3.Distance (target.transform.position, transform.position);
		summonTimer -= Time.deltaTime;
		if(summonTimer <= 0 && distance <= maxDistance){
			SummonEnemy();
		}
	}

	public void SummonEnemy(){
		GameObject newEnemy = (GameObject)Instantiate (enemyToInvoke,summonPos.position,summonPos.rotation);
		DontDestroyOnLoad(newEnemy);
		g.enemiesPerLevel[g.currLevelName].Add(newEnemy);
		summonTimer = summonDelay;
	}

	public void StopSummonAnim(){
		anim.SetBool("Summoning", false);
	}
}
