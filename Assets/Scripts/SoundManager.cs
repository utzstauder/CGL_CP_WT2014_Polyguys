using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour {

	public GameObject musicObject;
	private AudioSource[] audioSources;

	// Use this for initialization
	void Awake(){
		DontDestroyOnLoad(this.gameObject);
	}

	void Start () {
		audioSources = musicObject.GetComponents<AudioSource>();
	}
	
	// Update is called once per frame
	void Update () {

	}

	public void StartMusic(float fadeTime){
		foreach (AudioSource audioSource in audioSources){
			audioSource.Play();
		}
		if (fadeTime > 0) StartCoroutine(FadeIn(fadeTime));
	}

	public void StopMusic(float fadeTime){
		if (fadeTime <= 0)
			foreach (AudioSource audioSource in audioSources){
				audioSource.Stop();
			}
		else StartCoroutine(FadeOut(fadeTime));
	}

	private IEnumerator FadeIn(float fadeTime){
		for (float v = 0; v >= 1; v += Time.deltaTime/fadeTime){
			foreach (AudioSource audioSource in audioSources){
				audioSource.volume *= v;
			}
			yield return new WaitForEndOfFrame();
		}
	}

	private IEnumerator FadeOut(float fadeTime){
		for (float v = 1f; v >= 0; v -= Time.deltaTime/fadeTime){
			foreach (AudioSource audioSource in audioSources){
				audioSource.volume *= v;
			}
			yield return new WaitForEndOfFrame();
		}
	}
}
