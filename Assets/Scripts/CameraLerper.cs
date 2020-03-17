﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraLerper : MonoBehaviour
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
    }

    public void EndGame()
    {
        fpsPlayer.SetActive(true);
        playCam.gameObject.SetActive(false);
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