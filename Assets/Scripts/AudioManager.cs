using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {
	public AudioSource efxSource;
	public static AudioManager instance = null;
	public float LowPitchRange = .95f;
	public float HighPitchRange = 1.05f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void Awake(){
		if (instance == null)
			instance = this;
		
		else if (instance !=this)
			Destroy(gameObject);
		
		DontDestroyOnLoad(gameObject);

	}
	public void PlaySingle(AudioClip clip){

		efxSource.clip=clip;

		efxSource.Play();
	}
	public void RandomizeSfx (params AudioClip[] clips){
		int randomIndex = Random.Range(0 , clips.Length);
		float RandomPitch = Random.Range(LowPitchRange, HighPitchRange);
		efxSource.pitch=RandomPitch;
		efxSource.clip=clips[randomIndex];

	}

}