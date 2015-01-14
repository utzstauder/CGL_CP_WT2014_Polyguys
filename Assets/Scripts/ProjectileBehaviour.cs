using UnityEngine;
using System.Collections;

public class ProjectileBehaviour : MonoBehaviour {

	public GameObject source;
	public GameObject target;

	private Vector3 direction = Vector3.zero;

	[Range(1.0f,100.0f)]
	public float speed = 25.0f;

	private Sprite[] presetSprites;
	private SpriteRenderer spriteRenderer; 

	private ParticleSystem particleSystem;

	public int currentVertices;

	// Use this for initialization
	void Awake () {
		//Debug.Log ("projectile instantiated!");

		// Set references
		spriteRenderer = GetComponent<SpriteRenderer>();

		// Loading the sprite and audio resources
		presetSprites = new Sprite[] {	Resources.Load("Sprites/polygons/3_triangle", typeof(Sprite)) as Sprite,
			Resources.Load("Sprites/polygons/4_square", typeof(Sprite)) as Sprite,
			Resources.Load("Sprites/polygons/5_pentagon", typeof(Sprite)) as Sprite,
			Resources.Load("Sprites/polygons/6_hexagon", typeof(Sprite)) as Sprite,
			Resources.Load("Sprites/polygons/7_septagon", typeof(Sprite)) as Sprite,
			Resources.Load("Sprites/polygons/8_octagon", typeof(Sprite)) as Sprite
		};

		particleSystem = GetComponent<ParticleSystem>();
	}

	void Start(){

	}
	
	// Update is called once per frame
	void Update(){

	}

	void FixedUpdate(){
		if (source != null && target != null){
			direction = (target.transform.position - transform.position).normalized;
		}
		rigidbody2D.velocity = direction * speed * 10.0f * Time.deltaTime;
	}

	public void SetVertices(int targetVertices){
		spriteRenderer.sprite = presetSprites[targetVertices-3];
		particleSystem.startSize *= targetVertices*targetVertices;
		currentVertices = targetVertices;
	}

	void OnTriggerEnter2D (Collider2D other){
		if (other.gameObject == target.gameObject){
			//Debug.Log ("Collision with " + other.gameObject);
			other.GetComponent<PlatformerCharacter2D>().ChangeShape(other.GetComponent<PlatformerCharacter2D>().currentVertices+1);
			Destroy (this.gameObject);
		}
	}
}
