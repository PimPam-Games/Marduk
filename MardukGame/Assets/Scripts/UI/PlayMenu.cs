using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using g = GameController;

public class PlayMenu : MonoBehaviour {

	public Image mainMenu;
	public InputField inputField;
	public Button[] slots;

	private string newCharacterName;
	private string[] savedGames;

	public void Start(){
		UpdateSavedGames ();
	}

	private void UpdateSavedGames(){
		savedGames = Persistence.GetSavedGames ();
		if (savedGames == null)
			return;
		for (int i = 0; i < slots.Length; i++) {
			if(savedGames[i] != null){
					slots[i].gameObject.SetActive(true);
					slots[i].GetComponentInChildren<Text>().text = savedGames[i];
			}
			else{
				slots[i].gameObject.SetActive(false);
			}
		}
	}

	public void Back(){
		StartCoroutine(BackPush());
	}

	IEnumerator BackPush(){
		yield return new WaitForSeconds (0.3f); //ahce que espere un poco para que se escuche el sonido
		mainMenu.gameObject.SetActive(true);
		this.gameObject.SetActive(false);
	}
	public void Create(){
		newCharacterName = inputField.text;
		if (newCharacterName.Equals (""))
			return;
		if (File.Exists (Application.persistentDataPath + "/" + newCharacterName + ".dat")) {
			Debug.Log("El nombre ya existe");
			return;
		}
		if (Persistence.CantSavedGames () == Persistence.CantSlots) {
			Debug.Log("No hay mas slots");
			return;
		}
		g.nameToLoad = null;
		Persistence.AddSavedGame (newCharacterName);
		PlayerStats.playerName = newCharacterName;
		UpdateSavedGames ();
	}

	public void LoadSlot1(){
		g.nameToLoad = slots[0].GetComponentInChildren<Text>().text;
		Debug.Log (g.nameToLoad);
		ButtonPressed (0);
	}

	public void LoadSlot2(){
		g.nameToLoad = slots[1].GetComponentInChildren<Text>().text;
		Debug.Log (g.nameToLoad);
		ButtonPressed (1);
	}

	public void LoadSlot3(){
		g.nameToLoad = slots[2].GetComponentInChildren<Text>().text;
		Debug.Log (g.nameToLoad);
		ButtonPressed (2);
	}

	public void LoadSlot4(){
		g.nameToLoad = slots[3].GetComponentInChildren<Text>().text;
		Debug.Log (g.nameToLoad);
		ButtonPressed (3);
	}

	public void LoadSlot5(){
		g.nameToLoad = slots[4].GetComponentInChildren<Text>().text;
		Debug.Log (g.nameToLoad);
		ButtonPressed (4);
	}

	private void ButtonPressed (int index){
		for (int i = 0; i < slots.Length; i++) {
			Image img = slots[i].GetComponent<Image>();
			if(i == index)
				img.color = new Color32(122,86,47,255);
			else
				img.color = new Color(255,255,255);
		}
	}

	public void Delete(){
		if(g.nameToLoad != null){
			Persistence.Delete (g.nameToLoad);
			g.nameToLoad = null;
		}
		UpdateSavedGames();
		ButtonPressed (-1);
	}

	public void Play(){
		if (g.nameToLoad != null) {
			g.levelLoaded = false;
			//Fading.BeginFadeIn("level0");
			Application.LoadLevel ("level0");
		}
	}
}
