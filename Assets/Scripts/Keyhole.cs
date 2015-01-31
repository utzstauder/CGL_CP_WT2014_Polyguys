using UnityEngine;
using System.Collections;

public class Keyhole : MonoBehaviour {

	public bool keyhole;
	private Light light;

	public bool isTriangleKeyhole;
	public bool isSquareKeyhole;

	public bool switchObject;

	public bool playersInsideTriggerZone;	
	private bool player1InTrigger;
	private bool player2InTrigger;

	public bool trigger;
	
	private Animator anim;

	// Use this for initialization
	void Start () {
		if (switchObject) anim = transform.Find ("switchMesh").GetComponent<Animator>();
//		if (keyhole) light = transform.FindChild ("keyholeLightForeground").GetComponent<Light>();

		DeactivateTrigger();
	}
	
	// Update is called once per frame
	void Update () {

	}

	void FixedUpdate(){
		if (playersInsideTriggerZone){
			if (player1InTrigger && player2InTrigger) ActivateTrigger();
			else DeactivateTrigger();
		}
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

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Projectile" && switchObject) HitSwitch();

	}

	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "Player" && keyhole){
			if (other.gameObject.GetComponent<PlatformerCharacter2D>().currentVertices == 3) ActivateTrigger();
			else DeactivateTrigger();
		}
		if (other.gameObject.tag == "Player"){
			switch (other.gameObject.name){
			case "Player1": player1InTrigger = true;
				break;
			case "Player2": player2InTrigger = true;
				break;
			default:
				break;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Player"){
			switch (other.gameObject.name){
			case "Player1": player1InTrigger = false;
				break;
			case "Player2": player2InTrigger = false;
				break;
			default:
				break;
			}
		}
	}

	void ActivateTrigger(){
		if (isTriangleKeyhole || isSquareKeyhole){
			GetComponent<SpriteRenderer>().color = Color.green;
//			this.light.color = Color.green;
		}
		trigger = true;
	}

	void DeactivateTrigger(){
		if (isTriangleKeyhole || isSquareKeyhole){
			GetComponent<SpriteRenderer>().color = Color.red;
//			this.light.color = Color.red;
		}
		trigger = false;
	}

	void HitSwitch(){
		trigger = !trigger;
		anim.SetBool("trigger", trigger);
	}
}
