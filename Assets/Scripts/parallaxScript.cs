using UnityEngine;
using System.Collections;

public class parallaxScript : MonoBehaviour {

	#region private variables
	[SerializeField]
	private Camera mainCamera;

	private float distanceFromCamera;

	[SerializeField]
	private bool scrollX;
	[SerializeField]
	private bool scrollY;
	[SerializeField]
	private bool scrollZ;

	[SerializeField]
	private Vector3 scrollFactor;
	[SerializeField]
	private Vector3 offset;
	#endregion


	#region public variables
	#endregion


	#region initialization
	void Awake() {
		distanceFromCamera = Mathf.Abs(mainCamera.transform.position.z - this.transform.position.z);
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
		float newX;
		float newY;
		float newZ;

		if (scrollX) newX = this.transform.position.x + mainCamera.velocity.x * scrollFactor.x * 1/distanceFromCamera + offset.x;
		else newX = this.transform.position.x;
		if (scrollY) newY = this.transform.position.y + mainCamera.velocity.y * scrollFactor.y * 1/distanceFromCamera + offset.y;
		else newY = this.transform.position.y;
		if (scrollZ) newZ = this.transform.position.z + mainCamera.velocity.z * scrollFactor.z * 1/distanceFromCamera + offset.z;
		else newZ = this.transform.position.z;

		this.transform.position = new Vector3(newX, newY, newZ);
	}
	#endregion


	#region methods
	#endregion


	#region functions
	#endregion

	#region colliders
	#endregion

	#region triggers
	#endregion

	#region coroutines
	#endregion
}
