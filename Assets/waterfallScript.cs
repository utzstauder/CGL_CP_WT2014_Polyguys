using UnityEngine;
using System.Collections;

public class waterfallScript : MonoBehaviour {

	#region private variables
	[SerializeField]
	private bool on = true;
	[SerializeField]
	private GameObject dropPrefab;
	[SerializeField]
	private Vector2 waitInterval;
	private bool isDropping = false;

	[SerializeField]
	private bool activatedByTrigger = false;
	[Range(1,2)]
	[SerializeField]
	private int numberOfTriggers;
	[SerializeField]
	private bool bothTriggersNeeded;
	[SerializeField]
	private bool stayOn;
	[SerializeField]
	private GameObject triggerObject1;
	[SerializeField]
	private GameObject triggerObject2;
	#endregion


	#region public variables
	#endregion


	#region initialization
	void Awake() {

	}

	void Start () {
		if (on) StartCoroutine(DropAtRandom());
	}
	#endregion


	#region gameloop
	void Update () {
		if (activatedByTrigger){
			if (numberOfTriggers == 1 && triggerObject1.GetComponent<Keyhole>().trigger){
				if (!isDropping) StartCoroutine(DropAtRandom());
			} else if (numberOfTriggers == 2){
				if ( (bothTriggersNeeded && (triggerObject1.GetComponent<Keyhole>().trigger && triggerObject2.GetComponent<Keyhole>().trigger) )
				    || (!bothTriggersNeeded && (triggerObject1.GetComponent<Keyhole>().trigger || triggerObject2.GetComponent<Keyhole>().trigger) ) ){
					if (!isDropping) StartCoroutine(DropAtRandom());
				} else if (isDropping) StopCoroutine(DropAtRandom());
			} else if (isDropping) StopCoroutine(DropAtRandom());
		} else if (on && !isDropping) StartCoroutine(DropAtRandom());
		else if (!on && isDropping) StopCoroutine(DropAtRandom());
	}

	void FixedUpdate() {

	}
	#endregion


	#region methods
	private void Drop(){
		GameObject drop = Instantiate(dropPrefab, this.transform.position, this.transform.rotation) as GameObject;
		drop.transform.localScale = this.transform.localScale;
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
		isDropping = true;
		while (true){
			yield return new WaitForSeconds(Random.Range(waitInterval.x, waitInterval.y));
			Drop();
		}
	}
	#endregion

	void OnDrawGizmosSelected(){
		// draw trigger reference(s)
		if (activatedByTrigger){
			Gizmos.color = Color.red;
			if (numberOfTriggers >= 1 && triggerObject1) Gizmos.DrawLine(this.transform.position, triggerObject1.transform.position);
			if (numberOfTriggers >= 2 && triggerObject1 && triggerObject2) Gizmos.DrawLine(this.transform.position, triggerObject2.transform.position);
		}
	}
}
