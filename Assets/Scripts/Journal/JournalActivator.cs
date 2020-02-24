﻿using System.Collections;
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
    private bool journalActive = false;
    // Start is called before the first frame update
    void Start()
    {
		JournalCanvas.ChangeCanvasGroupVisibility(false);
	}

	// Update is called once per frame
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
            }
            else
            {
                DetachCamera.Detach();
                JournalCanvas.ChangeCanvasGroupVisibility(true);
				journalActive = true;

                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
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
