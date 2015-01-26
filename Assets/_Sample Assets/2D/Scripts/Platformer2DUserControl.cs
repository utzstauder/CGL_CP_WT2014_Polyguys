using UnityEngine;

[RequireComponent(typeof(PlatformerCharacter2D))]
public class Platformer2DUserControl : MonoBehaviour 
{
	private PlatformerCharacter2D character;
    private bool jump;

	// control schemes
	// 0 = jump, 1 = left, 2 = shoot, 3 = right
	private KeyCode[] controlsP1 = {KeyCode.W, KeyCode.A, KeyCode.S, KeyCode.D};
	private KeyCode[] controlsP2 = {KeyCode.UpArrow, KeyCode.LeftArrow, KeyCode.DownArrow, KeyCode.RightArrow};

	void Awake()
	{
		character = GetComponent<PlatformerCharacter2D>();
	}

//    void Update ()
//    {
//        // Read the jump input in Update so button presses aren't missed.
//		if (character.playerID == 1){
//			if (Input.GetButtonDown("p1Jump")) jump = true;
////			if (Input.GetKeyDown(controlsP1[0])) jump = true;
//			//if (Input.GetKeyDown(KeyCode.F)) character.ChangeShape(character.currentVertices+1);
//			if (Input.GetButtonDown("p1Shoot")) character.Shoot();
////			if (Input.GetKeyDown(controlsP1[2])) character.Shoot();
//		} else if (character.playerID == 2){
//			if (Input.GetButtonDown("p2Jump")) jump = true;
////			if (Input.GetKeyDown(controlsP2[0])) jump = true;
//			//if (Input.GetKeyDown(KeyCode.F)) character.ChangeShape(character.currentVertices+1);
//			if (Input.GetButtonDown("p2Shoot")) character.Shoot();
////			if (Input.GetKeyDown(controlsP2[2])) character.Shoot();
//		}
//    }
//
//	void FixedUpdate()
//	{
//		// Read the inputs.
//		//float h = Input.GetAxis("Horizontal");
//		float h = 0;
//
//		if (character.playerID == 1){
////			if (Input.GetKey(controlsP1[1])) h = -1.0f;
////			else if (Input.GetKey(controlsP1[3])) h = 1.0f;
//			h = Input.GetAxis ("p1Horizontal");
//		}else if (character.playerID == 2){
////			if (Input.GetKey(controlsP2[1])) h = -1.0f;
////			else if (Input.GetKey(controlsP2[3])) h = 1.0f;
//			h = Input.GetAxis("p2Horizontal");
//		}
//
//		// Pass all parameters to the character control script.
//		character.Move( h, jump );
//
//        // Reset the jump input once it has been used.
//	    jump = false;
//	}

}
