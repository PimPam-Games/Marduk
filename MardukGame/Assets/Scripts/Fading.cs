using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using g = GameController;

public class Fading : MonoBehaviour {

	public static Image fadeImage;
	private static Animator anim;
	private static string sceneToLoad;

	void Awake(){
		anim = GetComponent<Animator> ();
		fadeImage = GetComponent<Image> ();
	}

	public static void BeginFadeIn (string newScene)
	{
		sceneToLoad = newScene;
		anim.SetBool ("FadeOut",false);
		anim.SetBool ("FadeIn",true);
	}

	public static void BeginFadeOut ()
	{
		anim.SetBool ("FadeOut",true);
		anim.SetBool ("FadeIn",false);
	}

	public void LoadScene(){
		g.SetActiveEnemies(g.currLevelName,false);
		g.SetActiveChunks(g.currLevelName,false);
		g.currLevelName = sceneToLoad;
		Application.LoadLevel (sceneToLoad);
		if(g.chunksPerZone.ContainsKey(g.currLevelName))
			g.SetActiveChunks(g.currLevelName,true);
		if(g.enemiesPerLevel.ContainsKey(g.currLevelName))
			g.SetActiveEnemies(g.currLevelName,true);
		BeginFadeOut ();
	}

	// OnLevelWasLoaded is called when a level is loaded. It takes loaded level index (int) as a parameter so you can limit the fade in to certain scenes.
	//void OnLevelWasLoaded()
	//{
		// alpha = 1;		// use this if the alpha is not set to 1 by default
	//	BeginFade(-1);		// call the fade in function
	//}
}