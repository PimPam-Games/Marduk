using UnityEngine;
using System.Collections;

public class SpellStats : MonoBehaviour {

	public Vector2 force;
	public bool flipProjectile;
	public GameObject projectile;
	public string spellName;
	public float castDelay;
	public skillsTypes type;
	public float manaReserved;
	public float lifeRegenPerSecond;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public enum skillsTypes{
		Skill,
		Aura,
		Curse
	}
}
