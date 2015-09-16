using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using p = PlayerStats;

public class SpellsPanel : MonoBehaviour, IHasChanged {

	public static GameObject[] projectiles = new GameObject[4];
	public Transform slots;
	//public PlayerProjLauncher[] projLaunchers = new PlayerProjLauncher[4];
	private GameObject player;
	public SpellStats[] playerSkills;
	private int a = 0;
	// Use this for initialization
	void Start () {
		player =  GameObject.Find ("Player");
		if (player != null) {
			//projLaunchers = player.gameObject.GetComponent<PlatformerCharacter2D>().projLaunchers;
			playerSkills  = player.gameObject.GetComponent<PlatformerCharacter2D>().playerSkills;
			HasChanged ();
		}

	}

	void Update(){
		if (player == null ){
			player = GameObject.FindGameObjectWithTag("Player");
			if (player != null){
				//projLaunchers = player.gameObject.GetComponent<PlatformerCharacter2D>().projLaunchers;
				playerSkills  = player.gameObject.GetComponent<PlatformerCharacter2D>().playerSkills;
				HasChanged ();
			}
		}
		a = 0;
	}

	public void HasChanged(){ //se llama cuando hubo algun cambio en el spell panel, ej: se intridujo un nuevo skill o se cambio de lugar uno
		int i = 0;
		foreach (Transform slot in slots) {
			GameObject spell = slot.GetComponent<Slot>().spell;
			if(spell != null){
				SpellStats stats =  spell.GetComponent<SpellStats>();
				/*projectiles[i] = stats.projectile;
				projLaunchers[i].projectile = projectiles[i];
				projLaunchers[i].force = stats.force;
				projLaunchers[i].flipProjectile = stats.flipProjectile;
				//Debug.Log("castdelay " + stats.castDelay);
				projLaunchers[i].castDelay = stats.castDelay;*/
				playerSkills[i] = stats;
			}
			else{
				playerSkills[i] = null;
				/*projectiles[i] = null;
				projLaunchers[i].projectile = null;*/
			}
			i++;
		}
	}

	public bool AddSpell(string spellName){
		/*a++;
		if (a > 1)
			return false;*/
		GameObject newSpellPrefab = (GameObject)Resources.Load ("PlayerSpells/" + spellName); //creo el prefab para meterlo en el slot
		if (newSpellPrefab == null) {
			Debug.LogError("prefab del skill no encontrado");
			return false;
		}
		GameObject newSpell = (GameObject)Instantiate (newSpellPrefab, newSpellPrefab.transform.position, newSpellPrefab.transform.rotation);
		foreach(Transform slot in slots){ //busca un slot vacio en spell panel
			GameObject spell = slot.GetComponent<Slot>().spell;

			if(spell == null){

				newSpell.transform.SetParent(slot);
				newSpell.GetComponent<RectTransform>().localScale = new Vector3(1,1,1);
				SpellStats st = newSpell.GetComponent<SpellStats>();

				if(st.type == Types.SkillsTypes.Aura){
					p.defensives[p.LifePerSecond] += st.lifeRegenPerSecond;
				}
				HasChanged();
				return true;
			}
		}
		return false;

	}

}

namespace UnityEngine.EventSystems{
	public interface IHasChanged : IEventSystemHandler{
		void HasChanged();
	}
}