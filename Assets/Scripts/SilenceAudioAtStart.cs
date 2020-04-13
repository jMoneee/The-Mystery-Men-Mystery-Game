using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SilenceAudioAtStart : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
		AudioSource sound = GetComponent<AudioSource>();
		if (sound != null)
		{
			float vol = sound.volume;
			sound.volume = 0f;
			StartCoroutine(Wait(sound, vol));
		}
    }
	
	private IEnumerator Wait(AudioSource sound, float vol)
	{
		yield return new WaitUntil(() => !sound.isPlaying && Time.time > 1f);
		sound.volume = vol;
	}
}
