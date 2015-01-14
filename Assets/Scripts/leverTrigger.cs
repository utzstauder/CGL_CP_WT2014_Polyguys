using UnityEngine;
using System.Collections;

public class leverTrigger : MonoBehaviour {

	[HideInInspector]
	public bool hit;

	// Use this for initialization
	void Start () {
		hit = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Projectile") hit = true;
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Projectile") hit = false;
	}
}
