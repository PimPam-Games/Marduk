using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CombatText : MonoBehaviour {

	private static Text text;
	private static float timer = 0;

	// Use this for initialization
	void Start () {
		text = GetComponentInChildren<Text>();
		text.text = "";
		text.color = new Color(1,1,1,1);
	}

	void Update(){
		timer -= Time.deltaTime;
		if(timer <= 0 && text.enabled){
			text.text = "";
			text.enabled = false;
		}
		if(PlatformerCharacter2D.isFacingRight())
			this.transform.rotation = Quaternion.Euler(0,0,0);
		else
			this.transform.rotation = Quaternion.Euler(0,180,0);

	}

	public static void ShowCombatText(string txt){
		timer = 1;
		text.enabled = true;
		text.text = txt;
	}
}
