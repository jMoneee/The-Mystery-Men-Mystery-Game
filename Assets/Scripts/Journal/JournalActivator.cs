using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JournalActivator : MonoBehaviour
{
    public static JournalActivator instance;
    private void Awake()
    {
        instance = this;
    }
    public static bool IsPaused { get { return instance.journalActive == true; } }

    private CanvasGroup JournalCanvas { get { return JournalManager2.instance.GetComponentInParent<CanvasGroup>(); } }
	public AudioClip showSound;
	public AudioClip hideSound;
    private bool journalActive = false;
	public AudioSource sound;
    private CursorLockMode prevLock;
    private bool prevCursorVisible;

    void Start()
    {
		JournalCanvas.ChangeCanvasGroupVisibility(false);
	}

	void Update()
    {
        if (FindObjectOfType<ActivateMenu>().isPaused == false)
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (journalActive)
                {
                    DialogueController.lockDialogue = false;

                    DetachCamera.Reattach();
                    JournalCanvas.ChangeCanvasGroupVisibility(false);
                    journalActive = false;

                    if (DialogueController.instance.HandlingText == false)
                    {
                        Cursor.lockState = prevLock;
                        Cursor.visible = prevCursorVisible;
                    }

                    sound.PlayOneShot(hideSound);
                }
                else
                {
                    DialogueController.lockDialogue = true;

                    DetachCamera.Detach();
                    JournalCanvas.ChangeCanvasGroupVisibility(true);
                    journalActive = true;
                    prevLock = Cursor.lockState;
                    prevCursorVisible = Cursor.visible;
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    sound.PlayOneShot(showSound);
                }
            }
        }

        //if (journalActive)
        //{
        //    Cursor.lockState = CursorLockMode.None;
        //    Cursor.visible = true;
        //}
        //else
        //{
        //    Cursor.lockState = CursorLockMode.Locked;
        //    Cursor.visible = false;
        //}
    }
}
