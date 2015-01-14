using UnityEngine;
using System.Collections;

public class TestPlayerMovement : MonoBehaviour {
	[Range(3,8)]
	public int vertices;

	private float[] _mass;
	private Vector3[] _groundCheckPosition;

	[Range(1.0f, 500.0f)]
	public float horizontalSpeed;
	private float[] _horizontalSpeed;

	[Range(1.0f, 100.0f)]
	public float jumpForce;
	private float[] _jumpForce;

	// is the player on the ground
	private bool grounded;

	[Range(0.0f,1.0f)]
	public float groundedRadius;
	private float[] _groundedRadius;
	// A mask determining what is ground to the character
	[SerializeField] LayerMask whatIsGround;			
	
	private Transform groundCheck;

	private SpriteRenderer spriteRenderer;
	private Sprite[] sprites;

	// Use this for initialization
	void Start () {
		groundCheck = transform.FindChild("GroundCheck");
		spriteRenderer = GetComponent<SpriteRenderer>();

		sprites = new Sprite[]{	Resources.Load("Sprites/polygons_pixel/3_triangle", typeof(Sprite)) as Sprite,
								Resources.Load("Sprites/polygons_pixel/4_square", typeof(Sprite)) as Sprite
		};

		_horizontalSpeed = new float[] { 70.0f, 130.0f };
		_jumpForce = new float[] {60.0f, 70.0f};
		_groundCheckPosition = new Vector3[] {new Vector3(0,-0.14f,0), new Vector3(0,0,0)}; 
		_groundedRadius = new float[] {0.4f, 0.55f};
		_mass = new float[] {0.43f, 1.0f};
	}
	
	// Update is called once per frame
	void Update () {
		Movement();
		if (Input.GetKeyDown(KeyCode.F)){
			if (vertices == 3) ChangeShape (4);
			else if (vertices == 4) ChangeShape (3);
		}
	}

	void FixedUpdate(){
		grounded = Physics2D.OverlapCircle(groundCheck.position, groundedRadius, whatIsGround);
	}

	void Movement(){
		if (Input.GetKeyDown(KeyCode.D)){
			if (grounded){
				// move right
				rigidbody2D.AddForce(new Vector2(horizontalSpeed * vertices * 10 * Time.deltaTime, 0));
				rigidbody2D.AddTorque(-horizontalSpeed * vertices * 10 * Time.deltaTime);
			}else 
			{
				//rigidbody2D.AddForce(new Vector2(horizontalSpeed * vertices * 10 * Time.deltaTime, 0));
			}
		}
		if (Input.GetKeyDown(KeyCode.A)){
			if (grounded){
				// move left
				rigidbody2D.AddForce(new Vector2(-horizontalSpeed * vertices * 10 * Time.deltaTime, 0));
				rigidbody2D.AddTorque(horizontalSpeed * vertices * 10 * Time.deltaTime);
			}else{
				//rigidbody2D.AddForce(new Vector2(-horizontalSpeed * vertices * 10 * Time.deltaTime, 0));
			}
		}
		if (Input.GetKeyDown(KeyCode.W) && grounded){
			// jump
			rigidbody2D.AddForce(new Vector2(0,jumpForce * vertices * vertices * 20 * Time.deltaTime));
		}

	}

	void ChangeShape(int sides){
		switch (sides){
		case 4:{
					GetComponent<PolygonCollider2D>().enabled = false;
					GetComponent<BoxCollider2D>().enabled = true;
					spriteRenderer.sprite = sprites[1];
			break;
				}
		case 3:{
			GetComponent<PolygonCollider2D>().enabled = true;
			GetComponent<BoxCollider2D>().enabled = false;
			spriteRenderer.sprite = sprites[0];
			break;
		}
		default: break;
		}

		horizontalSpeed = _horizontalSpeed[sides-3];
		jumpForce = _jumpForce[sides-3];
		groundCheck.transform.localPosition = _groundCheckPosition[sides-3];
		groundedRadius = _groundedRadius[sides-3];
		rigidbody2D.mass = _mass[sides-3];
		vertices = sides;
	}
}
