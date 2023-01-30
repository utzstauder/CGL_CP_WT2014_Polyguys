using UnityEngine;
using System.Collections;

public class BossScript : MonoBehaviour 
{
	#region variables
	private AudioSource audioSource;
	private AudioClip[] audioClipsChangeShape;
	private AudioClip[] audioClipsJump;
	private AudioClip audioClipDeath;

	//---Polygon variables

	public static int minVertices = 3;
	public static int maxVertices = 8;

	public int currentVertices;
	
	private float[] presetMass = {	0.43f,
									1.00f,
									1.72f,
									2.60f,
									3.63f,
									4.83f};

	private PolygonCollider2D[] polygonCollider2D;
	private BoxCollider2D boxCollider2D;
	private GameObject[] particleObject;

	public Transform mesh3;
	public Transform mesh4;
	public Transform mesh5;
	public Transform mesh6;
	public Transform mesh7;
	public Transform mesh8;

	private Transform[] meshes;
	
	[SerializeField]
	private ParticleSystem particleEffect;
		
	//---Polygon variables


	#region boss specific variables
	public int phase = 1;

	[SerializeField]
	private Animator bossPlatform;
	private Light light;
	[SerializeField]
	private float lightFactor;
	[SerializeField]
	private float cooldownTime;
	public bool inCooldown = false;

	[SerializeField]
	private GameObject platform1;
	[SerializeField]
	private GameObject platform2;
	[SerializeField]
	private GameObject platform3;
	[SerializeField]
	private GameObject platform4;

	private GameObject[] platforms;

	#endregion


    void Awake()
	{
		audioSource = GetComponent<AudioSource>();

		// Getting all colliders
		polygonCollider2D = GetComponents<PolygonCollider2D>();
		boxCollider2D = GetComponent<BoxCollider2D>();

		// Getting all meshes
		meshes = new Transform[6]{mesh3, mesh4, mesh5, mesh6, mesh7, mesh8};

		// Setting the default vertex count
		currentVertices = minVertices;

		// Loading the audio resources
		audioClipsChangeShape = new AudioClip[] { Resources.Load("Sounds/shape_sounds_1/a5", typeof(AudioClip)) as AudioClip,
			Resources.Load("Sounds/shape_sounds_1/f#5", typeof(AudioClip)) as AudioClip,
			Resources.Load("Sounds/shape_sounds_1/d5", typeof(AudioClip)) as AudioClip,
			Resources.Load("Sounds/shape_sounds_1/b4", typeof(AudioClip)) as AudioClip,
			Resources.Load("Sounds/shape_sounds_1/g4", typeof(AudioClip)) as AudioClip,
			Resources.Load("Sounds/shape_sounds_1/e4", typeof(AudioClip)) as AudioClip
		};

		audioClipsJump = new AudioClip[] { Resources.Load("Sounds/jump_sounds_1/d6", typeof(AudioClip)) as AudioClip,
			Resources.Load("Sounds/jump_sounds_1/b5", typeof(AudioClip)) as AudioClip,
			Resources.Load("Sounds/jump_sounds_1/a5", typeof(AudioClip)) as AudioClip,
			Resources.Load("Sounds/jump_sounds_1/g5", typeof(AudioClip)) as AudioClip,
			Resources.Load("Sounds/jump_sounds_1/e5", typeof(AudioClip)) as AudioClip,
			Resources.Load("Sounds/jump_sounds_1/d5", typeof(AudioClip)) as AudioClip
		};

		audioClipDeath = Resources.Load ("Sounds/death_01", typeof(AudioClip)) as AudioClip;
		light = GetComponent<Light>();

		platforms = new GameObject[4]{platform1, platform2, platform3, platform4};

		Init (3);
	}

	#endregion

	#region methods/functions

	public void Init(int vertices){
		// Initiate the player shape
		ChangeShape(vertices);
	}

	void Update(){

	}

	void FixedUpdate()
	{

	}
	
	private void ActivateCollider(){
		switch (currentVertices){
		case 3: polygonCollider2D[0].enabled = true; break;
		case 4: boxCollider2D.enabled = true; break;
		case 5: polygonCollider2D[1].enabled = true; break;
		case 6: polygonCollider2D[2].enabled = true; break;
		case 7: polygonCollider2D[3].enabled = true; break;
		case 8: polygonCollider2D[4].enabled = true; break;
		default: break;
		}
	}

	private void DeactivateCollider(){
		switch (currentVertices){
		case 3: polygonCollider2D[0].enabled = false; break;
		case 4: boxCollider2D.enabled = false; break;
		case 5: polygonCollider2D[1].enabled = false; break;
		case 6: polygonCollider2D[2].enabled = false; break;
		case 7: polygonCollider2D[3].enabled = false; break;
		case 8: polygonCollider2D[4].enabled = false; break;
		default: break;
		}
	}

	private void ActivateMesh(int vertices){
		if (vertices >= 3 && vertices <= 8)
		meshes[vertices-3].gameObject.SetActive(true);
	}

	private void DeactivateAllMeshes(){
		for (int i = 0; i < meshes.Length; i++){
			meshes[i].gameObject.SetActive(false);
		}
	}

	public void ChangeShape(int targetVertices){

		// Evaluate parameter
		if (targetVertices > maxVertices) targetVertices = minVertices;

		// Update all polygon specific variables
//		spriteRenderer.sprite = presetSprites[targetVertices-3];
		DeactivateAllMeshes();
		ActivateMesh(targetVertices);
		GetComponent<Rigidbody2D>().mass = presetMass[targetVertices-3];

		// Deactivate all colliders first
		for (int i = 0; i < polygonCollider2D.Length; i++){
			polygonCollider2D[i].enabled = false;
		}
		boxCollider2D.enabled = false;

		// Then Activate corresponding collider
		switch (targetVertices){
			case 3: polygonCollider2D[0].enabled = true; break;
			case 4: boxCollider2D.enabled = true; break;
			case 5: polygonCollider2D[1].enabled = true; break;
			case 6: polygonCollider2D[2].enabled = true; break;
			case 7: polygonCollider2D[3].enabled = true; break;
			case 8: polygonCollider2D[4].enabled = true; break;
			default: break;
		}

		// Set the new vertex count
		currentVertices = targetVertices;
	}

	void Hit(){
		Debug.Log ("Boss hit");

		inCooldown = true;
		DisablePlatforms();
		ChangeShape(currentVertices+1);
		NextPhase();
		StartCoroutine(Cooldown());
	}

	void NextPhase(){
		phase += 1;
		light.range = 4 + phase*2;
		light.intensity = phase;
		bossPlatform.SetInteger("phase", phase);
	}

	void EnablePlatforms(){
		foreach (GameObject platform in platforms) platform.SetActive(true);
	}

	void DisablePlatforms(){
		foreach (GameObject platform in platforms) platform.SetActive(false);
	}

	void Die(){
		ParticleSystem littleExplosion = Instantiate(particleEffect, this.transform.position, Quaternion.identity) as ParticleSystem;
		littleExplosion.startSize *= this.transform.localScale.x;
		littleExplosion.startSpeed *= this.transform.localScale.x;
		littleExplosion.GetComponent<autoDestroy>().triggerDestroy(littleExplosion.startLifetime);
		Destroy (this.gameObject);
	}

	#endregion
	
	#region triggers

	void OnTriggerEnter2D(Collider2D other){
		switch (other.gameObject.tag){
		case "Deadly":
			Die ();
			break;
		case "Projectile":
			if (!inCooldown){
				Hit();
				other.gameObject.GetComponent<ProjectileBehaviour>().particleSystem.GetComponent<autoDestroy>().triggerDestroy(other.gameObject.GetComponent<ProjectileBehaviour>().particleSystem.startLifetime);
				Destroy (other.gameObject);
			}
			break;
		default: break;
		}
	}

	#endregion

	#region coroutines
	private IEnumerator Cooldown(){
		float intensity = light.intensity;
		for (float t = 0; t < cooldownTime; t+=Time.deltaTime/cooldownTime){
			if (t < cooldownTime/2) light.intensity = intensity + (t/cooldownTime * lightFactor);
				else light.intensity = intensity + ((1f - t/cooldownTime) * lightFactor);
			yield return new WaitForEndOfFrame();
		}
		EnablePlatforms();
		inCooldown = false;
	}
	#endregion	 
}
