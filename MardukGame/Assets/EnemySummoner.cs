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
	// Use this for initialization
	void Start () {
		stats = GetComponent<EnemyStats> ();
		target = GameObject.FindGameObjectWithTag ("Player");	
	}
	
	// Update is called once per frame
	void Update () {
		if (stats.isDead || enemyToInvoke == null || summonPos == null)
			return;
		float distance = Vector3.Distance (target.transform.position, transform.position);
		summonTimer -= Time.deltaTime;
		if(summonTimer <= 0 && !anim.GetBool("Summoning") && !anim.GetBool("Attacking") && distance <= maxDistance){
			anim.SetBool("Summoning", true);
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
