using UnityEngine;
using System.Collections;

public class lever : MonoBehaviour {

	// true = left, false = right;
	public bool trigger;

	private bool hit;
	private Vector2 collisionAngle;

	public float angle;
	
	public GameObject handle;

	private Animator anim;

	public float speed;

	private leverTrigger triggerLeft;
	private leverTrigger triggerRight;

	// Use this for initialization
	void Start () {
		anim = handle.GetComponent<Animator>();

		anim.SetBool("trigger", trigger);
		hit = false;
	}
	

	void FixedUpdate () {

		if (hit){
			// hit from left to right
			if (trigger == true && collisionAngle.x > 0){
				trigger = !trigger;
			} else if (trigger == false && collisionAngle.x < 0){
				trigger = !trigger;
			}
			anim.SetBool("trigger", trigger);
			hit = false;
			collisionAngle = Vector2.zero;
		}
	}

	void OnTriggerEnter2D(Collider2D other){
		//Debug.Log("Collision with " + other.gameObject.name);
		if (other.gameObject.tag == "Projectile"){
			collisionAngle = other.gameObject.rigidbody2D.velocity;
			hit = true;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Projectile"){
			hit = false;
		}
	}
	
}
