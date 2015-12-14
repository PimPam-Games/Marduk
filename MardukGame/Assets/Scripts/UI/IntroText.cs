using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class IntroText : MonoBehaviour {

	public Image background;
	public Text text;
	public Text skipText;
	public static bool introVisible = true;
	private float duration = 45;
	private bool skip = false;
	private bool routineStarted = false;
	// Use this for initialization
	void Start () {
		//StartCoroutine(EnterToSkip());
	}
	
	// Update is called once per frame
	void Update () {
		duration -= Time.deltaTime;
		if(Input.GetButtonUp("Submit"))
			skip = true;
		if((duration < 0 || skip) && !routineStarted){
			routineStarted = true;
			StartCoroutine(IntroFade());
		}
		
		
	}

	IEnumerator IntroFade(){
		while(background.color.a > 0 && text.color.a > 0){
			yield return new WaitForSeconds (0.04f);
			background.color = new Color(1,1,1,background.color.a - 0.05f);
			text.color = new Color(1,1,1,text.color.a - 0.09f);
		}
		introVisible = false;
		Destroy(this.gameObject); 
	}
	
	IEnumerator EnterToSkip(){ // no se esta usando por ahora
		bool textTransparent  = false;
		while(true){
			yield return new WaitForSeconds (0.3f);
			if(!textTransparent)
				skipText.color = new Color(1,1,1,0.2f);
			else
				skipText.color = new Color(1,1,1,0.4f);
			textTransparent = !textTransparent;			
		} 
	}

}
