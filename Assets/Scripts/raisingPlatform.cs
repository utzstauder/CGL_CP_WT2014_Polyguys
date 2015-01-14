using UnityEngine;
using System.Collections;

public class raisingPlatform : MonoBehaviour {

	[Range(1,2)]
	public int numberOfTriggers;

	public GameObject triggerObject;
	public GameObject triggerObject2;
	public float speed;

	public float yMin;
	public float yMax;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (numberOfTriggers == 1 && triggerObject.GetComponent<Keyhole>().trigger){
			Raise ();
		} else if (numberOfTriggers == 2){
			if (triggerObject.GetComponent<Keyhole>().trigger || triggerObject2.GetComponent<Keyhole>().trigger){
				Raise ();
			} else Lower ();
		} else Lower ();
	}

	void Raise(){
		// raise the platform
		if (transform.localPosition.y < yMax)
		transform.position += new Vector3(0,Time.deltaTime * speed,0);
	}

	void Lower(){
		// lower the platform
		if (transform.localPosition.y > yMin)
		transform.position -= new Vector3(0,Time.deltaTime * speed,0);
	}

}
