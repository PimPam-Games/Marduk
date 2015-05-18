using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

	public float lifeTime;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine (RemoveAfterTime (lifeTime));
	}

	IEnumerator RemoveAfterTime(float lifeTime) {		
		yield return new WaitForSeconds (lifeTime);
		Destroy (this.gameObject);
	}
}
