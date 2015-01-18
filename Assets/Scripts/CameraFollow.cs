using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	// Link to game manager
	public GameManager gm;

	// Link to camera
	private Camera camera;

	// Link to target object
	private Vector3 cameraTarget;

	[Range(0,10.0f)]
	public float minSize;
	[Range(10.0f, 30.0f)]
	public float maxSize;

	public bool limitX;
	public float xMin;
	public float xMax;
	public bool limitY;
	public float yMin;
	public float yMax;

	public bool lockX;
	public bool lockY;
	public bool lockZoom;

	[Range(0,10.0f)]
	public float xOffset;
	[Range(0,10.0f)]
	public float yOffset;
	public float zOffset;

	private float playerDistance;

	// Use this for initialization
	void Start () {
		gm = GameObject.Find ("_GM").GetComponent<GameManager>();
		camera = GetComponent<Camera>();
		cameraTarget = Vector3.zero;
	}

	void LateUpdate () {
		if (gm != null && gm.players[1] != null && gm.players[0] != null){
			cameraTarget = (gm.players[1].transform.position + gm.players[0].transform.position)/2;
			playerDistance = Vector3.Distance(gm.players[1].transform.position, gm.players[0].transform.position);
		}

		float newX = 0;
		float newY = 0;
		if (lockX) newX = transform.position.x;
		else {
			newX = cameraTarget.x;
			if (limitX){
				if (newX < xMin) newX = xMin;
				else if (newX > xMax) newX = xMax;
			}
		}
		if (lockY) newY = transform.position.y;
		else {
			newY = cameraTarget.y;
			if (limitY){
				if (newY < yMin) newY = yMin;
				else if (newY > yMax) newY = yMax;
			}
		}

		// move to target
		transform.position = new Vector3(newX+xOffset, newY+yOffset, zOffset);


		// zoom
		if (!lockZoom){
			if (playerDistance/2 > minSize && playerDistance/2 < maxSize){
				camera.orthographicSize = playerDistance/2;
			} else if (playerDistance/2 < minSize){
				camera.orthographicSize = minSize;
			} else camera.orthographicSize = maxSize;
		}
	}
}
