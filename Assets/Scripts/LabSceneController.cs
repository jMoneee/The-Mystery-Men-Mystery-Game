using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class LabSceneController : MonoBehaviour
{
	private DialogueController DialogueController;
	public RigidbodyFirstPersonController fpsplayer;
	[Space]
	[Space]
	public TextAsset openingDialog;

	void Start()
    {
		StartCoroutine(DoTheEverything());
	}

	private IEnumerator DoTheEverything()
	{
		//opening dialog
		yield return new WaitForSeconds(0.2f);

		DialogueController = FindObjectOfType<DialogueController>();
		yield return WaitForDialog(openingDialog);
	}

	private IEnumerator WaitForDialog(TextAsset asset)
	{
		DialogueController.HandleDialogueText(asset);
		yield return new WaitUntil(() => fpsplayer.enabled);
	}
}
