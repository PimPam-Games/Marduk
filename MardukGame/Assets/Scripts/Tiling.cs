using UnityEngine;
using System.Collections;

[RequireComponent (typeof(SpriteRenderer))]

public class Tiling : MonoBehaviour {

	public int offsetX = 2 ;

	//for checking if we need to instantiate stuff
	public bool hasARightBuddy = false;
	public bool hasALeftBuddy = false;

	public bool reverseScale = false; //use if the object is no tilable

	private float spriteWidth = 0f;
	private Camera cam;
	private Transform myTransform;

	void Awake(){
		cam = Camera.main;
		myTransform = transform;
	}
	// Use this for initialization
	void Start () {
		SpriteRenderer sRederer = GetComponent<SpriteRenderer>();
		spriteWidth = sRederer.sprite.bounds.size.x;
	}
	
	// Update is called once per frame
	void Update () {
		if (hasALeftBuddy == false || hasARightBuddy == false) {
			float camHorizontalExtend = cam.orthographicSize * Screen.width/Screen.height;

			//calculate the x position where the camera can see the edge of the sprite
			float edgeVisiblePositionRight = (myTransform.position.x + spriteWidth/2) - camHorizontalExtend;
			float edgeVisiblePositionLeft =  (myTransform.position.x - spriteWidth/2) + camHorizontalExtend;

			//checking if we can see the edge of the element and then calling MakeNewBuddy if we can
			if (cam.transform.position.x >= edgeVisiblePositionRight - offsetX && hasARightBuddy == false){
				MakeNewBuddy(1);
				hasARightBuddy = true;
			}
			else if(cam.transform.position.x <= edgeVisiblePositionLeft + offsetX && hasALeftBuddy == false){
				MakeNewBuddy(-1);
				hasALeftBuddy = true;
			}
		}
	}

	void MakeNewBuddy(int rightOrLeft){
		//calculating the new position for the new buddy
		Vector3 newPosition = new Vector3 (myTransform.position.x + spriteWidth * rightOrLeft,myTransform.position.y, myTransform.position.z);
		Transform newBuddy = (Transform) Instantiate (myTransform, newPosition, myTransform.rotation);

		//if not tilable let's reverse the x size of our object to get rid of ugly seams
		if (reverseScale == true) {
			newBuddy.localScale = new Vector3 (newBuddy.localScale.x *-1, newBuddy.localScale.y, newBuddy.localScale.z);
		}
		newBuddy.parent = myTransform.parent;
		if (rightOrLeft > 0) {
			newBuddy.GetComponent<Tiling> ().hasALeftBuddy = true;
		} else {
			newBuddy.GetComponent<Tiling> ().hasARightBuddy = true;
		}
	}
}
