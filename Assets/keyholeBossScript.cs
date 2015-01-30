using UnityEngine;
using System.Collections;

public class keyholeBossScript : MonoBehaviour {

	#region private variables
	[SerializeField]
	private BossScript boss;
	private Keyhole keyhole;
	private SpriteRenderer spriteRenderer;
	#endregion


	#region public variables
	[Range(1,5)]
	public int activePhase;
	#endregion


	#region initialization
	void Awake() {
		keyhole = GetComponent<Keyhole>();
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Start () {
	
	}
	#endregion


	#region gameloop
	void Update () {
		if (boss.phase == activePhase) Activate();
		else Deactivate();
	}

	void FixedUpdate() {

	}
	#endregion


	#region methods
	private void Activate(){
		spriteRenderer.enabled = true;
		if (keyhole.isTriangleKeyhole) GetComponent<PolygonCollider2D>().enabled = true;
		else if (keyhole.isSquareKeyhole) GetComponent<BoxCollider2D>().enabled = true;
	}

	private void Deactivate(){
		spriteRenderer.enabled = false;
		if (keyhole.isTriangleKeyhole) GetComponent<PolygonCollider2D>().enabled = false;
		else if (keyhole.isSquareKeyhole) GetComponent<BoxCollider2D>().enabled = false;
		keyhole.trigger = false;
	}
	#endregion


	#region functions
	#endregion

	#region colliders
	#endregion

	#region triggers
	#endregion

	#region coroutines
	#endregion
}
