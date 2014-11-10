using UnityEngine;
using System.Collections;

[RequireComponent (typeof(AudioSource))]

public class VideoPlayer : MonoBehaviour 
{
	public MovieTexture movieTexture = null;
	private bool b_VideoFinished=false;
	private bool b_VideoStarted=false;

	void Start () 
	{
		if(movieTexture)
		{
			renderer.material.mainTexture = movieTexture as MovieTexture;
			audio.clip = movieTexture.audioClip;

			movieTexture.Play();
			audio.Play();
			b_VideoStarted=true;
		}
	}

	void Update () 
	{
		/*
		if(movieTexture)
		{
			if (Input.GetButtonDown ("Jump")) 
			{
				if (movieTexture.isPlaying) 
				{
					movieTexture.Pause();
					audio.Pause();
				}
				else 
				{
					movieTexture.Play();
					audio.Play();
				}
			}
		}
		*/
		if(movieTexture)
		{
			if(b_VideoStarted)
			{
				if (movieTexture.isPlaying) 
				{
				}
				else if(!b_VideoFinished)
				{
					b_VideoFinished=true;
					Debug.Log("Video Done "+Time.time);
				}
			}
		}
	}
}
