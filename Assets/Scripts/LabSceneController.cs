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
	public TextAsset labSceneComplete;
	private bool corpse = false;

	void Start()
    {
		StartCoroutine(DoTheMostThings());
		StartCoroutine(DoCorpseExamination());
	}

	private IEnumerator DoCorpseExamination()
	{
		corpse = false;

		InspectDetailedBody[] idb = FindObjectsOfType<InspectDetailedBody>();

		yield return new WaitUntil(() => idb.Where(x => x.gameObject.activeSelf).Count() == 0);

		yield return WaitForDialog(causeOfDeathDialog);

		corpse = true;
	}

	private IEnumerator DoTheMostThings()
	{
		//opening dialog
		yield return new WaitForSeconds(0.2f);

		DialogueController = FindObjectOfType<DialogueController>();
		yield return WaitForDialog(openingDialog);

		MinigameStation[] stations = FindObjectsOfType<MinigameStation>();
		List<MinigameListing> minigames = new List<MinigameListing>();
		foreach (MinigameListing[] mlArray in stations.Select(x => x.minigames))
		{
			foreach (MinigameListing ml in mlArray)
			{
				minigames.Add(ml);
			}
		}
		yield return new WaitUntil(() => minigames.Where(x => !x.success).Count() == 0);
		Debug.Log("past all minigames");
		yield return new WaitUntil(() => corpse);

		yield return WaitForDialog(labSceneComplete);

		//transition to court scene here
	}

	private IEnumerator WaitForDialog(TextAsset asset)
	{
		DialogueController.HandleDialogueText(asset);
		yield return new WaitForSeconds(0.1f);
		yield return new WaitUntil(() => fpsplayer.enabled);
	}
}
