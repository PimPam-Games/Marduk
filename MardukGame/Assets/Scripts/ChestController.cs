using UnityEngine;
using System.Collections;

public class ChestController : MonoBehaviour {

	private Rigidbody2D rb;
	private ItemGenerator itGen;
	private bool firstTimeOpen;
	private SpriteRenderer sprite;
	private BoxCollider2D boxCol;
	public Sprite openSprite;
	public AudioSource chestOpenSound;
	private float timer = 0;

	void Awake(){
		itGen = GetComponent<ItemGenerator> ();
		rb = GetComponent<Rigidbody2D> ();
		firstTimeOpen = true;
		sprite = GetComponent<SpriteRenderer> ();
		boxCol = GetComponent<BoxCollider2D> ();
	}

	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if(timer <= 0){
			timer = 0.2f;
			rb.velocity = new Vector2 (0, rb.velocity.y + 00000001); //truco para que el onTriggerStay se llame todo el tiempo
			rb.velocity = new Vector2 (0, rb.velocity.y - 00000001);
		}
	}

	void OnTriggerStay2D(Collider2D coll){
		if (coll.gameObject.GetComponent<PlayerStats>() != null && Input.GetButtonUp("Grab")) {
				chestOpenSound.Play();
				sprite.sprite = openSprite;
				DropItem();
				boxCol.enabled = false;
		}
	}

	public void DropItem(){
		if (firstTimeOpen) {
			firstTimeOpen = false;
			itGen.CreateItem(this.transform.position,this.transform.rotation);
		}
	}

}
