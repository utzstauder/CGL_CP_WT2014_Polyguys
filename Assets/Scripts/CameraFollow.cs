using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	// Link to game manager
	public GameManager gm;

	public Transform player1;
	public Transform player2;

	// Link to camera
	private Camera camera;

	// virtual target object
	[HideInInspector]
	public Vector3 cameraTarget;

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
	[SerializeField]
	private float zoomFactor = 0;

	[HideInInspector]
	public float externalZoomFactor = 0; 
	[HideInInspector]
	public Vector3 externalOffset;
	
	[SerializeField]
	private Vector3 offset;

	private float playerDistance;

	// Use this for initialization
	void Start () {
//		gm = GameObject.Find ("_GM").GetComponent<GameManager>();
		camera = GetComponent<Camera>();
		cameraTarget = (GameObject.Find("StartP1").transform.position + GameObject.Find("StartP2").transform.position)/2 + new Vector3(0,0,this.transform.position.z);
		externalZoomFactor = 0;
		externalOffset = Vector3.zero;
	}

	void LateUpdate () {
//		if (gm) if (gm != null && gm.players[1] != null && gm.players[0] != null){
//			player1 = gm.players[0].transform;
//			player2 = gm.players[1].transform;
//		}
		if (player1 && player2){
			cameraTarget = (player2.position + player1.position)/2;
			playerDistance = Vector3.Distance(player2.position, player1.position);
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
		transform.position = new Vector3(newX + offset.x + externalOffset.x, newY + offset.y + externalOffset.y, this.transform.position.z);


		// zoom
		if (!lockZoom){
			if (playerDistance/2 > minSize && playerDistance/2 < maxSize){
				camera.orthographicSize = playerDistance/2;
			} else if (playerDistance/2 < minSize){
				camera.orthographicSize = minSize;
			} else camera.orthographicSize = maxSize;

			camera.orthographicSize *= 1f + zoomFactor + externalZoomFactor;
		}
	}
}
