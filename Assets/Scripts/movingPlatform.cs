using UnityEngine;
using System.Collections;

public class movingPlatform : MonoBehaviour {

	[SerializeField]
	private Vector2 target;

	// Use this for initialization
	void Start () {
		target = Vector2.zero;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FixedUpdate(){
		GetComponent<Rigidbody2D>().MovePosition(target);
	}

	void OnCollisionStay2D(Collision2D other){
		if (other.gameObject.tag == "Player"){
			//other.gameObject.rigidbody2D.MovePosition(target);
			other.gameObject.GetComponent<Rigidbody2D>().AddForce(this.GetComponent<Rigidbody2D>().velocity*100f);
		}
	}

}
