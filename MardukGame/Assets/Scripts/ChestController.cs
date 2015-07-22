using UnityEngine;
using System.Collections;

public class ChestController : MonoBehaviour {

	private Rigidbody2D rb;
	private ItemGenerator itGen;
	private bool firstTimeOpen;
	private SpriteRenderer sprite;
	public Sprite openSprite;

	void Awake(){
		itGen = GetComponent<ItemGenerator> ();
		rb = GetComponent<Rigidbody2D> ();
		firstTimeOpen = true;
		sprite = GetComponent<SpriteRenderer> ();
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		rb.velocity = new Vector2 (0, rb.velocity.y + 00000001); //truco para que el onTriggerStay se llame todo el tiempo
		rb.velocity = new Vector2 (0, rb.velocity.y - 00000001);
	}

	void OnTriggerStay2D(Collider2D coll){
		if (coll.gameObject.tag == "Player" && Input.GetButtonUp("Grab")) {
			/*Debug.Log(anim.GetBool("Open"));
			if(anim.GetBool("Open"))
				anim.SetBool("Open", false);
			else{*/
				sprite.sprite = openSprite;
				//anim.SetBool("Open", true);
				DropItem();
			//}
		}
	}

	public void DropItem(){
		if (firstTimeOpen) {
			firstTimeOpen = false;
			itGen.CreateItem(this.transform.position,this.transform.rotation);
		}
	}

}
