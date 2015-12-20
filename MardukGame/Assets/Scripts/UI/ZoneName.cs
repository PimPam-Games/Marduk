using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ZoneName : MonoBehaviour {

	public Image imgGo;
	public Sprite[] zoneImagesGo;
	public static Sprite[] zoneImages;
	private static Image img;
	private static float zoneImgTimer = 0;
	// Use this for initialization
	void Start () {
		img = imgGo;
		zoneImages = zoneImagesGo;
	}
	
	// Update is called once per frame
	void Update () {
		zoneImgTimer -= Time.deltaTime;
		if(zoneImgTimer <= 0 && img.enabled)
			img.enabled = false;
	}

	public static void ShowZoneName(int id){
		zoneImgTimer = 6f;
		img.sprite = zoneImages[id-1];
		img.enabled = true;
	}	
}
