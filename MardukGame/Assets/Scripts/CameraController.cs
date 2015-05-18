using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform player;

	public Vector2 margin,smoothing;
	public BoxCollider2D bounds;

	private Vector3 min, max;
	private float nextTimeToSearch = 0;

	public bool isFollowing{ get; set;}

	public void Start(){
		FindPlayer ();
		isFollowing = true;
		if (bounds != null){
			min = bounds.bounds.min;
			max = bounds.bounds.max;
		}
	}

	public void Update(){
		if (bounds == null)
			return;
		if(player == null || PlayerStats.isDead){
			FindPlayer();
			return;
		}
		var x = transform.position.x;
		var y = transform.position.y;
		if(isFollowing){

			if(Mathf.Abs(x - player.position.x)>margin.x)
				x = Mathf.Lerp(x,player.position.x, smoothing.x * Time.deltaTime);
			if(Mathf.Abs(y - player.position.y)> margin.y)
				y = Mathf.Lerp(y, player.position.y, smoothing.y * Time.deltaTime);
		}

		var cameraHalfWidth = Camera.main.orthographicSize * ((float)Screen.width / Screen.height);
		x = Mathf.Clamp (x, min.x + cameraHalfWidth, max.x - cameraHalfWidth);
		y = Mathf.Clamp (y, min.y + Camera.main.orthographicSize, max.y - Camera.main.orthographicSize);
		transform.position = new Vector3 (x, y, transform.position.z);
	}

	public void SetBounds(BoxCollider2D newBounds){
		bounds = newBounds;
		min = bounds.bounds.min;
		max = bounds.bounds.max;
	}

	void FindPlayer(){
		if(nextTimeToSearch <= Time.time){
			GameObject searchResult = GameObject.FindGameObjectWithTag("Player");
			if(searchResult != null){
				player = searchResult.transform;
			}
			nextTimeToSearch = Time.time + 0.5f;
		}
	}
}

