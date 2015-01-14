using UnityEngine;
using System.Collections;

public class indicator : MonoBehaviour {

	public GameObject triggerObject;
	private bool trigger;
	private Light light;

	// Use this for initialization
	void Start () {
		light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		if (triggerObject.GetComponent<Keyhole>().trigger) light.color = new Color(0, 1.0f,0);
		else light.color = new Color(1.0f, 0,0);
	}
}