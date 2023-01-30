using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;

public class GameManager : MonoBehaviour {

	public enum State{mainmenu, playing, paused, endlevel, endgame};
	public State state;

	public bool playMusic;
	public bool enableTimer;

	public GameObject playerPrefab;

	public Transform spawnP1;
	public Transform spawnP2;
	public Color colorP1;
	public Color colorP2;

	[HideInInspector]
	public GameObject[] players;

	[HideInInspector]
	public Vector3[] spawnPoints;

	// reference objects

	public Canvas mainMenu;
	public EventSystem mainMenuEventSystem;
	public Canvas pauseMenu;
	public EventSystem pauseMenuEventSystem1;
	public EventSystem pauseMenuEventSystem2;
	public GameObject goal;
	public GameObject timerObject;
	private timerScript timer;
	public Image black;
	public float fadeTime = 2f;
	public Text endText;
	public Image endBlack;

	// data variables

	private int verticesAtLevelStartP1 = 3;
	private int verticesAtLevelStartP2 = 3;

	// Use this for initialization
	void Awake(){
		StartCoroutine(FadeToClear(1f));

		DontDestroyOnLoad(this.gameObject);
		endText.gameObject.SetActive(false);

		mainMenu.gameObject.SetActive(true);
		pauseMenu.gameObject.SetActive(false);

		pauseMenuEventSystem1.enabled = false;
		pauseMenuEventSystem2.enabled = false;

		// go to menu
		state = State.mainmenu;
	}

	void Start () {
		players = new GameObject[2];

		timer = timerObject.GetComponent<timerScript>();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.T)) timer.ToggleShow();

		switch(state){
		case State.mainmenu:
			mainMenu.gameObject.SetActive(true);
			mainMenu.enabled = true;
			pauseMenu.gameObject.SetActive(false);
			pauseMenu.enabled = false;
			break;

		case State.playing:
			// hide main menu
			mainMenu.enabled = false;
			mainMenu.gameObject.SetActive(false);
			// hide pause menu
			pauseMenu.enabled = false;
			pauseMenu.gameObject.SetActive(false);

			// check if players reached the end of the level
			if (goal) if (goal.GetComponent<exitTrigger>().player1InTrigger &&  goal.GetComponent<exitTrigger>().player2InTrigger) EndLevel();

			// pause menu
//			if (Input.GetKeyDown(KeyCode.Escape)) PauseGame();
			if (Input.GetButtonDown("p1Pause")) PauseGame(1);
			    else if (Input.GetButtonDown("p2Pause")) PauseGame(2);
			break;

		case State.paused:
			// show pause menu
			pauseMenu.gameObject.SetActive(true);
			pauseMenu.enabled = true;
			// close pause menu
			if (Input.GetButtonDown("p1Pause") || Input.GetButtonDown("p2Pause")) ResumeGame();
			break;

		case State.endlevel:
				break;

		case State.endgame:
			if (Input.GetButtonDown("p1Pause") || Input.GetButtonDown("p2Pause")) GoToMainMenu();
			break;

		default: break;
		}
	}

#region menu functions
	public void StartGame(){
		StopCoroutine(FadeToClear(.5f));

		//disable event system to prevent button presses
		mainMenuEventSystem.enabled = false;

		// load first level
		verticesAtLevelStartP1 = 3;
		verticesAtLevelStartP2 = 3;
		if (playMusic) GetComponent<SoundManager>().StartMusic(1f);
		StartCoroutine(loadNextLevel());

		timerObject.SetActive(true);
	}
	
	public void QuitGame(){
		//disable event system to prevent button presses
		mainMenuEventSystem.enabled = false;

		Application.Quit();
	}

	private void PauseGame(int playerID){
				switch (playerID){
				case 1: pauseMenuEventSystem1.enabled = true; 
					break;
				case 2: pauseMenuEventSystem2.enabled = true;
					break;
				default: break;
				}
		foreach (GameObject player in players){
			player.GetComponent<PlatformerCharacter2D>().playerHasControl = false;
			player.GetComponent<Rigidbody2D>().isKinematic = true;
		}
		timer.PauseTimer();
		state = State.paused;
	}
	
	public void ResumeGame(){
		pauseMenuEventSystem1.enabled = false;
		pauseMenuEventSystem2.enabled = false;

		foreach (GameObject player in players){
			player.GetComponent<PlatformerCharacter2D>().playerHasControl = true;
			player.GetComponent<Rigidbody2D>().isKinematic = false;
		}
		timer.ResumeTimer();
		state = State.playing;
	}

	public void RestartLevel(){
		pauseMenuEventSystem1.enabled = false;
		pauseMenuEventSystem2.enabled = false;
		StartCoroutine(ReloadCurrentLevel());
	}

	public void GoToMainMenu(){
		pauseMenuEventSystem1.enabled = false;
		pauseMenuEventSystem2.enabled = false;
		StartCoroutine(loadMainMenu());
	}
#endregion


	void InitScene(){
		spawnP1 = GameObject.Find ("StartP1").transform;
		spawnP2 = GameObject.Find ("StartP2").transform;
		spawnPoints = new Vector3[2]{spawnP1.position, spawnP2.position};
		goal = GameObject.Find ("_exitTrigger");

		SpawnPlayers(verticesAtLevelStartP1, verticesAtLevelStartP2);


		// reset the timer
		timer.ResetTimer();
		timer.StartTimer();
		// give the players control
		foreach (GameObject player in players) player.GetComponent<PlatformerCharacter2D>().playerHasControl = true;


		state = State.playing;
	}

	void EndLevel(){
		state = State.endlevel;
		timer.StopTimer();

		// save the players current vertices
		verticesAtLevelStartP1 = players[0].GetComponent<PlatformerCharacter2D>().currentVertices;
		verticesAtLevelStartP2 = players[1].GetComponent<PlatformerCharacter2D>().currentVertices;

		// if there is another level load it else end the game
		if (Application.loadedLevel != Application.levelCount-1) StartCoroutine(loadNextLevel());
		else EndGame();
	}

	void EndGame(){
		foreach (GameObject player in players) player.GetComponent<PlatformerCharacter2D>().playerHasControl = false;
		StartCoroutine(FadeToEnd(3f));
		state = State.endgame;
	}


	void SpawnPlayers(int verticesP1, int verticesP2){
		// kill the player objects if existing
		foreach (GameObject player in players) if (player) Destroy (player.gameObject);

		// check for correct vertex count
		switch(Application.loadedLevel){
		case 0: break;
		case 1: break;
		case 2:
			if ((verticesP1 + verticesP2) < 7) verticesP1++;
			break;
		default: break;
		}


		// Spawn players 
		players[0] = Instantiate(playerPrefab, spawnPoints[0], Quaternion.identity) as GameObject;
		players[0].GetComponent<PlatformerCharacter2D>().playerID = 1;
		players[0].GetComponent<PlatformerCharacter2D>().Init(verticesP1);
		
		players[1] = Instantiate(playerPrefab, spawnPoints[1], Quaternion.identity) as GameObject;
		players[1].GetComponent<PlatformerCharacter2D>().playerID = 2;
		players[1].GetComponent<PlatformerCharacter2D>().Init(verticesP2);
		
		players[0].GetComponent<PlatformerCharacter2D>().otherPlayer = players[1];
		players[1].GetComponent<PlatformerCharacter2D>().otherPlayer = players[0];
	}

	private IEnumerator ReloadCurrentLevel(){
		//fade to black
		black.enabled = true;
		for (float a = 0; a<1f; a += Time.deltaTime/fadeTime){
			black.color = new Color(0, 0, 0, a);
			yield return new WaitForEndOfFrame();
		}
		black.color = Color.black;

		Application.LoadLevel(Application.loadedLevel);
		while (Application.isLoadingLevel) yield return new WaitForEndOfFrame();

		InitScene();

		//fade to clear
		black.color = Color.black;
		for (float a = 1f; a>0; a -= Time.deltaTime/fadeTime){
			black.color = new Color(0, 0, 0, a);
			yield return new WaitForEndOfFrame();
		}
		black.enabled = false;
	}

	private IEnumerator loadNextLevel(){
		//fade to black
		black.enabled = true;
		for (float a = 0; a<1f; a += Time.deltaTime/fadeTime){
			black.color = new Color(0, 0, 0, a);
			yield return new WaitForEndOfFrame();
		}
		black.color = Color.black;

		Application.LoadLevel(Application.loadedLevel+1);
		while (Application.isLoadingLevel) yield return new WaitForEndOfFrame();

		InitScene ();

		//fade to clear
		black.color = Color.black;
		for (float a = 1f; a>0; a -= Time.deltaTime/fadeTime){
			black.color = new Color(0, 0, 0, a);
			yield return new WaitForEndOfFrame();
		}
		black.enabled = false;
	}

	private IEnumerator loadMainMenu(){
		if (playMusic) GetComponent<SoundManager>().StopMusic(2f);

		//fade to black
		black.enabled = true;
		for (float a = 0; a<1f; a += Time.deltaTime/fadeTime){
			black.color = new Color(0, 0, 0, a);
			yield return new WaitForEndOfFrame();
		}


		Destroy(pauseMenu.gameObject);
		Destroy(mainMenu.gameObject);

		Application.LoadLevel(0);
		// kill the player objects
		foreach (GameObject player in players) if (player) Destroy (player.gameObject);
		Destroy(this.gameObject);
	}

	private IEnumerator FadeToBlack(float fadeTime){
		black.enabled = true;
		for (float a = 0; a<1f; a += Time.deltaTime/fadeTime){
			black.color = new Color(0, 0, 0, a);
			yield return new WaitForEndOfFrame();
		}
		black.color = Color.black;
	}

	private IEnumerator FadeToClear(float fadeTime){
		black.enabled = true;
		black.color = Color.black;
		for (float a = 1f; a>0; a -= Time.deltaTime/fadeTime){
			black.color = new Color(0, 0, 0, a);
			yield return new WaitForEndOfFrame();
		}
		black.enabled = false;
	}

	private IEnumerator FadeToEnd(float fadeTime){
		endBlack.enabled = true;
		endText.gameObject.SetActive(true);
		endText.enabled = true;
		for (float a = 0; a<1f; a += Time.deltaTime/fadeTime){
			endText.color = new Color(1f,1f,1f,a);
			endBlack.color = new Color(0, 0, 0, a);
			yield return new WaitForEndOfFrame();
		}
		endBlack.color = Color.black;
		endText.color = Color.white;
	}
}
