﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DumpsterMiniGame : MonoBehaviour
{
	public Camera playCam;
	public GameObject fpsPlayer;

	public void Play()
	{
		//instead of disable, turn off controls/camera
		//that'll keep calling the "during interact" of the player so the game can end
		fpsPlayer.SetActive(false);
		playCam.gameObject.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		StartCoroutine(lerpToPlay());
		StartCoroutine(endGame());

		foreach (var item in FindObjectsOfType<DumpsterPickupable>())
		{
			item.cam = FindObjectsOfType<Camera>().Where(x => x.enabled).First();
		}
	}

	private IEnumerator endGame()
	{
		Playable p = GetComponent<Playable>();
		yield return new WaitForSeconds(1f);
		yield return new WaitUntil(() => Input.GetKeyDown(p.key));
		fpsPlayer.SetActive(true);
		playCam.gameObject.SetActive(false);
		GetComponent<Playable>().InteractEnd();
	}

	private IEnumerator lerpToPlay()
	{
		Quaternion fpsRot = fpsPlayer.transform.rotation;
		Quaternion playRot = playCam.transform.rotation;
		Vector3 fpsPos = fpsPlayer.transform.position;
		Vector3 playPos = playCam.transform.position;
		playCam.transform.rotation = fpsRot;
		playCam.transform.position = fpsPos;

		float timer = 0f;
		while (timer <= 1f)
		{
			playCam.transform.rotation = Quaternion.Lerp(fpsRot, playRot, timer);
			playCam.transform.position = Vector3.Lerp(fpsPos, playPos, timer);
			timer += Time.deltaTime;
			yield return null;
		}
	}
}