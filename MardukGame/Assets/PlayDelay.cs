using UnityEngine;
using System.Collections;

public class PlayDelay : MonoBehaviour {

	public AudioSource sound;
	public float delaySecs;
	// Use this for initialization
	void Start () {
		sound.PlayDelayed (delaySecs);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}
