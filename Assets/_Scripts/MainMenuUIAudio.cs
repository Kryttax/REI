using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuUIAudio : MonoBehaviour
{
	public AudioClip sonarPing;

	private AudioSource source;


	void Awake ()
	{
		source = GetComponent<AudioSource> ();
	}


	void PlaySonarPing ()
	{
		source.PlayOneShot (sonarPing);
	}
}
