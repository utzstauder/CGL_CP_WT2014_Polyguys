using UnityEngine;
using System.Collections;

public class jumpPad : MonoBehaviour {
	
	private Rigidbody2D jumpTarget;
	private Vector2 jumpForce;

	private Rigidbody2D first;
	private Rigidbody2D second;

	private Vector2 firstVelocity;
	private Vector2 secondVelocity;

	private bool firstOnCollider;
	private bool secondOnCollider;

	[SerializeField]
	private float jumpScale = 100f;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){
		if (first) firstVelocity = first.velocity;
		if (second) secondVelocity = second.velocity;
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player"){
			if (!first && first != other.GetComponent<Rigidbody2D>()) first = other.GetComponent<Rigidbody2D>();
			else if (!second && second != other.GetComponent<Rigidbody2D>()) second = other.GetComponent<Rigidbody2D>();
		}
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Player"){
			if (other.gameObject.GetComponent<Rigidbody2D>() == first) firstOnCollider = true;
			else if (other.gameObject.GetComponent<Rigidbody2D>() == second) secondOnCollider = true;

			if (firstOnCollider && secondOnCollider){
				Debug.Log ("Jump! " + jumpForce);
				if (other.gameObject.GetComponent<Rigidbody2D>() == first) second.AddForce(-firstVelocity* other.gameObject.GetComponent<Rigidbody2D>().mass * jumpScale);
				else if (other.gameObject.GetComponent<Rigidbody2D>() == second) first.AddForce(-secondVelocity* other.gameObject.GetComponent<Rigidbody2D>().mass * jumpScale);
				jumpForce = Vector2.zero;
			}
		}
	}

	void OnCollisionExit2D(Collision2D other){
		if (other.gameObject.tag == "Player"){
			if (other.gameObject.GetComponent<Rigidbody2D>() == first) firstOnCollider = false;
			else if (other.gameObject.GetComponent<Rigidbody2D>() == second) secondOnCollider = false;
		}
	}
}
