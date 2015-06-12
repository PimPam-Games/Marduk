using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class EnemyHealthUiController : MonoBehaviour {
	
	public GameObject HBText;
	public GameObject Hslider;
	public GameObject lvlTextObject;
	public GameObject enemyNameObject;

	private static Text healthBarText; 
	private static Slider healthSlider;
	private static Text lvlText;
	private static Text enemName;

	private static float activeCount=0;
	private static float activeTime = 3f; // tiempo maximo en que la barra permanece activa
	private static Canvas canvas;


	void Awake(){
		canvas = (Canvas)GetComponent<Canvas>();
		healthBarText = (Text)HBText.GetComponent<Text> ();
		lvlText = (Text)lvlTextObject.GetComponent<Text> ();
		enemName = (Text)enemyNameObject.GetComponent<Text> ();
		healthSlider = (Slider)Hslider.GetComponent<Slider> ();
		canvas.enabled = false;
	}

	// Update is called once per frame
	void Update () {
		activeCount -= Time.deltaTime;
		if (activeCount <= 0)
			canvas.enabled = false;
	}

	public static void UpdateHealthBar(float currHealth, float maxHealth, String enemyName){
		canvas.enabled = true;
		activeCount = activeTime;
		lvlText.text = "1";
		enemName.text = enemyName;
		healthBarText.text = Math.Round(currHealth,1) + " / " + Math.Round(maxHealth,1);
		healthSlider.maxValue = maxHealth;
		healthSlider.value = currHealth;

	}

}
