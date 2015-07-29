using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SpellsPanel : MonoBehaviour, IHasChanged {

	public static GameObject[] projectiles = new GameObject[4];
	public Transform slots;
	public PlayerProjLauncher[] projLaunchers = new PlayerProjLauncher[4];
	private GameObject player;

	// Use this for initialization
	void Start () {
		player =  GameObject.Find ("Player");
		if (player != null) {
			projLaunchers = player.gameObject.GetComponentsInChildren<PlayerProjLauncher> ();
			HasChanged ();
		}

	}

	void Update(){
		if (player == null ){
			player = GameObject.FindGameObjectWithTag("Player");
			if (player != null){
				projLaunchers = player.gameObject.GetComponentsInChildren<PlayerProjLauncher> ();
				HasChanged ();
			}
		}
	}

	public void HasChanged(){
		int i = 0;
		foreach (Transform slot in slots) {
			GameObject spell = slot.GetComponent<Slot>().spell;;
			if(spell != null){
				SpellStats stats =  spell.GetComponent<SpellStats>();
				projectiles[i] = stats.projectile;
				projLaunchers[i].projectile = projectiles[i];
				projLaunchers[i].force = stats.force;
				projLaunchers[i].flipProjectile = stats.flipProjectile;
			}
			else{
				projectiles[i] = null;
				projLaunchers[i].projectile = null;
			}
			i++;
		}
	}


}

namespace UnityEngine.EventSystems{
	public interface IHasChanged : IEventSystemHandler{
		void HasChanged();
	}
}