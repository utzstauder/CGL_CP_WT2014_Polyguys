using UnityEngine;
using System.Collections;

public class checkpoint : MonoBehaviour {

	[SerializeField]
	private bool isActivated;
	[SerializeField]
	private bool isStartPoint;

	private Light indicatorLight;
	private SpriteRenderer spriteRenderer;

	// Use this for initialization
	void Start () {
		indicatorLight = GetComponent<Light>();
		spriteRenderer = GetComponent<SpriteRenderer>();

		setLight();
		setSpriteRenderer();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Player"){
			isActivated = true;
			setLight();
			setSpriteRenderer();
		}
	}

	public bool Activated(){
		return this.isActivated;
	}

	private void setLight(){
//		if (!isStartPoint) indicatorLight.enabled = true;

		if (isActivated) indicatorLight.color = Color.green;
		else indicatorLight.color = Color.red;
	}

	private void setSpriteRenderer(){
//		if (!isStartPoint) spriteRenderer.enabled = true;

		if (isActivated) spriteRenderer.color = Color.green;
		else spriteRenderer.color = Color.red;
	}
}
