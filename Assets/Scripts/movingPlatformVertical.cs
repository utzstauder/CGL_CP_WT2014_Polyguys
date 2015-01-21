using UnityEngine;
using System.Collections;

public class movingPlatformVertical : MonoBehaviour {

	[SerializeField]
	// true = up; false = down;
	private bool direction;

	[Range(1,2)]
	[SerializeField]
	private int numberOfTriggers;

	[SerializeField]
	private GameObject triggerObject1;
	[SerializeField]
	private GameObject triggerObject2;
	[SerializeField]
	private float speed;

	[SerializeField]
	private float timeDelay;

	[SerializeField]
	private float yMin;
	[SerializeField]
	private float yMax;

	private Vector3 initialPosition;

	// Use this for initialization
	void Start () {
		initialPosition = this.transform.position;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (numberOfTriggers == 1 && triggerObject1.GetComponent<Keyhole>().trigger){
			MovePlatform(direction);
		} else if (numberOfTriggers == 2){
			if (triggerObject1.GetComponent<Keyhole>().trigger || triggerObject2.GetComponent<Keyhole>().trigger){
				MovePlatform(direction);
			} else MovePlatform(!direction);
		} else MovePlatform(!direction);
	}

//	void Raise(){
//		// raise the platform
//		if ((transform.position.y - initialPosition.y) < yMax)
//		transform.position += new Vector3(0,Time.deltaTime * speed,0);
//	}
//
//	void Lower(){
//		// lower the platform
//		if ((transform.position.y - initialPosition.y) > yMin)
//		transform.position -= new Vector3(0,Time.deltaTime * speed,0);
//	}

	void MovePlatform(bool up){
		if (up){
			// raise the platform
			if ((transform.position.y - initialPosition.y) < yMax)
				transform.position += new Vector3(0,Time.deltaTime * speed,0);
		} else{ 
			// lower the platform
			if ((transform.position.y - initialPosition.y) > yMin)
				transform.position -= new Vector3(0,Time.deltaTime * speed,0);
			}
	}

	void OnDrawGizmosSelected(){
		// draw movement line
		Gizmos.color = Color.blue;
		if (Application.isPlaying) Gizmos.DrawLine(initialPosition + new Vector3(0,yMin,0),initialPosition + new Vector3(0,yMax,0));
			else Gizmos.DrawLine(this.transform.position + new Vector3(0,yMin,0),this.transform.position + new Vector3(0,yMax,0));

		// draw trigger reference(s)
		Gizmos.color = Color.red;
		if (numberOfTriggers >= 1) Gizmos.DrawLine(this.transform.position, triggerObject1.transform.position);
		if (numberOfTriggers >= 2) Gizmos.DrawLine(this.transform.position, triggerObject2.transform.position);
	}

}
