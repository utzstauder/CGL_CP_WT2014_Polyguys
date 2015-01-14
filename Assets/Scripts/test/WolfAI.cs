using UnityEngine;
using System.Collections;

public class WolfAI : MonoBehaviour {

	public GameObject player;
	//Animator anim;
	Vector3 currentPos;
	Vector3 destPos;
	public float maxSpeed = 8.0f;
	public float velocity = 200f;
	public bool facingRight = false;
	public int distanceToPlayer = 8;
	public float waitBetweenAttacks = 2.0f;
	float timeLeft;
	//PlayerHandler handler;
	float currentVel;
	bool spotted = false;

	// Use this for initialization
	void Start () {
		//handler = this.GetComponent<PlayerHandler> ();
		timeLeft = 0;
		//anim = this.GetComponent<Animator> ();
	}
	
	// Update is called once per frame
	void Update () {
//		anim.SetFloat ("xSpeed", Mathf.Abs(	this.rigidbody2D.velocity.x));

		if (!spotted) {
			//anim.ResetTrigger("Attack");
			currentPos = this.transform.position;
			destPos = player.transform.position;
			currentVel = Mathf.Abs(this.rigidbody2D.velocity.x);
			timeLeft -= Time.deltaTime;
			//Debug.Log(timeLeft);
			
			if (currentPos.x < destPos.x-distanceToPlayer) {
				if (currentVel < maxSpeed) {
					this.rigidbody2D.AddForce(new Vector2(velocity,0));
				}
				if (!facingRight) {
					Debug.Log ("FLIPr");
					Flip();
				}
				//Debug.Log("go right");
			}
			else if (currentPos.x > destPos.x+distanceToPlayer) {
				if (currentVel < maxSpeed) {
					this.rigidbody2D.AddForce(new Vector2(velocity*-1,0));
				}
				if (facingRight) {
					Debug.Log ("FLIPl");
					Flip();
				}
				//Debug.Log("go left");
			}
			if (timeLeft <= 0) {
				if (currentPos.x > destPos.x - distanceToPlayer && currentPos.x < destPos.x) {
					//attack when player is right and wolf is left
					//anim.SetTrigger("Attack");
					
					//Debug.Log("ATTACK");
				}
				if (currentPos.x < destPos.x + distanceToPlayer && currentPos.x > destPos.x) {
					//attack when player is left and wolf is right
					//anim.SetTrigger("Attack");
					//Debug.Log("ATTACK");
				}
				timeLeft = waitBetweenAttacks;
			}
		}
	}

	void OnTriggerEnter2D(Collider2D other) {
		//player enters trigger
		if (other.gameObject.tag == "player")
			spotted = true;
	}
	
	void OnTriggerExit2D(Collider2D other) {
		//player leaves trigger
		if (other.gameObject.tag == "player")
			spotted = false;
	}

	void OnCollisionEnter2D(Collision2D other) {
		if (other.gameObject.tag == "player") {
			Debug.Log("PLAYER DEAD");
			//handler.Kill ();
		}
	}

	void Flip() {
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
