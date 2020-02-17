using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumpsterMiniGame : MonoBehaviour
{
	public Camera playCam;
	public GameObject fpsPlayer;

	private void Start()
	{
		playCam = GetComponent<Camera>();
	}
	public void Play()
	{
		fpsPlayer.SetActive(false);
		playCam.gameObject.SetActive(true);
		Cursor.lockState = CursorLockMode.Confined;
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
			timer += Time.deltaTime / 2f;
			yield return null;
		}
	}
}
