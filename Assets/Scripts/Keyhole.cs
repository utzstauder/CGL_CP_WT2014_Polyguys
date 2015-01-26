using UnityEngine;
using System.Collections;

public class Keyhole : MonoBehaviour {

	public bool keyhole;
	public bool switchObject;

	public bool trigger;

	// Use this for initialization
	void Start () {
		trigger = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player" && keyhole) trigger = true;
		if (other.gameObject.tag == "Projectile" && switchObject) trigger = !trigger; 
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Player" && keyhole) trigger = false;
	}
}
