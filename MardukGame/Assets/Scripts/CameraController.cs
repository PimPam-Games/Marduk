using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform player;
	public static bool stopFollow = false; // este se llama desde PlatformerCharacter2D y Fading
	public static bool stopFollowX = false;
	public Vector2 margin,smoothing;
	public BoxCollider2D bounds;

	private Vector3 min, max;
	private float nextTimeToSearch = 0;

	public bool isFollowing{ get; set;}
    private Camera cam;
    private bool maximized = false;

	public void Start(){
        cam = GetComponent<Camera>();
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
		if (stopFollow)
			return;
		var x = transform.position.x;
		var y = transform.position.y;
		if(isFollowing){
			if(!stopFollowX){
				if(Mathf.Abs(x - player.position.x)>margin.x)
					x = Mathf.Lerp(x,player.position.x, smoothing.x * Time.deltaTime);
			}
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
			GameObject searchResult = GameObject.FindGameObjectWithTag("CameraCenter");
			if(searchResult != null){
				player = searchResult.transform;
			}
			nextTimeToSearch = Time.time + 0.5f;
		}
	}

    public void ToggleMinimap()
    {
        if (!maximized)
        {
            cam.orthographicSize = 100;
            cam.rect = new Rect(0.1f, 0.1f, 0.8f, 0.7f);
            maximized = true;
        }
        else
        {
            cam.orthographicSize = 30;
            cam.rect = new Rect(0.79f, 0.76f, 0.2f, 0.22f);
            maximized = false;
        }
    }
}

