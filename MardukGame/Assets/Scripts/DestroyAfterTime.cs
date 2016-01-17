using UnityEngine;
using System.Collections;

public class DestroyAfterTime : MonoBehaviour {

	public float lifeTime;
	public bool hasBoxCollider = false;
	public float colliderLifetime;
	public bool disableAfterTime = false;
	// Use this for initialization
	void Start () {
		StartCoroutine (RemoveAfterTime (lifeTime));
		if (hasBoxCollider)
			StartCoroutine (RemoveColAfterTime(colliderLifetime));
	}

	void OnEnable(){
		StartCoroutine (RemoveAfterTime (lifeTime));
		if (hasBoxCollider)
			StartCoroutine (RemoveColAfterTime(colliderLifetime));
	}
	
	IEnumerator RemoveAfterTime(float lifeTime) {		
		yield return new WaitForSeconds (lifeTime);
		if(!disableAfterTime)
			Destroy (this.gameObject);
		else
			this.gameObject.SetActive(false);
	}

	IEnumerator RemoveColAfterTime(float collLifetime) {
		yield return new WaitForSeconds (collLifetime);
		GetComponent<BoxCollider2D>().enabled = false;
		
	}
}
