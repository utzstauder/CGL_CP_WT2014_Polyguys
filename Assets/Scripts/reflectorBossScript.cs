using UnityEngine;
using System.Collections;

public class reflectorBossScript : MonoBehaviour {

	#region private variables
	[SerializeField]
	private BossScript boss;

	[SerializeField]
	private Keyhole trigger;

	[SerializeField]
	private GameObject mesh;
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
		if (trigger.trigger || boss.phase > 5 || boss.inCooldown) Deactivate();
		else Activate();
	}
	#endregion


	#region methods
	private void Activate(){
		GetComponent<BoxCollider2D>().enabled = true;
		mesh.SetActive(true);
	}

	private void Deactivate(){
		GetComponent<BoxCollider2D>().enabled = false;
		mesh.SetActive(false);
	}
	#endregion

	void OnDrawGizmosSelected(){
		Gizmos.color = Color.red;
		Gizmos.DrawLine (this.transform.position, trigger.transform.position);
	}

	#region coroutines
	private IEnumerator triggerCheck(){
		yield return new WaitForSeconds(2f);
		if (trigger.trigger) Deactivate();
	} 
	#endregion
}
