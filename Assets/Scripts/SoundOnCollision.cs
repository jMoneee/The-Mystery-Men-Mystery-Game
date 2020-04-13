using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnCollision : MonoBehaviour
{
	[SerializeField] AudioSource sound;

	private void Start()
	{
		if (sound == null)
			sound = GetComponent<AudioSource>();
	}

	private void OnCollisionEnter(Collision collision)
	{
		if (!sound.isPlaying)
			sound.Play();
	}
}
