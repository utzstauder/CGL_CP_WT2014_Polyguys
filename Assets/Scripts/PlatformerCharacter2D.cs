using UnityEngine;
using System.Collections;

public class PlatformerCharacter2D : MonoBehaviour 
{
	#region variables
	public bool playerHasControl = false;				// if true, the player can control his character

	[SerializeField] float maxSpeed = 8f;				// The fastest the player can travel in the x axis.
	float jumpForce;									// Amount of force added when the player jumps.	

	public bool aircontrol;								// Does the player move in the air?
	[Range(0,3)]
	public int airJumps;
	private int airJumpsTmp;

	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character
	[SerializeField] LayerMask whatIsOtherPlayer;

	public GameObject otherPlayer;						// A reference to the other player

	[SerializeField] Transform projectilePrefab;		// The Vertex projectile

	Transform groundCheck;								// A position marking where to check if the player is grounded.
	float groundedRadius = .4f;							// Radius of the overlap circle to determine if grounded
	bool grounded = false;								// Whether or not the player is grounded.
	bool canJump = false;
	bool otherPlayerGrounded = false;
	bool otherPlayerOnTop = false;
	[SerializeField]
	private float onTopThreshold = 0.1f;

//	SpriteRenderer spriteRenderer;						// Reference to the player's sprite renderer component.

	[SerializeField]
	private float respawnTime;
	private bool alive;

	public CameraFollow mainCamera;
	public CameraSystem mainCameraSystem;

	//---Player variables

	public int playerID;

	private Light[] playerLights;
	[SerializeField]
	private Light colorLight;
	[SerializeField]
	private Light colorLightForeground;
	[HideInInspector]
	public Color color;

	public bool enableParticles = false;

	private AudioSource audioSource;
	private AudioClip[] audioClipsChangeShape;
	private AudioClip[] audioClipsJump;
	private AudioClip audioClipDeath;
	
	//---Player variables


	//---Polygon variables

	public static int minVertices = 3;
	public static int maxVertices = 8;

	public int currentVertices;

	private Sprite[] presetSprites;

	private float[] presetMass = {	0.43f,
									1.00f,
									1.72f,
									2.60f,
									3.63f,
									4.83f};
	
	private float[] presetGroundedRadius = { 	0.5f,
												0.75f,
												0.9f,
												1.1f,
												1.25f,
												1.4f
	};

	private float[] presetJumpForce = {	370f,
										810f,
										1240f,
										1775f,
										2150f,
										2600f
	};

	private Color[] presetColorsP1 = {	new Color(.949f, 1f, .341f, 1f),
										new Color(1f, .663f, .337f, 1f),
										new Color(.937f, .647f, .271f, 1f),
										new Color(1f, .522f, .337f, 1f),
										new Color(1f, .431f, .341f, 1f),
										new Color(.988f, .341f, .231f, 1f)};

	private Color[] presetColorsP2 = {	new Color(.157f, .824f, .176f, 1f),
										new Color(.369f, .918f, .41f, 1f),
										new Color(.157f, .824f, .439f, 1f),
										new Color(.341f, 1f, .965f, 1f),
										new Color(.341f, .761f, 1f, 1f),
										new Color(.231f, .357f, .988f, 1f)};

	private PolygonCollider2D[] polygonCollider2D;
	private BoxCollider2D boxCollider2D;
	private GameObject[] particleObject;

	public GameObject particles3;
	public GameObject particles4;
	public GameObject particles5;
	public GameObject particles6;
	public GameObject particles7;
	public GameObject particles8;

	public Transform mesh3;
	public Transform mesh4;
	public Transform mesh5;
	public Transform mesh6;
	public Transform mesh7;
	public Transform mesh8;

	private Transform[] meshes;

	[SerializeField]
	private respawnParticleEffect deathEffect;
	[SerializeField]
	private ParticleSystem particleEffect;
		
	//---Polygon variables

    void Awake()
	{
		// keep players alive during scenes
		DontDestroyOnLoad(this.gameObject);

		// Setting up references.
//		spriteRenderer = GetComponent<SpriteRenderer>();
		playerLights = GetComponentsInChildren<Light>();
		audioSource = GetComponent<AudioSource>();

		// Getting all colliders
		polygonCollider2D = GetComponents<PolygonCollider2D>();
		boxCollider2D = GetComponent<BoxCollider2D>();

		// Getting all particle emitters
		particleObject = new GameObject[]{particles3, particles4, particles5, particles6, particles7, particles8};

		// Getting all meshes
		meshes = new Transform[6]{mesh3, mesh4, mesh5, mesh6, mesh7, mesh8};

		// Setting the default vertex count
		currentVertices = minVertices;

		// Loading the sprite resources
		presetSprites = new Sprite[] {	Resources.Load("Sprites/polygons/3_triangle", typeof(Sprite)) as Sprite,
										Resources.Load("Sprites/polygons/4_square", typeof(Sprite)) as Sprite,
										Resources.Load("Sprites/polygons/5_pentagon", typeof(Sprite)) as Sprite,
										Resources.Load("Sprites/polygons/6_hexagon", typeof(Sprite)) as Sprite,
										Resources.Load("Sprites/polygons/7_septagon", typeof(Sprite)) as Sprite,
										Resources.Load("Sprites/polygons/8_octagon", typeof(Sprite)) as Sprite
		};

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

		if (!mainCamera) mainCamera = GameObject.Find ("MainCamera").GetComponent<CameraFollow>();
		if (!mainCameraSystem) mainCameraSystem = GameObject.Find ("MainCamera").GetComponent<CameraSystem>();
	}

	#endregion

	#region methods/functions

	public void Init(int vertices){
		// Initiate the player shape
		ChangeShape(vertices);

		this.gameObject.name = "Player"+playerID;

		switch(playerID){
		case 1:
			if (mainCamera) mainCamera.player1 = this.transform;
			if (mainCameraSystem) mainCameraSystem.player1 = this.transform;
			break;
		case 2:
			if (mainCamera) mainCamera.player2 = this.transform;
			if (mainCameraSystem) mainCameraSystem.player2 = this.transform;
			break;
		}

		alive = true;
	}

	//TODO: implement controls here
	void Update(){
		// jumping
		if (alive && playerHasControl){
			if ((playerID == 1 && Input.GetButtonDown("p1Shoot"))
			    || (playerID == 2 && Input.GetButtonDown("p2Shoot")) ) Shoot();
			else if (grounded && !otherPlayerOnTop && Mathf.Abs(rigidbody2D.velocity.y) < 3.5f){
				if ((playerID == 1 && Input.GetButtonDown("p1Jump"))
				    || (playerID == 2 && Input.GetButtonDown("p2Jump")) ){
					// Add a vertical force to the player.
					rigidbody2D.AddForce(new Vector2(0f, jumpForce));
					// Play audio
					audioSource.PlayOneShot(audioClipsJump[currentVertices-3],0.33f);
				} 
			}
		}
	}

	void FixedUpdate()
	{
		otherPlayerGrounded = otherPlayer.GetComponent<PlatformerCharacter2D>().grounded;

		otherPlayerOnTop = (Physics2D.OverlapCircle(transform.position, groundedRadius, whatIsOtherPlayer)
		                    && (otherPlayer.transform.position.y - onTopThreshold > this.transform.position.y));

		if (alive && playerHasControl){
			// move the player
			float move = 0;
			if (playerID == 1) move = Input.GetAxis ("p1Horizontal");
			if (playerID == 2) move = Input.GetAxis ("p2Horizontal");
			rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
		}
		grounded = false;
	}

	public void Move(float move, bool jump)
	{
		if (alive && playerHasControl){
			//only control the player if grounded or airControl is turned on
			if(grounded || aircontrol)
			{
				// Move the character
				rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
			}

			//TODO: old jump
//	        // If the player should jump...
//	        if (grounded && !otherPlayerOnTop && jump) {
//	            // Add a vertical force to the player.
//	            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
//				// Play audio
//				audioSource.PlayOneShot(audioClipsJump[currentVertices-3],0.33f);
//	        }
		}
	}

	public void Shoot(){
		if (currentVertices > 3 && otherPlayer != null && alive && otherPlayer.GetComponent<PlatformerCharacter2D>().alive && playerHasControl){
			//Debug.Log ("Shoot!");
			Transform projectile = null;

			// create a network instance of the projectile
			projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as Transform;

			// set the shape of the projectile
			projectile.GetComponent<ProjectileBehaviour>().Init(currentVertices, color);

			// set source, target and direction
			projectile.GetComponent<ProjectileBehaviour>().source = this.gameObject;
			projectile.GetComponent<ProjectileBehaviour>().target = otherPlayer.gameObject;

			ChangeShape (currentVertices-1);
		} //else Debug.Log (currentVertices);
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
		rigidbody2D.mass = presetMass[targetVertices-3];
		groundedRadius = presetGroundedRadius[targetVertices-3];
		jumpForce = presetJumpForce[targetVertices-3];

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

		// Play audio only if in playing state
		if (playerHasControl){
			if (targetVertices < currentVertices) audioSource.PlayOneShot(audioClipsChangeShape[targetVertices-3],1.2f);
			else audioSource.PlayOneShot(audioClipsChangeShape[targetVertices-3],1.0f);
		}

		// Deactiveate all particle emitters first
		for (int i = 0; i < particleObject.Length; i++){
			if (particleObject[i] != null) particleObject[i].SetActive(false);
			else Debug.Log(i + " is null");
		}
		// Activate corresponding particle emitter
		if (enableParticles && particleObject[targetVertices-3] != null) particleObject[targetVertices-3].SetActive(true);

		// Change color
		switch(playerID){
		case 1: ChangeColor(presetColorsP1[targetVertices-3]);
			break;
		case 2: ChangeColor(presetColorsP2[targetVertices-3]);
			break;
		default:
			break;
		}

		// Set the new vertex count
		currentVertices = targetVertices;
	}
	
	public void ChangeColor(Color color){
//		spriteRenderer.color = new Color(color.x, color.y, color.z, 1.0f);

//		for (int i = 0; i < playerLights.Length; i++)
//			playerLights[i].color = color;
		colorLight.color = color;
		colorLightForeground.color = color;

		foreach (GameObject particle in particleObject){
			particle.GetComponent<ParticleSystem>().startColor = color;
		}
		this.color = color;
	}
	
	public void ChangeLayerMask(string layerName){
		gameObject.layer = LayerMask.NameToLayer(layerName);
	}

	#endregion

	#region functions
	
	// returns the nearest transform tagges as "Spawn Point"
	private Vector3 nearestSpawnPoint(){
		GameObject[] respawnPoints = GameObject.FindGameObjectsWithTag("Spawn Point");
		Transform returnValue = null;
		foreach (GameObject spawnPoint in respawnPoints){
			if (returnValue == null || (Vector3.Distance(spawnPoint.transform.position,this.transform.position) < Vector3.Distance(returnValue.position, this.transform.position))){
				if ( (spawnPoint.GetComponent<checkpoint>().isActivatedP1 && playerID == 1) || (spawnPoint.GetComponent<checkpoint>().isActivatedP2 && playerID == 2) )
				returnValue = spawnPoint.transform;
			}
		}
		return returnValue.position;
	}

	// returns the players color
	private Color GetPlayerColor(){
		return color;
	}
	
	#endregion

	#region debug

	void OnDrawGizmos(){
		Gizmos.color = color;
		Gizmos.DrawWireSphere(this.transform.position,groundedRadius);		
	}

	#endregion

	#region collisions

	void OnCollisionStay2D(Collision2D other){
		if (other.gameObject.tag == "Player"){
			if (other.transform.position.y > this.transform.position.y) otherPlayerOnTop = true;
		}

		if (other.gameObject.layer == LayerMask.NameToLayer("Ground") || other.gameObject.layer == LayerMask.NameToLayer("NoGroundWallClip")){
			grounded = true;
		}

		if (other.gameObject.tag == "Player"){
			if (other.gameObject.GetComponent<PlatformerCharacter2D>().currentVertices != 3) grounded = true;
		}
	}

	void OnCollisionExit2D(Collision2D other){
		if (other.gameObject.tag == "Player"){
			otherPlayerGrounded = false;
			otherPlayerOnTop = false;
		}
	}

	#endregion

	#region triggers

	void OnTriggerEnter2D(Collider2D other){
		if (other.gameObject.tag == "Deadly" && alive){
//			Debug.Log("Contact with killzone");
			StartCoroutine(DieAndRespawn(other.gameObject));
		}
	}

	void OnTriggerStay2D(Collider2D other){
		if (other.gameObject.tag == "Jumpzone"){
			grounded = true;
		}
	}

	#endregion

	#region coroutines

	private IEnumerator DieAndRespawn(GameObject killObject){
		// TODO: die and move to respawn
		alive = false;
//		spriteRenderer.enabled = false;
		DeactivateAllMeshes();
		this.rigidbody2D.isKinematic = true;
		particleObject[currentVertices-3].SetActive(false);
		DeactivateCollider();
		audioSource.PlayOneShot(audioClipDeath,.66f);

		ParticleSystem littleExplosion = Instantiate(particleEffect, this.transform.position, Quaternion.identity) as ParticleSystem;
		littleExplosion.startColor = GetPlayerColor();
		littleExplosion.GetComponent<autoDestroy>().triggerDestroy(littleExplosion.startLifetime);

		respawnParticleEffect deathParticles = Instantiate(deathEffect, this.transform.position, this.transform.localRotation) as respawnParticleEffect;
		deathParticles.Init(currentVertices, GetPlayerColor());

		Vector3 deathPosition = this.transform.position;
		Vector3 respawnPoint = nearestSpawnPoint();

		for (float t = 0; t <= respawnTime; t += Time.deltaTime){
			this.transform.position = Vector3.Lerp (deathPosition, respawnPoint, t/respawnTime);
			deathParticles.transform.position = this.transform.position;
				deathParticles.Scale((.5f-t/respawnTime)*2f);
			deathParticles.Rotate(t/respawnTime);
			this.transform.Rotate(Vector3.forward * Mathf.PI * deathParticles.GetRotationCycles() * t/respawnTime);
//			Debug.Log(t + " -> " + this.transform.position);
			yield return new WaitForSeconds(Time.deltaTime);
		}

		deathParticles.GetComponent<autoDestroy>().triggerDestroy(deathParticles.GetStartLifetime());
//		spriteRenderer.enabled = true;
		ActivateMesh(currentVertices);
		if (enableParticles) particleObject[currentVertices-3].SetActive(true);
		this.rigidbody2D.isKinematic = false;
		ActivateCollider();
		alive = true;
	}

	#endregion
	 
}
