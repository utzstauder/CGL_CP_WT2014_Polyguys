using UnityEngine;
using System.Collections;

public class setLightsForPlayer : MonoBehaviour {

	#region private variables
	private Light light;
	#endregion


	#region public variables
	public PlatformerCharacter2D player;
	#endregion


	#region initialization
	void Awake() {
		light = GetComponent<Light>();
	}

	void Start () {
	
	}
	#endregion


	#region gameloop
	void Update () {
	
	}

	void FixedUpdate() {
		light.color = player.color;
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
