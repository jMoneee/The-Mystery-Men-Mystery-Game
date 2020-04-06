using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CameraLerper : MonoBehaviour
{
    public Camera playCam;
    public GameObject fpsPlayer;
    public float lerpTime = 1;

    public void Play(Vector3? targetPos = null, Quaternion? targetRot = null)
    {
        //instead of disable, turn off controls/camera
        //that'll keep calling the "during interact" of the player so the game can end
        fpsPlayer.SetActive(false);
        playCam.gameObject.SetActive(true);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        Vector3 playPos = targetPos.HasValue ? targetPos.Value : playCam.transform.position;
        Quaternion playRot = targetRot.HasValue ? targetRot.Value : playCam.transform.rotation;

        StartCoroutine(lerpToPlay(playPos, playRot));
    }

    public void EndGame()
    {
        fpsPlayer.SetActive(true);
        playCam.gameObject.SetActive(false);
    }

    private IEnumerator lerpToPlay(Vector3 playPos, Quaternion playRot)
    {
        Quaternion fpsRot = fpsPlayer.transform.rotation;
        Vector3 fpsPos = fpsPlayer.transform.position;
        playCam.transform.rotation = fpsRot;
        playCam.transform.position = fpsPos;

        float timer = 0f;
        do
        {
            if (lerpTime != 0)
            {
                playCam.transform.rotation = Quaternion.Lerp(fpsRot, playRot, timer / lerpTime);
                playCam.transform.position = Vector3.Lerp(fpsPos, playPos, timer / lerpTime);
            }
            else
            {
                playCam.transform.rotation = playRot;
                playCam.transform.position = playPos;
            }
            timer += Time.deltaTime;
            yield return null;
        } while (timer <= lerpTime);

        playCam.transform.rotation = playRot;
        playCam.transform.position = playPos;
    }
}
