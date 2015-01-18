using UnityEngine;
using System.Collections;

public class autoDestroy : MonoBehaviour {

	[SerializeField]
	private bool external;

	[SerializeField]
	private bool useTrigger;
	[SerializeField]
	private string targetTag;

	[SerializeField]
	private bool useTime;
	[SerializeField]
	private float time;

	// Use this for initialization
	void Start () {
		if (useTime) Destroy (this.gameObject, time);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void triggerDestroy(float tmp){
		Destroy (this.gameObject, tmp);
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == targetTag){
			if (useTime) Destroy (this.gameObject, time);
			else Destroy (this.gameObject, 0);
		}
	}
}
