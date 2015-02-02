using UnityEngine;
using System.Collections;

public class newDoorScript : MonoBehaviour {

	#region private variables
	[Range(1,2)]
	[SerializeField]
	private int numberOfTriggers;
	
	[SerializeField]
	private bool bothTriggersNeeded;

	[SerializeField]
	private bool closes;

	[SerializeField]
	private bool stayOpen;
	
	[SerializeField]
	private GameObject triggerObject1;
	[SerializeField]
	private GameObject triggerObject2;

	[SerializeField]
	private float openSpeed;
	[SerializeField]
	private bool openSpeedEqualsCloseSpeed = true;
	[SerializeField]
	private float closeSpeed;
	
	private Transform[] doorParts;

	private float minScale = 0.03f;
	private float maxScale = 1f;
	#endregion	


	#region public variables
	#endregion


	#region initialization
	void Awake() {
		doorParts = new Transform[2]{transform.Find ("doorTop"), transform.Find ("doorBottom")};
	}

	void Start () {
	
	}
	#endregion


	#region gameloop
	void Update () {
		if (numberOfTriggers == 1 && triggerObject1.GetComponent<Keyhole>().trigger){
			if (!closes) Open ();
			else Close ();
		} else if (numberOfTriggers == 2){
			if ( (bothTriggersNeeded && (triggerObject1.GetComponent<Keyhole>().trigger && triggerObject2.GetComponent<Keyhole>().trigger) )
			    || (!bothTriggersNeeded && (triggerObject1.GetComponent<Keyhole>().trigger || triggerObject2.GetComponent<Keyhole>().trigger) ) ){
				if (!closes) Open ();
				else Close ();
			} else if (!stayOpen) if (closes) Open ();
									else Close ();
		} else if (!stayOpen) if (closes) Open ();
								else Close ();
	}

	void FixedUpdate() {

	}
	#endregion


	#region methods
	private void Open(){
		foreach (Transform part in doorParts){
			if (part.localScale.z > minScale)
			part.localScale -= new Vector3(0,0,Time.deltaTime * openSpeed);
		}
	}

	private void Close(){
		foreach (Transform part in doorParts){
			if (part.localScale.z <= maxScale)
				if (openSpeedEqualsCloseSpeed) part.localScale += new Vector3(0,0,Time.deltaTime * openSpeed);
			else part.localScale += new Vector3(0,0,Time.deltaTime * closeSpeed);
		}
	}
	#endregion
	
	void OnDrawGizmosSelected(){
		// draw trigger reference(s)
		Gizmos.color = Color.red;
		if (numberOfTriggers >= 1 && triggerObject1) Gizmos.DrawLine(this.transform.position, triggerObject1.transform.position);
		if (numberOfTriggers >= 2 && triggerObject1 && triggerObject2) Gizmos.DrawLine(this.transform.position, triggerObject2.transform.position);
	}
}
