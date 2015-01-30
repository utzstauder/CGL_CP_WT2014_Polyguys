using UnityEngine;
using System.Collections;

public class zoomTriggerScript : MonoBehaviour {

	#region private variables
	[SerializeField]
	private CameraFollow cameraScript;

	[SerializeField]
	private float zoomFactor;
	[SerializeField]
	private Vector3 offset;

	private float distanceToCamera;
	private CircleCollider2D trigger;
	private float triggerRadius;
	private float factor = 0;
	#endregion


	#region public variables
	#endregion


	#region initialization
	void Awake() {
		trigger = GetComponent<CircleCollider2D>();
		cameraScript = GameObject.Find ("MainCamera").GetComponent<CameraFollow>();
	}

	void Start () {
	
	}
	#endregion


	#region gameloop
	void Update () {
		triggerRadius = trigger.radius;

		distanceToCamera = Vector2.Distance(new Vector2(cameraScript.transform.position.x,cameraScript.transform.position.y) , new Vector2(this.transform.position.x, this.transform.position.y));

		factor = (triggerRadius-distanceToCamera)/triggerRadius;
		if (factor <= 0){
			factor = 0;
		}
//		Debug.Log ("(" + triggerRadius + " - " + distanceToCamera + ") / " + triggerRadius + " = " + factor);
//		Debug.Log ("distanceToCamera" + distanceToCamera + " <= " + triggerRadius + "triggerRadius");
		if (distanceToCamera <= triggerRadius){
			cameraScript.externalZoomFactor = zoomFactor * factor;
			cameraScript.externalOffset = offset * factor;
		}

	}
	
	#endregion


	#region methods
	#endregion


	#region functions
	#endregion

	#region colliders
	#endregion

	#region triggers
//	void OnTriggerEnter2D(Collider2D other){
//		if (other.gameObject.tag == "Player"){
//			if (other.gameObject.name == "Player1") Player1InTrigger = true;
//			if (other.gameObject.name == "Player2") Player2InTrigger = true;
//		}
//	}
//
//	void OnTriggerExit2D(Collider2D other){
//		if (other.gameObject.tag == "Player"){
//			if (other.gameObject.name == "Player1") Player1InTrigger = false;
//			if (other.gameObject.name == "Player2") Player2InTrigger = false;
//		}
//	}
	#endregion

	#region coroutines
	#endregion
}
