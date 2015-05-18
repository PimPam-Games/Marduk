using UnityEngine;
using System.Collections;

public class EnemyBarController : MonoBehaviour {

	public GUITexture hpTexture;
	public Transform target;
	public Vector3 offset;
	public bool clampToScreen = false;
	public float clampBorderSize = 0.5f;
	public bool useMainCamera =  true;
	public Camera cameraToUse;
	private Camera cam;
	private Transform thisTransform;
	private Transform camTransform;


	// Use this for initialization
	void Start () {
		hpTexture = GetComponent<GUITexture> ();
		thisTransform = transform;
		if (useMainCamera)
			cam = Camera.main;
		else
			cam = cameraToUse;
		camTransform = cam.transform;
	
	}
	
	// Update is called once per frame
	void Update () {
		if (clampToScreen) {
			Vector3 relativePosition = camTransform.InverseTransformPoint (target.position);
			relativePosition.z = Mathf.Max (relativePosition.z, 1);
			thisTransform.position = cam.WorldToViewportPoint (camTransform.TransformPoint (relativePosition + offset));
			thisTransform.position = new Vector3 (Mathf.Clamp (thisTransform.position.x, clampBorderSize, 0), Mathf.Clamp (thisTransform.position.y, clampBorderSize, 0), thisTransform.position.z);
		} else {
			thisTransform.position = cam.WorldToViewportPoint(target.position + offset);
		}
	}
}
