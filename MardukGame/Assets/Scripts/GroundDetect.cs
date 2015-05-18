using UnityEngine;
using System.Collections;

public class GroundDetect : MonoBehaviour {

	[SerializeField] private LayerMask whatIsGround;
	private Transform groudDetect;
	// Use this for initialization
	void Start () {
		groudDetect = transform.FindChild ("ItemGroundCheck");
	}

	// Update is called once per frame
	void Update () {
		if (Physics2D.OverlapCircle ( new Vector2(groudDetect.position.x ,groudDetect.position.y ) , 0.25f, whatIsGround))
			GetComponent<Rigidbody2D> ().isKinematic = true;
	}
}
