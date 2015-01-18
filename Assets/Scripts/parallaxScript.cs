using UnityEngine;
using System.Collections;

public class parallaxScript : MonoBehaviour {

	#region private variables
	[SerializeField]
	private Camera mainCamera;
	
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

		if (scrollX) newX = this.transform.position.x + mainCamera.velocity.x * scrollFactor.x * .1f + offset.x;
		else newX = this.transform.position.x;
		if (scrollY) newY = this.transform.position.y + mainCamera.velocity.y * scrollFactor.y * .1f + offset.y;
		else newY = this.transform.position.y;
		if (scrollZ) newZ = this.transform.position.z + mainCamera.velocity.z * scrollFactor.z * .1f + offset.z;
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
