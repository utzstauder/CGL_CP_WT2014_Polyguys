using UnityEngine;
using System.Collections;

public class SimplePlayerMovement : MonoBehaviour {

	public float speed = 10f;
	public float maxSpeed = 100f;
	
	private float lastSynchronizationTime = 0.0f;
	private float syncDelay = 0.0f;
	private float syncTime = 0.0f;
	private Vector3 syncStartPosition = Vector3.zero;
	private Vector3 syncEndPosition = Vector3.zero;

	SpriteRenderer spriteRenderer;
	static Sprite[] sprites;

	void Awake () {
		spriteRenderer = GetComponent<SpriteRenderer>();

		sprites = new Sprite[]{	Resources.Load("Sprites/balint", typeof(Sprite)) as Sprite,
								Resources.Load("Sprites/jo", typeof(Sprite)) as Sprite,
								Resources.Load("Sprites/cunt", typeof(Sprite)) as Sprite,
								Resources.Load("Sprites/utz", typeof(Sprite)) as Sprite
		};

		Debug.Log (sprites.Length);
		for (int i = 0; i < sprites.Length; i++){
			Debug.Log(sprites[i]);
		}
	}

	void Update()
	{

		GetComponent<Rigidbody2D>().velocity = Vector2.ClampMagnitude(GetComponent<Rigidbody2D>().velocity, maxSpeed);
	}

	private void InputColorChange(){
		if (Input.GetKeyDown(KeyCode.R)){
			ChangeColorTo(new Vector3(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f)));
		}
	}

	private void InputSpriteChange(){
		if (Input.GetKeyDown(KeyCode.F)){
			ChangeSpriteTo(Random.Range(0, sprites.Length));
		}
	}

	void InputMovement()
	{
		if (Input.GetKey(KeyCode.W)){
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0,speed * Time.deltaTime));
		}

		if (Input.GetKey(KeyCode.S)){
			GetComponent<Rigidbody2D>().AddForce(new Vector2(0,-speed * Time.deltaTime));

		}
		
		if (Input.GetKey(KeyCode.D)){
			GetComponent<Rigidbody2D>().AddForce(new Vector2(speed * Time.deltaTime,0));
		}
		
		if (Input.GetKey(KeyCode.A)){
			GetComponent<Rigidbody2D>().AddForce(new Vector2(-speed * Time.deltaTime,0));
		}
	}

	void SyncedMovement(){
		syncTime += Time.deltaTime;
		GetComponent<Rigidbody2D>().position = Vector2.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
	}

	/*void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info){
		Vector3 syncPosition = Vector3.zero;
		Vector3 syncVelocity = Vector3.zero;

		if (stream.isWriting){
			syncPosition = GetComponent<Rigidbody2D>().position;
			stream.Serialize(ref syncPosition);

			syncVelocity = GetComponent<Rigidbody2D>().velocity;
			stream.Serialize(ref syncVelocity);
		}else{
			stream.Serialize(ref syncPosition);
			stream.Serialize(ref syncVelocity);

			syncTime = 0.0f;
			syncDelay = Time.time - lastSynchronizationTime;
			lastSynchronizationTime = Time.time;

			syncEndPosition = syncPosition + syncVelocity * syncDelay;
			syncStartPosition = GetComponent<Rigidbody2D>().position;
		}
	}*/

	void ChangeColorTo(Vector3 color){
		spriteRenderer.color = new Color(color.x, color.y, color.z, 1.0f);
		
	}

	void ChangeSpriteTo(int spriteIndex){
		spriteRenderer.sprite = sprites[spriteIndex];
		
	}

	void OnCollisionEnter2D(Collision2D otherObject){
		//Debug.Log (otherObject);
		ChangeColorTo(new Vector3(Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f),Random.Range(0.0f,1.0f)));
	}
}
