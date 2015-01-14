using UnityEngine;
using System.Collections;

public class VertexPickup : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	}

	void OnTriggerEnter2D (Collider2D other){
		if (other.gameObject.tag == "Player" && other.gameObject.GetComponent<PlatformerCharacter2D>().currentVertices < 8){
			other.GetComponent<PlatformerCharacter2D>().ChangeShape(other.GetComponent<PlatformerCharacter2D>().currentVertices+1);
			Destroy (this.gameObject);
		}
	}
}
