using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class LabSceneController : MonoBehaviour
{
	private DialogueController DialogueController;
	public RigidbodyFirstPersonController fpsplayer;
	[Space]
	[Space]
	public TextAsset openingDialog;

	public TextAsset causeOfDeathDialog;

	void Start()
    {
		StartCoroutine(DoTheMostThings());
		StartCoroutine(DoCorpseExamination());
	}

	private IEnumerator DoCorpseExamination()
	{
		InspectDetailedBody[] idb = FindObjectsOfType<InspectDetailedBody>();

		yield return new WaitUntil(() => idb.Where(x => x.gameObject.activeSelf).Count() == 0);

		yield return WaitForDialog(causeOfDeathDialog);
	}

	private IEnumerator DoTheMostThings()
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
