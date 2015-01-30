using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class DebugManager : MonoBehaviour {
	
	public bool enableTimer;

	public GameObject playerPrefab;

	public Transform spawnP1;
	[SerializeField]
	[Range(3,8)]
	private int startVerticesP1 = 3;
	public Transform spawnP2;
	[SerializeField]
	[Range(3,8)]
	private int startVerticesP2 = 3;
	public Color colorP1;
	public Color colorP2;

	[HideInInspector]
	public GameObject[] players;

	[HideInInspector]
	public Vector3[] spawnPoints;

	// reference objects
	
	public GameObject timerObject;
	private timerScript timer;

	// Use this for initialization
	void Awake(){

	}

	void Start () {
		players = new GameObject[2];

		if (enableTimer){
			timer = timerObject.GetComponent<timerScript>();
			timer.gameObject.SetActive(true);
		}

		InitScene();

	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Alpha1)) AddVertex(players[0]);
		if (Input.GetKeyDown(KeyCode.Alpha2)) AddVertex(players[1]);
	}

#region menu functions

#endregion


	void InitScene(){
		spawnPoints = new Vector3[2]{spawnP1.position, spawnP2.position};

		SpawnPlayers(startVerticesP1, startVerticesP2);


		// reset the timer
		if (timer) timer.ResetTimer();
		if (timer) timer.StartTimer();
		// give the players control
		foreach (GameObject player in players) player.GetComponent<PlatformerCharacter2D>().playerHasControl = true;
	}

	void SpawnPlayers(int verticesP1, int verticesP2){
		// kill the player objects if existing
		foreach (GameObject player in players) if (player) Destroy (player.gameObject);
		
		// Spawn players 
		players[0] = Instantiate(playerPrefab, spawnPoints[0], Quaternion.identity) as GameObject;
		players[0].GetComponent<PlatformerCharacter2D>().playerID = 1;
		players[0].GetComponent<PlatformerCharacter2D>().Init(verticesP1, colorP1);
		
		players[1] = Instantiate(playerPrefab, spawnPoints[1], Quaternion.identity) as GameObject;
		players[1].GetComponent<PlatformerCharacter2D>().playerID = 2;
		players[1].GetComponent<PlatformerCharacter2D>().Init(verticesP2, colorP2);
		
		players[0].GetComponent<PlatformerCharacter2D>().otherPlayer = players[1];
		players[1].GetComponent<PlatformerCharacter2D>().otherPlayer = players[0];
	}

	void AddVertex(GameObject player){
		PlatformerCharacter2D character = player.GetComponent<PlatformerCharacter2D>();

		if (character.currentVertices < 8) character.ChangeShape(character.currentVertices+1);
			else character.ChangeShape(3);
	}

}
