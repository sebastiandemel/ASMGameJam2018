using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour {

    public AudioSource musicSource;

    public AudioClip[] musics;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void PlayMusic(int musicIndex)
    {
        musicSource.clip = musics[musicIndex];
        musicSource.Play();
    }
    public void StopMusic()
    {       
        musicSource.Stop();
    }
}
