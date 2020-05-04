using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class ActivateMenu : MonoBehaviour
{
	public GameObject Menu;
	public bool isPaused = false;
	public CanvasGroup dialogueCanvasGroup;
	public CanvasGroup journalCanvasGroup;
	private float journalAlpha = 1;
	private float dialogueAlpha = 1;
    private CursorLockMode prevLock;
    private bool prevCursorVisible;

    private void Start()
    {
        Menu.SetActive(false);
    }

    // Update is called once per frame
    void Update() //pause game and activate menu
    {
		if (Input.GetKeyDown(KeyCode.Escape))
		{
            if (isPaused == false)
            {
                Pause();
            }
            else
            {
                Resume();
            }
        }
    }

    public void Pause()
    {
        Menu.SetActive(true);
        isPaused = true;
        DetachCamera.Detach();
        prevCursorVisible = Cursor.visible;
        prevLock = Cursor.lockState;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        journalAlpha = journalCanvasGroup.alpha;
        dialogueAlpha = dialogueCanvasGroup.alpha;
        dialogueCanvasGroup.alpha = 0;
        journalCanvasGroup.alpha = 0;
        DialogueController.lockDialogue = true;
    }

    public void Resume() //resume game and hide menu
	{
		isPaused = false;
		Menu.SetActive(false);

        if (DialogueController.instance.HandlingText == false && JournalActivator.IsPaused == false)
        {
            Cursor.visible = prevCursorVisible;
            Cursor.lockState = prevLock;
        }

        DetachCamera.Reattach();
        dialogueCanvasGroup.alpha = dialogueAlpha;
        journalCanvasGroup.alpha = journalAlpha;

        DialogueController.lockDialogue = false;
	}
}
