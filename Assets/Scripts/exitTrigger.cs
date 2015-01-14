using UnityEngine;
using System.Collections;

public class exitTrigger : MonoBehaviour {

	private BoxCollider2D triggerArea;

	public int playersInTrigger;


	// Use this for initialization
	void Start () {
		triggerArea = GetComponent<BoxCollider2D>();
		playersInTrigger = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player"){
			playersInTrigger++;
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Player"){
			playersInTrigger--;
		}
	}
}
