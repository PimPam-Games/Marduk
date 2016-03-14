using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class EnemyCombatText : MonoBehaviour {

	private  Text text;
	public Animator anim;
    public bool reverseText = false; //se usa en los corrupted que estan al reves que el resto
	public EnemyIAMovement eMovement;

	// Use this for initialization
	void Start () {
		text = GetComponentInChildren<Text>();
		text.text = "";
		text.color = new Color(1,1,1,1);
	}
	
	void Update(){
		if((!eMovement.IsFacingRight() && !reverseText) || (eMovement.IsFacingRight() && reverseText))
			this.transform.rotation = Quaternion.Euler(0,0,0);
		else
			this.transform.rotation = Quaternion.Euler(0,180,0);

	}

	public void DisableText(){
		text.text = "";
		anim.SetBool ("DamageText",false);
		text.enabled = false;
	}

	public void ShowCombatText(string txt){
		text.enabled = true;
		anim.SetBool ("DamageText",true);
		text.text = txt;
	}


}
