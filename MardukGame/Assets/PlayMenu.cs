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
		savedGames = Persistence.GetSavedGames ();
		if (savedGames == null)
			return;
		for (int i = 0; i < savedGames.Length; i++) {
			if(savedGames[i] != null){
				slots[i].gameObject.SetActive(true);
				slots[i].GetComponentInChildren<Text>().text = savedGames[i];
			}
		}
	}

	public void Back(){
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
		Application.LoadLevel ("level0");
	}

	public void LoadSlot1(){
		g.nameToLoad = slots[0].GetComponentInChildren<Text>().text;
		Debug.Log (g.nameToLoad);
		Application.LoadLevel ("level0");
	}

	public void LoadSlot2(){
		g.nameToLoad = slots[1].GetComponentInChildren<Text>().text;
		Debug.Log (g.nameToLoad);
		Application.LoadLevel ("level0");
	}

	public void LoadSlot3(){
		g.nameToLoad = slots[2].GetComponentInChildren<Text>().text;
		Debug.Log (g.nameToLoad);
		Application.LoadLevel ("level0");
	}

	public void LoadSlot4(){
		g.nameToLoad = slots[3].GetComponentInChildren<Text>().text;
		Debug.Log (g.nameToLoad);
		Application.LoadLevel ("level0");
	}

	public void LoadSlot5(){
		g.nameToLoad = slots[4].GetComponentInChildren<Text>().text;
		Debug.Log (g.nameToLoad);
		Application.LoadLevel ("level0");
	}
}
