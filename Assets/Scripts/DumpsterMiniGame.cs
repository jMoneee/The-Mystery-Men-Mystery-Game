using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Reflection;

public class DumpsterMiniGame : MonoBehaviour
{
	public Camera playCam;
	public GameObject fpsPlayer;
	private bool quit = false;
	public bool _win;

	public bool win
	{
		get
		{
			return _win;
		}
		set
		{
			_win = value;
			if (_win)
				Quit();
		}
	}

	private void Start()
	{
		win = false;
		SetInteractables(typeof(DumpsterPickupable), false);
		SetInteractables(typeof(Dialogueable), false);
	}

	public void Play()
	{
		//instead of disable, turn off controls/camera
		//that'll keep calling the "during interact" of the player so the game can end
		fpsPlayer.SetActive(false);
		playCam.gameObject.SetActive(true);
		Cursor.lockState = CursorLockMode.None;
		Cursor.visible = true;
		SetInteractables(typeof(DumpsterPickupable), true);
		SetInteractables(typeof(Dialogueable), true);
		StartCoroutine(lerpToPlay());
		StartCoroutine(endGame());


		foreach (var item in FindObjectsOfType<DumpsterPickupable>())
		{
			item.cam = FindObjectsOfType<Camera>().Where(x => x.enabled).First();
		}
	}

	public void Quit()
	{
		quit = true;
	}

	private IEnumerator endGame()
	{
		Playable p = GetComponentInChildren<Playable>();
		yield return new WaitForSeconds(1f);
		yield return new WaitUntil(() => quit);
		fpsPlayer.SetActive(true);
		playCam.gameObject.SetActive(false);
		GetComponentInChildren<Playable>().InteractEnd();
		SetInteractables(typeof(DumpsterPickupable), false);
		SetInteractables(typeof(Dialogueable), false);
		if (win)
			SetInteractables(typeof(Playable), false);
	}

	private void SetInteractables(Type intType, bool toState)
	{
		foreach (MonoBehaviour item in GetComponentsInChildren(intType, true))
		{
			item.enabled = toState;
		}
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