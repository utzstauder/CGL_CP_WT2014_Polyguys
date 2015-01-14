using UnityEngine;
using System.Collections;

public class door : MonoBehaviour {
	
	public GameObject triggerObject;
	public float speed;
	
	public float yMin;
	public float yMax;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (triggerObject.GetComponent<lever>().trigger){
			if (transform.position.y < yMax)
				// raise the platform
				transform.position += new Vector3(0,Time.deltaTime * speed,0);
		} else if (transform.position.y > yMin)
			// lower the platform
			transform.position -= new Vector3(0,Time.deltaTime * speed,0);
	}
	
}
