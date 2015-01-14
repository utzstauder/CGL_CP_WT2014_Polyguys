using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public GameObject musicObject;
	private AudioSource[] audioSources;

	// Use this for initialization
	void Start () {
		audioSources = musicObject.GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void StartMusic(){
		foreach (AudioSource audioSource in audioSources){
			audioSource.Play();
		}
	}
}
