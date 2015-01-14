using UnityEngine;

public class PlatformerCharacter2D : MonoBehaviour 
{
	[SerializeField] float maxSpeed = 8f;				// The fastest the player can travel in the x axis.
	float jumpForce;									// Amount of force added when the player jumps.	

	public bool aircontrol;								// Does the player move in the air?

	[SerializeField] LayerMask whatIsGround;			// A mask determining what is ground to the character
	[SerializeField] LayerMask whatIsOtherPlayer;

	public GameObject otherPlayer;						// A reference to the other player

	[SerializeField] Transform projectilePrefab;		// The Vertex projectile

	Transform groundCheck;								// A position marking where to check if the player is grounded.
	float groundedRadius = .4f;							// Radius of the overlap circle to determine if grounded
	bool grounded = false;								// Whether or not the player is grounded.
	bool otherPlayerGrounded = false;
	bool otherPlayerOnTop = false;
	Animator anim;										// Reference to the player's animator component.
	SpriteRenderer spriteRenderer;						// Reference to the player's sprite animator component.
	

	//---Player variables

	public int playerID;

	private Light[] playerLights;

	private Vector3[] playerColor = {new Vector3(0.66f,0.66f,1.0f), new Vector3(1.0f,0.66f,0.66f)};

	private AudioSource audioSource;
	private AudioClip[] presetAudioClips;
	
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
	
	private float[] presetGroundedRadius = { 	0.45f,
												0.6f,
												0.8f,
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

	private PolygonCollider2D[] polygonCollider2D;
	private BoxCollider2D boxCollider2D;
	private GameObject[] particleObject;

	public GameObject particles3;
	public GameObject particles4;
	public GameObject particles5;
	public GameObject particles6;
	public GameObject particles7;
	public GameObject particles8;
		
	//---Polygon variables

    void Awake()
	{
		// Setting up references.
		anim = GetComponent<Animator>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		playerLights = GetComponentsInChildren<Light>();
		audioSource = GetComponent<AudioSource>();

		// Getting all colliders
		polygonCollider2D = GetComponents<PolygonCollider2D>();
		boxCollider2D = GetComponent<BoxCollider2D>();

		// Getting all particle emitters
		particleObject = new GameObject[]{particles3, particles4, particles5, particles6, particles7, particles8};

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
		presetAudioClips = new AudioClip[] { Resources.Load("Sounds/projectile_3", typeof(AudioClip)) as AudioClip,
			Resources.Load("Sounds/projectile_4", typeof(AudioClip)) as AudioClip,
			Resources.Load("Sounds/projectile_5", typeof(AudioClip)) as AudioClip,
			Resources.Load("Sounds/projectile_6", typeof(AudioClip)) as AudioClip,
			Resources.Load("Sounds/projectile_7", typeof(AudioClip)) as AudioClip,
			Resources.Load("Sounds/projectile_8", typeof(AudioClip)) as AudioClip
		};
	}

	public void Init(){
		// Initiate the player shape
		ChangeShape(minVertices);
		
		// Initiate the player color and layer masks
		ChangeColor(playerColor[playerID-1]);
		
		switch (playerID){
		case 1: whatIsOtherPlayer = LayerMask.GetMask("Player2");
			ChangeLayerMask("Player1");
			break;
		case 2: whatIsOtherPlayer = LayerMask.GetMask("Player1");
			ChangeLayerMask("Player2");
			break;
		default: break;
		}
	}

	void Update(){

	}

	void FixedUpdate()
	{
		// The player is grounded if a circlecast to the groundcheck position hits anything designated as ground
		//grounded = Physics2D.OverlapCircle(transform.position, groundedRadius, whatIsGround);
		/*if (Physics2D.OverlapCircle(transform.position, groundedRadius, whatIsGround)
		    && otherPlayerOnTop) grounded = false;
		else if (Physics2D.OverlapCircle(transform.position, groundedRadius, whatIsGround)
		         || Physics2D.OverlapCircle(transform.position, groundedRadius, whatIsOtherPlayer) && otherPlayer.GetComponent<PlatformerCharacter2D>().grounded) grounded = true;
		    else grounded = false;*/
		otherPlayerOnTop = (Physics2D.OverlapCircle(transform.position, groundedRadius, whatIsOtherPlayer)
		                    && (otherPlayer.transform.position.y > this.transform.position.y));

		grounded = Physics2D.OverlapCircle(transform.position, groundedRadius, whatIsGround)
			    || ((Physics2D.OverlapCircle(transform.position, groundedRadius, whatIsOtherPlayer) && otherPlayer.GetComponent<PlatformerCharacter2D>().grounded))
				&& !otherPlayerOnTop
				&& (Mathf.Abs(rigidbody2D.velocity.y) < 0.1f);

		anim.SetBool("Ground", grounded);

		// Set the vertical animation
		anim.SetFloat("vSpeed", rigidbody2D.velocity.y);
	}


	public void Move(float move, bool jump)
	{

		//only control the player if grounded or airControl is turned on
		if(grounded || aircontrol)
		{
			// The Speed animator parameter is set to the absolute value of the horizontal input.
			anim.SetFloat("Speed", Mathf.Abs(move));

			// Move the character
			rigidbody2D.velocity = new Vector2(move * maxSpeed, rigidbody2D.velocity.y);
		}

        // If the player should jump...
        if (grounded && jump) {
            // Add a vertical force to the player.
            anim.SetBool("Ground", false);
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
        }
	}

	public void Shoot(){
		if (currentVertices > 3 && otherPlayer != null){
			//Debug.Log ("Shoot!");
			Transform projectile = null;

			// create a network instance of the projectile
			projectile = Instantiate(projectilePrefab, transform.position, Quaternion.identity) as Transform;

			// set the shape of the projectile
			projectile.GetComponent<ProjectileBehaviour>().SetVertices(currentVertices);

			// set source, target and direction
			projectile.GetComponent<ProjectileBehaviour>().source = this.gameObject;
			projectile.GetComponent<ProjectileBehaviour>().target = otherPlayer.gameObject;

			ChangeShape (currentVertices-1);
		} //else Debug.Log (currentVertices);
	}
	
	public void ChangeShape(int targetVertices){

		// Evaluate parameter
		if (targetVertices > maxVertices) targetVertices = minVertices;

		// Update all polygon specific variables
		spriteRenderer.sprite = presetSprites[targetVertices-3];
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

		// Play audio
		if (targetVertices < currentVertices) audioSource.PlayOneShot(presetAudioClips[currentVertices-3],1.0f);
		else audioSource.PlayOneShot(presetAudioClips[targetVertices-3],1.0f);

		// TODO: check
		// Deactiveate all particle emitters first
		for (int i = 0; i < particleObject.Length; i++){
			if (particleObject[i] != null) particleObject[i].SetActive(false);
			else Debug.Log(i + " is null");
		}
		// Activate corresponding particle emitter
		if (particleObject[targetVertices-3] != null) particleObject[targetVertices-3].SetActive(true);

		// Set the new vertex count
		currentVertices = targetVertices;
	}
	
	public void ChangeColor(Vector3 color){
		spriteRenderer.color = new Color(color.x, color.y, color.z, 1.0f);

		for (int i = 0; i < playerLights.Length; i++)
			playerLights[i].color = new Color(color.x, color.y, color.z, 1.0f);

		foreach (GameObject particle in particleObject){
			particle.GetComponent<ParticleSystem>().startColor = new Color(color.x, color.y, color.z, 1.0f);
		}
	}
	
	public void ChangeLayerMask(string layerName){
		gameObject.layer = LayerMask.NameToLayer(layerName);
	}

	void OnCollisionEnter2D(Collision2D other){
		if (other.gameObject.tag == "Player"){
			if (other.transform.position.y > this.transform.position.y) otherPlayerOnTop = true;
			if (other.gameObject.GetComponent<PlatformerCharacter2D>().grounded) otherPlayerGrounded = true;
			else otherPlayerGrounded = false;
		}
	}

	void OnCollisionExit2D(Collision2D other){
		if (other.gameObject.tag == "Player"){
			otherPlayerGrounded = false;
			otherPlayerOnTop = false;
		}
	}
}
