using UnityEngine;
using System.Collections;

public class Keyhole : MonoBehaviour {

	public bool keyhole;

	public bool isTriangleKeyhole;
	public bool isSquareKeyhole;

	public bool switchObject;

	public bool trigger;

	// Use this for initialization
	void Start () {
		trigger = false;
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate(){

	}

	void OnCollisionStay2D(Collision2D other){
		if (other.gameObject.tag == "Player" && keyhole){
			switch(other.gameObject.GetComponent<PlatformerCharacter2D>().currentVertices){
			case 3:
				if (isTriangleKeyhole) ActivateTrigger();
				else if (isSquareKeyhole) DeactivateTrigger();
				break;
			case 4:
				if (isSquareKeyhole) ActivateTrigger();
				else if (isTriangleKeyhole) DeactivateTrigger();
				break;
			default:
				DeactivateTrigger();
				break;
			}
		}
	}

	void OnCollisionExit2D(Collision2D other){
		DeactivateTrigger();
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "Player" && keyhole){
			if (other.gameObject.GetComponent<PlatformerCharacter2D>().currentVertices == 3) ActivateTrigger();
			else DeactivateTrigger();
		}
		if (other.gameObject.tag == "Projectile" && switchObject) SwitchTrigger();
	}

	void ActivateTrigger(){
		if (isTriangleKeyhole || isSquareKeyhole){
			GetComponent<SpriteRenderer>().color = Color.green;
		}
		trigger = true;
	}

	void DeactivateTrigger(){
		if (isTriangleKeyhole || isSquareKeyhole){
			GetComponent<SpriteRenderer>().color = Color.red;
		}
		trigger = false;
	}

	void SwitchTrigger(){
		trigger = !trigger;
	}
}
