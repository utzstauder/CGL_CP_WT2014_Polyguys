using UnityEngine;
using System.Collections;

public class exitTrigger : MonoBehaviour {

	public bool player1InTrigger = false;
	public bool player2InTrigger = false;


	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void FixedUpdate () {
		player1InTrigger = false;
		player2InTrigger = false;
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.name == "Player1"){
			player1InTrigger = true;
		}
		if (other.gameObject.name == "Player2"){
			player2InTrigger = true;
		}
	}
}
