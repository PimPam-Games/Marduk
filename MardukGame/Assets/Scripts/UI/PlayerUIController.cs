using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using p = PlayerStats;

public class PlayerUIController : MonoBehaviour {


	private float startingHealth;
	//private float currentHealth;
	private Text healthBarText; 
	private Slider healthSlider;
	private Image damageImage;
	public float flashSpeed = 5f;
	public Color flashColour = new Color(1f,0f,0f,0.1f);
	
	bool damaged;
	// Use this for initialization
	void Awake(){
		healthBarText = GameObject.Find("HealthBarText").GetComponent<Text>();
		damageImage = (Image)GameObject.Find ("DamageImage").GetComponent<Image>();
		healthSlider = (Slider)GameObject.Find ("HealthSlider").GetComponent<Slider>();
		//currentHealth = playerStats.defensives[MaxHealth];
		if(healthSlider != null && p.defensives != null)
			healthSlider.value = p.defensives[p.MaxHealth];
		if(healthBarText != null && p.defensives != null)
			healthBarText.text = Math.Round(p.currentHealth,1) + " / " + Math.Round(p.defensives[p.MaxHealth],1);
	}
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if (damaged)
			damageImage.color = flashColour;
		else
			damageImage.color = Color.Lerp (damageImage.color, Color.clear, flashSpeed * Time.deltaTime);
		damaged = false;
		healthBarText.text = Math.Round(p.currentHealth,1) + " / " + Math.Round(p.defensives[p.MaxHealth],1);
		healthSlider.maxValue = p.defensives [p.MaxHealth];
		healthSlider.value = p.currentHealth;
		if (p.isDead) {
			healthBarText.text = 0 + " / " + Math.Round(p.defensives[p.MaxHealth],1);
		}
	}

	public void TakeDamage(float amount){
		damaged = true;
		//currentHealth -= amount;

	}
}
