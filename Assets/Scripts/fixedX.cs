using UnityEngine;
using System.Collections;

public class fixedX : MonoBehaviour {

	float xLocalFixed;

	// Use this for initialization
	void Start () {
		xLocalFixed = transform.localPosition.x;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		transform.localPosition = new Vector3(xLocalFixed, this.transform.localPosition.y, this.transform.localPosition.z);
	}
}
