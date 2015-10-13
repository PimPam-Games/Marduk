using UnityEngine;
using System.Collections;
using icg = InputControllerGui;

public class Teleporter : MonoBehaviour {


	public const int TZone1 = 0 , TCastle = 1, TDungeon = 2, TSublevel = 3; 
	public int id;
	private Animator anim;

	// Use this for initialization
	void Start () {
		anim = GetComponentInChildren<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D col){
		if (col.tag == "Player" && !icg.tpOpen) {
			PlayerItems.playerTeleporters[id] = true;
			//Debug.Log("enter portal");
			icg.OpenTeleporterPanel();
			anim.SetBool("Activated",true);
		}
	}

	void OnTriggerStay2D(Collider2D col){
		if (col.tag == "Player" && !icg.tpOpen) {

			PlayerItems.playerTeleporters[id] = true;
			//Debug.Log("enter portal");
			icg.OpenTeleporterPanel();
			anim.SetBool("Activated",true);
		}
	}

	void OnTriggerExit2D(Collider2D col){
		if (col.tag == "Player" && icg.tpOpen) {
			icg.CloseTeleporterPanel();
			anim.SetBool("Activated",false);
		}
	}
}
