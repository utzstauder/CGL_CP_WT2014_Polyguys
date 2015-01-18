﻿using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class timerScript : MonoBehaviour {

	private Text text;
	private bool running;
	private float time;

	// Use this for initialization
	void Start () {
		text = GetComponent<Text>();
		running = false;
		StartTimer();
	}
	
	// Update is called once per frame
	void Update () {
		if (running){
			text.text = string.Format("{0:#0}:{1:00}:{2:00}", Mathf.Floor(time/60), Mathf.Floor(time%60), Mathf.Floor(time*100)%100);
			time += Time.deltaTime;
		}
	}

	public void StartTimer(){
		//Debug.Log ("Start!");
		running = true;
		time = 0;
	}
	
	public void StopTimer(){
		//Debug.Log ("Stop!");
		running = false;
		string finalText = "Congratulations!\n Your time was " + text.text + "\nENTER = next level\n SPACE = restart level\n ESC = quit";
		text.text = finalText;
		text.transform.localPosition = new Vector2(0,0);
	}
}
