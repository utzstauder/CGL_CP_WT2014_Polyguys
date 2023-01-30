using UnityEngine;
using System.Collections;

public class ProjectileBehaviour : MonoBehaviour {
	[HideInInspector]
	public GameObject source;
	[HideInInspector]
	public GameObject target;

	private Vector3 direction = Vector3.zero;
	private float progression;

	[Range(1.0f,100.0f)]
	public float speed = 25.0f;

	private Sprite[] presetSprites;
	[SerializeField]
	private SpriteRenderer spriteRenderer; 

	[SerializeField]
	private ParticleSystem projectileParticleSystem;
	[SerializeField]
	private ParticleSystem particleEffect;

	[HideInInspector]
	public ParticleSystem particleSystem;

	[HideInInspector]
	public int currentVertices;

	[SerializeField]
	private Light light1;
	[SerializeField]
	private Light light2;
	[SerializeField]
	private Light light3;
	[SerializeField]
	private Light light4;
	[SerializeField]
	private float maxIntensity;
	[SerializeField]
	private float maxRange;

	// Use this for initialization
	void Awake () {
		//Debug.Log ("projectile instantiated!");

		// Set references
//		spriteRenderer = GetComponent<SpriteRenderer>();

		// Loading the sprite and audio resources
		presetSprites = new Sprite[] {	Resources.Load("Sprites/polygons_fill/3_triangle", typeof(Sprite)) as Sprite,
			Resources.Load("Sprites/polygons_fill/4_square", typeof(Sprite)) as Sprite,
			Resources.Load("Sprites/polygons_fill/5_pentagon", typeof(Sprite)) as Sprite,
			Resources.Load("Sprites/polygons_fill/6_hexagon", typeof(Sprite)) as Sprite,
			Resources.Load("Sprites/polygons_fill/7_septagon", typeof(Sprite)) as Sprite,
			Resources.Load("Sprites/polygons_fill/8_octagon", typeof(Sprite)) as Sprite
		};

		//particleSystem = GetComponent<ParticleSystem>();
	}

	void Start(){
		light1.intensity = 0;
		light2.intensity = 0;
		light3.intensity = 0;
		light4.intensity = 0;
	}
	
	// Update is called once per frame
	void Update(){

	}

	void FixedUpdate(){
		if (source != null && target != null){
			direction = (target.transform.position - transform.position).normalized;
			progression = 1f - (target.transform.position - this.transform.position).magnitude/(target.transform.position - source.transform.position).magnitude;
		}
		GetComponent<Rigidbody2D>().velocity = direction * speed * 10.0f * Time.deltaTime;
		particleSystem.transform.position = new Vector3(this.transform.position.x, this.transform.position.y, -11f);

		if (source != null && target != null){
//			progression = 1f - (target.transform.position - this.transform.position).magnitude/(target.transform.position - source.transform.position).magnitude;
			progression = 1f - Vector3.Distance(target.transform.position, this.transform.position) / Vector3.Distance(target.transform.position, source.transform.position);
			//			Debug.Log (progression);
			if (progression <= .5f){
				light1.intensity = maxIntensity * progression;
				light2.intensity = maxIntensity * progression;
				light3.intensity = maxIntensity * progression;
				light4.intensity = maxIntensity * progression;
			}else{
				light1.intensity = maxIntensity * (1f-progression);
				light2.intensity = maxIntensity * (1f-progression);
				light3.intensity = maxIntensity * (1f-progression);
				light4.intensity = maxIntensity * (1f-progression);
			}
		}
	}

	public void Init(int targetVertices, Color color){
		spriteRenderer.sprite = presetSprites[targetVertices-3];
		spriteRenderer.color = color * new Color(1f, 1f, 1f, .5f);

		particleSystem = Instantiate(projectileParticleSystem, this.transform.position, Quaternion.identity) as ParticleSystem;
		particleSystem.startSize *= targetVertices*targetVertices;
		particleSystem.startColor = color;
//
		light1.color = color;
		light2.color = color;
		light3.color = color;
		light4.color = color;
		
		currentVertices = targetVertices;
	}

	private void Reflect(){
		GameObject tmp = source;
		source = target;
		target = tmp;
		tmp = null;
	}

	private void ParticleBlast(){
		ParticleSystem particleEffectObject = Instantiate(particleEffect, this.transform.position, Quaternion.identity) as ParticleSystem;
		particleEffectObject.startColor = particleSystem.startColor;
		particleEffectObject.GetComponent<autoDestroy>().triggerDestroy(particleEffect.startLifetime);
	}

	void OnTriggerEnter2D (Collider2D other){
		if (other.gameObject == target.gameObject){
			//Debug.Log ("Collision with " + other.gameObject);
			other.GetComponent<PlatformerCharacter2D>().ChangeShape(other.GetComponent<PlatformerCharacter2D>().currentVertices+1);
			particleSystem.GetComponent<autoDestroy>().triggerDestroy(particleSystem.startLifetime);
			Destroy (this.gameObject);
		} else if (other.gameObject.tag == "Impenetrable"){
			Reflect();
			ParticleBlast ();

		} else if (other.gameObject.tag == "Boss"){
			// allways reflect, everything else is done in the bosses script	
			Reflect ();
			ParticleBlast ();
		}
	}
}
