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

    public CanvasGroup JournalCanvas;
	public AudioClip showSound;
	public AudioClip hideSound;
    private bool journalActive = false;
	public AudioSource sound;

    void Start()
    {
		JournalCanvas.ChangeCanvasGroupVisibility(false);
	}

	void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (journalActive)
            {
                DetachCamera.Reattach();
                JournalCanvas.ChangeCanvasGroupVisibility(false);
                journalActive = false;

                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;

				sound.PlayOneShot(hideSound);
            }
            else
            {
                DetachCamera.Detach();
                JournalCanvas.ChangeCanvasGroupVisibility(true);
				journalActive = true;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
				sound.PlayOneShot(showSound);
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
