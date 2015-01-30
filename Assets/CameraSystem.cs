using UnityEngine;
using System.Collections;

public class CameraSystem : MonoBehaviour {

	#region private variables
	private GameObject[] waypoints;
	private Vector3 cameraTarget;
	private float zOffset = -30f;
	private BoxCollider2D[] limitCollider;
	#endregion


	#region public variables
	[HideInInspector]
	public Transform player1;
	[HideInInspector]
	public Transform player2;
	#endregion


	#region initialization
	void Awake() {
		waypoints = GameObject.FindGameObjectsWithTag("Waypoint");
		this.transform.position = GetCameraTarget();
		limitCollider = transform.Find("collider").GetComponents<BoxCollider2D>();
	}

	void Start () {
	
	}
	#endregion


	#region gameloop
	void Update () {
	
	}

	void FixedUpdate() {

	}

	void LateUpdate(){
		//set up variables
		float newX = GetCameraTarget().x;
		float newY = GetCameraTarget().y;
		float newZ = GetCameraTarget().z;

		//calculate x position
		newX = GetCameraTarget().x;

		//calculate y position
		newY = (GetNearestWaypointRight(newX).transform.position.y - GetNearestWaypointLeft(newX).transform.position.y)/(GetNearestWaypointRight(newX).transform.position.x - GetNearestWaypointLeft(newX).transform.position.x) * (newX - GetNearestWaypointLeft(newX).transform.position.x) + GetNearestWaypointLeft(newX).transform.position.y;

		//calculate progression on current waypoint segment
		float progression = Vector2.Distance(new Vector2(GetNearestWaypointLeft(newX).transform.position.x, GetNearestWaypointLeft(newX).transform.position.y), new Vector3 (newX, newY, newZ))
			/ Vector2.Distance(new Vector2(GetNearestWaypointLeft(newX).transform.position.x, GetNearestWaypointLeft(newX).transform.position.y), new Vector2(GetNearestWaypointRight(newX).transform.position.x, GetNearestWaypointRight(newX).transform.position.y));
		progression = Mathf.Clamp(progression, 0, 1f);

		//calculate new zoom
		float newZoom = Mathf.SmoothStep (GetNearestWaypointLeft(newX).zoom, GetNearestWaypointRight(newX).zoom,  progression);

		//update the limiting colliders
		UpdateColliders(newZoom);

		//set new zoom
		this.camera.orthographicSize = newZoom;

		//set new position
		this.transform.position = new Vector3(newX, newY, newZ + zOffset);
	}
	#endregion


	#region methods
	private void UpdateColliders(float zoom){
		limitCollider[0].size = new Vector2(1f, zoom * 2f);
		limitCollider[1].size = new Vector2(1f, zoom * 2f);
		limitCollider[0].center = new Vector2 (zoom * camera.aspect + limitCollider[1].size.x/2, 0);
		limitCollider[1].center = new Vector2 (-zoom * camera.aspect - limitCollider[1].size.x/2, 0);
	}
	#endregion


	#region functions
	private Vector3 GetCameraTarget(){
		if (player1 && player2){
			return (player2.position + player1.position)/2;
		}
		return Vector3.zero;
	}

	private WaypointScript GetFirstWaypoint(){
		Transform returnValueTranform = waypoints[0].transform;

		foreach(GameObject waypoint in waypoints){
			if (waypoint.transform.position.x < returnValueTranform.position.x) returnValueTranform = waypoint.transform;
			}
		return returnValueTranform.GetComponent<WaypointScript>();
	}

	private WaypointScript GetLastWaypoint(){
		Transform returnValueTranform = waypoints[0].transform;

		foreach(GameObject waypoint in waypoints){
			if (waypoint.transform.position.x > returnValueTranform.position.x) returnValueTranform = waypoint.transform;
		}
	return returnValueTranform.GetComponent<WaypointScript>();
	}

		private WaypointScript GetNearestWaypointLeft(float x){
		Transform returnValueTranform = GetFirstWaypoint().transform;

			foreach(GameObject waypoint in waypoints){
				if (waypoint.transform.position.x < x){
				if (!returnValueTranform) returnValueTranform = waypoint.transform;
					else if (waypoint.transform.position.x > returnValueTranform.position.x) returnValueTranform = waypoint.transform;
				}
			}

		return returnValueTranform.GetComponent<WaypointScript>();
		}

		private WaypointScript GetNearestWaypointRight(float x){
			Transform returnValueTranform = GetLastWaypoint().transform;

			foreach(GameObject waypoint in waypoints){
				if (waypoint.transform.position.x > x){
					if (waypoint.transform.position.x < returnValueTranform.position.x) returnValueTranform = waypoint.transform;
				}
			}
			return returnValueTranform.GetComponent<WaypointScript>();
		}

	private Vector3 CurrentWaypointSegment(float x){
//		Debug.Log (GetNearestWaypointLeft(x).transform.position + " to " + GetNearestWaypointRight(x).transform.position);
		return (GetNearestWaypointLeft(x).transform.position - GetNearestWaypointRight(x).transform.position);
	}
	
	#endregion

	void OnDrawGizmos(){
		if (Application.isPlaying){
			Gizmos.color = Color.red;
			Gizmos.DrawLine(this.transform.position, GetNearestWaypointLeft(GetCameraTarget().x).transform.position);
			Gizmos.color = Color.white;
			Gizmos.DrawLine(this.transform.position, GetNearestWaypointRight(GetCameraTarget().x).transform.position);

		}
	}

	#region coroutines
	#endregion
}
