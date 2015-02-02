using UnityEngine;
using System.Collections;

public class Keyhole : MonoBehaviour {

	public bool keyhole;
	private Light light;

	public bool isTriangleKeyhole;
	public bool isSquareKeyhole;

	public bool switchObject;

	public bool playersInsideTriggerZone;
	[Range(6, 11)]
	public int minVerticesNeeded = 6;
	private int player1InTrigger = 0;
	private int player2InTrigger = 0;

	public Color active = new Color(.102f, .604f, .079f, 1f);
	public Color inactive = new Color(.671f, .212f, .125f, 1f);

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
			if ((player1InTrigger + player2InTrigger) >= minVerticesNeeded) ActivateTrigger();
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
			case "Player1": player1InTrigger = other.gameObject.GetComponent<PlatformerCharacter2D>().currentVertices;
				break;
			case "Player2": player2InTrigger = other.gameObject.GetComponent<PlatformerCharacter2D>().currentVertices;
				break;
			default:
				break;
			}
		}
	}

	void OnTriggerExit2D(Collider2D other){
		if (other.gameObject.tag == "Player"){
			switch (other.gameObject.name){
			case "Player1": player1InTrigger = 0;
				break;
			case "Player2": player2InTrigger = 0;
				break;
			default:
				break;
			}
		}
	}

	void ActivateTrigger(){
		if (isTriangleKeyhole || isSquareKeyhole){
			GetComponent<SpriteRenderer>().color = active;
//			this.light.color = Color.green;
		}
		trigger = true;
	}

	void DeactivateTrigger(){
		if (isTriangleKeyhole || isSquareKeyhole){
			GetComponent<SpriteRenderer>().color = inactive;
//			this.light.color = Color.red;
		}
		trigger = false;
	}

	void HitSwitch(){
		trigger = !trigger;
		anim.SetBool("trigger", trigger);
	}
}
