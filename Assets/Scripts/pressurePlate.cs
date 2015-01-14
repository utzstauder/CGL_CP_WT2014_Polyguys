using UnityEngine;
using System.Collections;

public class pressurePlate : MonoBehaviour {

	public float speed;
	public float requiredMass;

	public float yMin;
	public float yMax;

	private bool playerOnTop;

	// Use this for initialization
	void Start () {
		playerOnTop = false;
	}
	
	void FixedUpdate () {
		if (playerOnTop && transform.localPosition.y > yMin) transform.position -= new Vector3 (0, Time.deltaTime * speed,0);
		else{
			if (!playerOnTop && transform.localPosition.y < yMax) transform.position += new Vector3 (0, Time.deltaTime * speed,0);
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Player"){
			if (other.gameObject.transform.position.y > transform.position.y && other.rigidbody.mass >= requiredMass) playerOnTop = true;
		}
	}

	void OnCollisionStay2D(Collision2D other){
		if (other.gameObject.tag == "Player"){
			if (other.rigidbody.mass < requiredMass) playerOnTop = false;
		}
	}

	void OnCollisionExit2D(Collision2D other){
		if (other.gameObject.tag == "Player"){
			playerOnTop = false;
		}
	}
}
