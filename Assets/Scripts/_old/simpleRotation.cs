using UnityEngine;
using System.Collections;

public class simpleRotation : MonoBehaviour {

	public float rotationSpeed;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(0,0,Time.deltaTime * rotationSpeed, Space.Self);
	}
}
