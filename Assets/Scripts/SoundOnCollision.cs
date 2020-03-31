using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundOnCollision : MonoBehaviour
{
	[SerializeField] AudioSource sound;

	private void OnCollisionEnter(Collision collision)
	{
		sound.Play();
	}
}
