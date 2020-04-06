using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class CrimeSceneController : MonoBehaviour
{
	private DialogueController DialogueController;
	public RigidbodyFirstPersonController fpsplayer;
	[Space]
	[Space]

	public TextAsset openingDialog;
	//public List<GameObject> initialInspections;
	public TextAsset initialInspectionComplete;
	[Space]
	public GameObject photographGameContainer;
	public PlayableTarget photographGame;
	public TextAsset endOfPhotoGame;
	[Space]
	public List<GameObject> detailedBodyInspection;
	public TextAsset endOfBodyInspection;
	[Space]
	public TextAsset endOfSceneDialog;
	//public List<Dialogueable> detailedOtherInspection;

	void Start()
    {
		StartCoroutine(DoTheEverything());
	}

    private IEnumerator DoTheEverything()
	{
		//opening dialog
		yield return new WaitForSeconds(0.2f);
		photographGameContainer.SetActive(false);

		DialogueController = FindObjectOfType<DialogueController>();
		yield return WaitForDialog(openingDialog);


		//initial inspections of all objects
		InspectInitial[] ii = FindObjectsOfType<InspectInitial>();
		foreach (var item in ii)
		{
			item.enabled = true;
		}
		yield return new WaitUntil(() => (ii.Where(x => x.enabled).Count() == 0)
			&& fpsplayer.enabled);

		//wait for end of initial inspection complete dialog
		yield return WaitForDialog(initialInspectionComplete);


		//play the game and tag things
		photographGameContainer.SetActive(true);
		foreach (var item in FindObjectsOfType<Taggable>())
		{
			item.enabled = true;
		}
		photographGame.InteractBegin();
		print("waiting for photo game to end");
		yield return new WaitUntil(() => photographGame.GetComponent<PictureMinigame>().win);
		print("photo game end");

		//end of minigame dialog
		yield return WaitForDialog(endOfPhotoGame);


		//time for detailed inspections of the body
		//fuck unity for making me do this differently than FindObjectsOfType<> because it doesn't return inactive objects
		foreach (var item in detailedBodyInspection)
		{
			item.gameObject.SetActive(true);
		}
		print("waiting for detailed inspection of body");
		yield return new WaitUntil(() => detailedBodyInspection.Where(x => x.gameObject.activeSelf).Count() == 0);
		print("detailed inspection of body end");

		//finished inspecting body
		yield return WaitForDialog(endOfBodyInspection);

		//now detailed inspect the other things
		InspectDetailedOther[] ido = FindObjectsOfType<InspectDetailedOther>();
		//Playable dumpster = FindObjectOfType<Playable>();
		//dumpster.enabled = true;
		foreach (var item in ido)
		{
			item.enabled = true;
			if (item.GetComponent<Playable>())
			{
				StartCoroutine(dumpsterWhatever(item));
			}
		}
		print("waiting for detailed inspection of other");
		yield return new WaitUntil(() => (ido.Where(x => x.enabled).Count() == 0) && FindObjectOfType<DumpsterMiniGame>().win);
		print("detailed inspection of other end");

		yield return WaitForDialog(endOfSceneDialog);
		//exit scene here

	}

	private IEnumerator dumpsterWhatever(InspectDetailedOther item)
	{
		yield return new WaitUntil(() => !item.enabled && fpsplayer.enabled);
		FindObjectOfType<DumpsterMiniGame>().Play();
	}

	private IEnumerator WaitForDialog(TextAsset endOfBodyInspection)
	{
		DialogueController.HandleDialogueText(endOfBodyInspection);
		yield return new WaitForSeconds(0.1f);
		yield return new WaitUntil(() => fpsplayer.enabled);
	}
}
