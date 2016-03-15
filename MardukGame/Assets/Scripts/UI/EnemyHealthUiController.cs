using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class EnemyHealthUiController : MonoBehaviour {
	
	public GameObject HBText;
	public GameObject Hslider;
	public GameObject lvlTextObject;
	public GameObject enemyNameObject;
    public GameObject affixObject;

	private static Text healthBarText; 
	private static Slider healthSlider;
	private static Text lvlText;
	private static Text enemName;
    private static Text affixText;

	private static float activeCount=0;
	private static float activeTime = 3f; // tiempo maximo en que la barra permanece activa
	private static Canvas canvas;


	void Awake(){
		canvas = (Canvas)GetComponent<Canvas>();
		healthBarText = (Text)HBText.GetComponent<Text> ();
		lvlText = (Text)lvlTextObject.GetComponent<Text> ();
		enemName = (Text)enemyNameObject.GetComponent<Text> ();
        affixText = (Text)affixObject.GetComponent<Text>();
        healthSlider = (Slider)Hslider.GetComponent<Slider> ();
		canvas.enabled = false;
	}

	// Update is called once per frame
	void Update () {
		activeCount -= Time.deltaTime;
		if (activeCount <= 0)
			canvas.enabled = false;
	}

	public static void UpdateHealthBar(float currHealth, float maxHealth, String enemyName, int lvl, String affix, Types.EnemyTypes type)
    {
		canvas.enabled = true;
		activeCount = activeTime;
		lvlText.text = lvl.ToString();		
		healthBarText.text = Math.Round(currHealth,1) + " / " + Math.Round(maxHealth,1);
		healthSlider.maxValue = maxHealth;
		healthSlider.value = currHealth;
        affixText.text = affix;
        switch (type)
        {
            case Types.EnemyTypes.Common:
                enemName.color = new Color(1, 1, 1);
                break;
            case Types.EnemyTypes.Champion:
                enemName.color = new Color(0, 0.3f, 1);
                break;
            case Types.EnemyTypes.MiniBoss:
                enemName.color = new Color(0.9f, 0.9f, 0.1f);
                break;
            case Types.EnemyTypes.Boss:
                enemName.color = new Color(1,0.4f,0.01f);
                break;
        }
        enemName.text = enemyName;

    }

}
