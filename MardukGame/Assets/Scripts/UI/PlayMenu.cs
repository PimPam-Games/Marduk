using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.IO;
using g = GameController;

public class PlayMenu : MonoBehaviour {

	public Image mainMenu;
	public InputField inputField;
	public Button[] slots;
    public GameObject confirmDeleteBtn;

    private string newCharacterName;
	private string[] savedGames;

	private bool escPressed = false;

	public GameObject infoMessages;
	public Text infoTxt;

	public void Start(){
		UpdateSavedGames ();
		inputField.characterLimit = 15;
	}

	void OnEnable() {
		escPressed = false;
        g.nameToLoad = null;
        ButtonPressed(-1);
	}

	public void Update(){
		if(infoMessages.activeSelf)
			return;
		if (Input.GetButtonUp ("Escape") && !escPressed){
			escPressed = true;
			StartCoroutine(BackPush());
		}
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
		if(infoMessages.activeSelf)
			return;
		if(!escPressed)
			StartCoroutine(BackPush());
	}

	IEnumerator BackPush(){
		yield return new WaitForSeconds (0.3f); //ahce que espere un poco para que se escuche el sonido
		mainMenu.gameObject.SetActive(true);
		this.gameObject.SetActive(false);
	}
	public void Create(){
		if(infoMessages.activeSelf)
			return;
		newCharacterName = inputField.text;
		if (newCharacterName.Equals (""))
			return;
	/*	if (File.Exists (Application.persistentDataPath + "/" + newCharacterName + ".dat")) {
        
			infoMessages.SetActive(true);
			infoTxt.text = "Character name already exists";
            confirmDeleteBtn.SetActive(false);
			return;
		}*/
		if (Persistence.CantSavedGames () == Persistence.CantSlots) {
			infoMessages.SetActive(true);
			infoTxt.text = "No more slots available";
            confirmDeleteBtn.SetActive(false);
            return;
		}
        if (!Persistence.AddSavedGame(newCharacterName))
        {
            infoMessages.SetActive(true);
            infoTxt.text = "Character name already exists";
            confirmDeleteBtn.SetActive(false);
            return;
        }
        //g.nameToLoad = null;
        inputField.text = "";
       
        PlayerStats.playerName = newCharacterName;
		UpdateSavedGames ();
	}

	public void InfoMsjButton(){
		infoMessages.SetActive(false);
	}

	public void LoadSlot1(){
		if(infoMessages.activeSelf)
			return;
		g.nameToLoad = slots[0].GetComponentInChildren<Text>().text;
		Debug.Log (g.nameToLoad);
		ButtonPressed (0);
	}

	public void LoadSlot2(){
		if(infoMessages.activeSelf)
			return;
		g.nameToLoad = slots[1].GetComponentInChildren<Text>().text;
		Debug.Log (g.nameToLoad);
		ButtonPressed (1);
	}

	public void LoadSlot3(){
		if(infoMessages.activeSelf)
			return;
		g.nameToLoad = slots[2].GetComponentInChildren<Text>().text;
		Debug.Log (g.nameToLoad);
		ButtonPressed (2);
	}

	public void LoadSlot4(){
		if(infoMessages.activeSelf)
			return;
		g.nameToLoad = slots[3].GetComponentInChildren<Text>().text;
		Debug.Log (g.nameToLoad);
		ButtonPressed (3);
	}

	public void LoadSlot5(){
		if(infoMessages.activeSelf)
			return;
		g.nameToLoad = slots[4].GetComponentInChildren<Text>().text;
		Debug.Log (g.nameToLoad);
		ButtonPressed (4);
	}

	private void ButtonPressed (int index){
		if(infoMessages.activeSelf)
			return;
		for (int i = 0; i < slots.Length; i++) {
			Image img = slots[i].GetComponent<Image>();
			if(i == index)
				img.color = new Color32(122,86,47,255);
			else
				img.color = new Color(255,255,255);
		}
	}

	public void Delete(){
		if(infoMessages.activeSelf)
			return;
		if(g.nameToLoad != null){
            infoMessages.SetActive(true);
            infoTxt.text = "Delete " + g.nameToLoad + " ?";
            confirmDeleteBtn.SetActive(true);
        }		
	}

    public void DeleteConfirmed()
    {
        if (g.nameToLoad != null)
        {
            Persistence.Delete(g.nameToLoad);
            g.nameToLoad = null;
        }
        UpdateSavedGames();
        for (int i = 0; i < slots.Length; i++)
        {
            Image img = slots[i].GetComponent<Image>();
            img.color = new Color(255, 255, 255);
        }
        infoMessages.SetActive(false);
    }

	public void Play(){
		if(infoMessages.activeSelf)
			return;
		if (g.nameToLoad != null) {
			g.levelLoaded = false;
			//Fading.BeginFadeIn("level0");
			Application.LoadLevel ("level0");
		}
	}
}
