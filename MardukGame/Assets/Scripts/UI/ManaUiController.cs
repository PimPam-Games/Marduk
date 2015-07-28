using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ManaUiController : MonoBehaviour {

	public GameObject manaSliderObject;
	public GameObject manaText;
	
	private static Slider manaSlider;
	private static Text manaBarText; 

	void Awake(){
		manaSlider = (Slider)manaSliderObject.GetComponent<Slider> ();
		manaBarText = (Text)manaText.GetComponent<Text> ();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public static void UpdateManaBar (double current,double max){
		manaBarText.text = Math.Round(current,1) + " / " + Math.Round(max,1);
		manaSlider.maxValue = (float)max;
		manaSlider.value = (float)current;
	}
}
