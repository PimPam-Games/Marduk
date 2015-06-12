using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class ExpUiController : MonoBehaviour {

	public GameObject expSliderObject;

	private static Slider expSlider;

	void Awake(){
		expSlider = (Slider)expSliderObject.GetComponent<Slider> ();
	}

	// Update is called once per frame
	void Update () {

	}

	public static void UpdateExpBar (double current, double min, double max){
		expSlider.minValue = (float)min;
		expSlider.maxValue = (float)max;
		expSlider.value = (float)current;
	}

}
