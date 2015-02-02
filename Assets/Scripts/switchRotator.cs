using UnityEngine;
using System.Collections;

public class switchRotator : MonoBehaviour {

	#region private variables
	[SerializeField]
	private Keyhole trigger;
	[SerializeField]
	private float rotationSpeed = .8f;
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
		if (trigger.trigger) transform.Rotate(Vector3.up * Time.fixedDeltaTime *rotationSpeed);
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
