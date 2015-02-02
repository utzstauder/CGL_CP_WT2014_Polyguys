using UnityEngine;
using System.Collections;

public class obstacleSpawner : MonoBehaviour {

	#region private variables
	[SerializeField]
	private GameObject obstaclePrefab;
	
	[SerializeField]
	private Vector2 waitInterval;
	private Vector2 initialWaitInterval;

	[SerializeField]
	private bool debug;
	#endregion


	#region public variables
	#endregion


	#region initialization
	void Awake() {

	}

	void Start () {
		initialWaitInterval = waitInterval;
		StartCoroutine(DropAtRandom());
	}
	#endregion


	#region gameloop
	void Update () {
		if (debug){
			if (Input.GetKeyDown(KeyCode.Alpha3)) waitInterval -= initialWaitInterval/10;
			if (Input.GetKeyDown(KeyCode.Alpha4)) waitInterval += initialWaitInterval/10;
			if (Input.GetKeyDown(KeyCode.Alpha5)) waitInterval = initialWaitInterval;
		}
	}

	void FixedUpdate() {

	}
	#endregion


	#region methods
	void Drop(){
//		Debug.Log ("Drop()");
		GameObject drop = Instantiate(obstaclePrefab, this.transform.position, this.transform.rotation) as GameObject;
		drop.GetComponent<Obstacle>().Init(Mathf.RoundToInt(Random.Range(3f,8f)));
	}
	#endregion


	#region functions
	#endregion

	#region colliders
	#endregion

	#region triggers
	#endregion

	#region coroutines
	private IEnumerator DropAtRandom(){
		while (true){
			yield return new WaitForSeconds(Random.Range(waitInterval.x, waitInterval.y));
			Drop();
		}
	}
	#endregion
}
