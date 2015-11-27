using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using g = GameController;
using p = PlayerStats;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine.UI;

public class TraitsPanel : MonoBehaviour{

	//private static List<GameObject> buttons;
	public Text passivePointsText;
	public Image[] traitsButtons;

	// Use this for initialization
	void Start () {
		/*buttons = new List<GameObject> ();
		foreach (Transform child in this.transform) {
			buttons.Add(child.gameObject);
		}*/
	
	}

	// Update is called once per frame
	void Update () {
		passivePointsText.text = p.passivePoints.ToString();
	}
	
	public void clickOnTrait(int tName) {
		if (!Traits.traits [tName].isActive ()) {
			if(p.passivePoints >= Traits.traits[tName].getCost()){
				traitsButtons[tName].color = new Color(1,1,1,0.35f);
				Traits.activate(tName);
			}
		} else {
			traitsButtons[tName].color = new Color(1,1,1,1);
			Traits.deactivate(tName);
		}
	}

	public void clickOnReset () {
		foreach(Image img in traitsButtons){
			img.color = new Color(1,1,1,1);
		}
		Traits.reset();
	}


}
