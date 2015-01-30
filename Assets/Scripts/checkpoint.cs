using UnityEngine;
using System.Collections;

public class checkpoint : MonoBehaviour {

	public bool isActivatedP1;
	public bool isActivatedP2;

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
			switch (other.GetComponent<PlatformerCharacter2D>().playerID){
			case 1: isActivatedP1 = true;
				break;
			case 2: isActivatedP2 = true;
				break;
			default: break;
			}
			setLight();
			setSpriteRenderer();
		}
	}

	private void setLight(){
//		if (!isStartPoint) indicatorLight.enabled = true;

		if (isActivatedP1 || isActivatedP2) indicatorLight.color = Color.green;
		else indicatorLight.color = Color.red;
	}

	private void setSpriteRenderer(){
//		if (!isStartPoint) spriteRenderer.enabled = true;

		if (isActivatedP1 || isActivatedP2) spriteRenderer.color = Color.green;
		else spriteRenderer.color = Color.red;
	}
}
