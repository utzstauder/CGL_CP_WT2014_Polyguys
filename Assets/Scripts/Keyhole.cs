using UnityEngine;
using System.Collections;

public class Keyhole : MonoBehaviour {

	public bool trigger;

	// Use this for initialization
	void Start () {
		trigger = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player") trigger = true;
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Player") trigger = false;
	}
}
