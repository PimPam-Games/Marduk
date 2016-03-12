using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class CombatText : MonoBehaviour {

	private static Text text;
    public Animator anim;

    // Use this for initialization
    void Start () {
		text = GetComponentInChildren<Text>();
		text.text = "";
		text.color = new Color(1,1,1,1);
	}

	void Update(){
		if(text.enabled && !anim.GetBool("DamageText")){
            anim.SetBool("DamageText", true);
        }
		if(PlatformerCharacter2D.isFacingRight())
			this.transform.rotation = Quaternion.Euler(0,0,0);
		else
			this.transform.rotation = Quaternion.Euler(0,180,0);

	}

    public void DisableText()
    {
        text.text = "";
        anim.SetBool("DamageText", false);
        text.enabled = false;
    }

    public static void ShowCombatText(string txt){
		text.enabled = true;
		text.text = txt;
	}
}
