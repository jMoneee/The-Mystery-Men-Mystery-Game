using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ActivateMenu : MonoBehaviour
{
	public GameObject Menu;
	private bool isPaused = false;
	public CanvasGroup dialogueCanvasGroup;
	public CanvasGroup journalCanvasGroup;
	private float journalAlpha = 1;
	private float dialogueAlpha = 1;
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update() //pause game and activate menu
    {
		if (Input.GetKey(KeyCode.Escape) && isPaused == false)
		{
			Menu.SetActive(true);
			isPaused = true;
			DetachCamera.Detach();
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
			journalAlpha = journalCanvasGroup.alpha;
			dialogueAlpha = dialogueCanvasGroup.alpha;
			dialogueCanvasGroup.alpha = 0;
			journalCanvasGroup.alpha = 0;
			DialogueController.lockDialogue = true;
		}
    }

	public void Resume() //resume game and hide menu
	{
		isPaused = false;
		Menu.SetActive(false);
		Cursor.visible = false;
		DetachCamera.Reattach();
		Cursor.lockState = CursorLockMode.Locked;
		dialogueCanvasGroup.alpha = dialogueAlpha;
		journalCanvasGroup.alpha = journalAlpha;
		DialogueController.lockDialogue = false;
	}
}
