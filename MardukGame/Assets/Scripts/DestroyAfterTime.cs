using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

	public float lifeTime;
	public bool hasBoxCollider = false;
	public float colliderLifetime;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		StartCoroutine (RemoveAfterTime (lifeTime));
		if (hasBoxCollider)
			StartCoroutine (RemoveColAfterTime(colliderLifetime));
	}

	IEnumerator RemoveAfterTime(float lifeTime) {		
		yield return new WaitForSeconds (lifeTime);
		Destroy (this.gameObject);
	}

	IEnumerator RemoveColAfterTime(float collLifetime) {
		yield return new WaitForSeconds (collLifetime);
		GetComponent<BoxCollider2D>().enabled = false;
		
	}
}
