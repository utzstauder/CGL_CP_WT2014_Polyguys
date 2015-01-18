using UnityEngine;
using System.Collections;

public class LightScript : MonoBehaviour {
	
	private Light light;

	[SerializeField]
	private float baseIntensity;
	[SerializeField]
	private float wiggleSpeed;
	[SerializeField]
	private float wiggleAmount;

	// Use this for initialization
	void Start () {
		light = GetComponent<Light>();
	}
	
	// Update is called once per frame
	void Update () {
		light.intensity = baseIntensity + wiggleAmount * Mathf.Sin(Time.time * wiggleSpeed);
	}
}
