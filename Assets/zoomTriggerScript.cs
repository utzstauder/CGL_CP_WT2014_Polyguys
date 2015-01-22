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
	private float triggerRadius;
	private float factor;
	#endregion


	#region public variables
	#endregion


	#region initialization
	void Awake() {
		triggerRadius = GetComponent<CircleCollider2D>().radius;
	}

	void Start () {
	
	}
	#endregion


	#region gameloop
	void Update () {

	}

	void FixedUpdate() {
		distanceToCamera = Vector3.Distance(cameraScript.cameraTarget, this.transform.position);
		
		if (distanceToCamera <= triggerRadius){
			factor = (triggerRadius-distanceToCamera)/triggerRadius;
			cameraScript.externalZoomFactor = zoomFactor * factor;
			cameraScript.externalOffset = offset * factor;
		}
		else cameraScript.externalOffset = Vector3.zero;
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
