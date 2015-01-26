using UnityEngine;
using System.Collections;

public class doorScript : MonoBehaviour {

	#region private variables
	[Range(1,2)]
	[SerializeField]
	private int numberOfTriggers;
	
	[SerializeField]
	private bool bothTriggersNeeded;
	[SerializeField]
	private bool stayOpen;
	
	[SerializeField]
	private GameObject triggerObject1;
	[SerializeField]
	private GameObject triggerObject2;
	[SerializeField]
	private float speed;

	private Animator anim;
	#endregion


	#region public variables
	#endregion


	#region initialization
	void Awake() {
		anim = GetComponent<Animator>();
	}

	void Start () {
	
	}
	#endregion


	#region gameloop
	void Update () {
		if (numberOfTriggers == 1 && triggerObject1.GetComponent<Keyhole>().trigger){
			anim.SetBool("open", true);
		} else if (numberOfTriggers == 2){
			if ( (bothTriggersNeeded && (triggerObject1.GetComponent<Keyhole>().trigger && triggerObject2.GetComponent<Keyhole>().trigger) )
			    || (!bothTriggersNeeded && (triggerObject1.GetComponent<Keyhole>().trigger || triggerObject2.GetComponent<Keyhole>().trigger) ) ){
				anim.SetBool("open", true);
			} else if (!stayOpen) anim.SetBool("open", false);
		} else if (!stayOpen) anim.SetBool("open", false);

		anim.speed = speed;
	}

	void FixedUpdate() {

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

	void OnDrawGizmosSelected(){
		// draw trigger reference(s)
		Gizmos.color = Color.red;
		if (numberOfTriggers >= 1) Gizmos.DrawLine(this.transform.position, triggerObject1.transform.position);
		if (numberOfTriggers >= 2) Gizmos.DrawLine(this.transform.position, triggerObject2.transform.position);
	}
}
