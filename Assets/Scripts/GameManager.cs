using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameManager : MonoBehaviour {

	public enum State{initializing, playing, paused, end};
	public State state;

	public bool playMusic;
	public bool enableTimer;

	public GameObject playerPrefab;

	public Transform spawnP1;
	public Transform spawnP2;

	[HideInInspector]
	public GameObject[] players;

	[HideInInspector]
	public Vector3[] spawnPoints;

	public GameObject startObject;

	public GameObject timerObject;
	private timerScript timer;

	public GameObject goal;

	// Use this for initialization
	void Start () {
		players = new GameObject[2];
		spawnPoints = new Vector3[2]{spawnP1.position, spawnP2.position};

		if (enableTimer) timer = timerObject.GetComponent<timerScript>();

		startObject.GetComponent<Text>().text = "Welcome to Level " + (Application.loadedLevel+1) + "\nSPACE = start\nESC = quit";
		startObject.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.Escape)) Application.Quit();
		if (state == State.initializing){
			if (Input.GetKeyDown(KeyCode.Space)) StartGame ();
		}
		else if (state == State.playing){
			if (goal) if (goal.GetComponent<exitTrigger>().playersInTrigger >= 2) End ();
			if (Input.GetKeyDown(KeyCode.Space)) Application.LoadLevel(Application.loadedLevel);
		}
		else if (state == State.paused){

		}else if (state == State.end){
			if (Input.GetKeyDown(KeyCode.Space)) Application.LoadLevel(Application.loadedLevel);
			if (Input.GetKeyDown(KeyCode.Return)){
				int levelToLoad;
				if (Application.loadedLevel == Application.levelCount-1) levelToLoad = 0;
				else levelToLoad = Application.loadedLevel + 1;
				Application.LoadLevel(levelToLoad);
			} 
		}
	}

	void StartGame(){
		// Deactivate start message
		startObject.SetActive(false);

		// Spawn players
		players[0] = Instantiate(playerPrefab, spawnPoints[0], Quaternion.identity) as GameObject;
		players[0].GetComponent<PlatformerCharacter2D>().playerID = 1;
		players[0].GetComponent<PlatformerCharacter2D>().Init();
		
		players[1] = Instantiate(playerPrefab, spawnPoints[1], Quaternion.identity) as GameObject;
		players[1].GetComponent<PlatformerCharacter2D>().playerID = 2;
		players[1].GetComponent<PlatformerCharacter2D>().Init();
		
		players[0].GetComponent<PlatformerCharacter2D>().otherPlayer = players[1];
		players[1].GetComponent<PlatformerCharacter2D>().otherPlayer = players[0];

		// Start the timer
		if (enableTimer) timerObject.SetActive(true);

		if (playMusic) GetComponent<SoundManager>().StartMusic();
		state = State.playing;
	}

	void End(){
		state = State.end;
		if (enableTimer) timer.StopTimer();
		Destroy (players[0]);
		Destroy (players[1]);
	}
}
