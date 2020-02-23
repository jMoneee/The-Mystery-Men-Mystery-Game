using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class DialogueSystem : MonoBehaviour
{
    public static DialogueSystem instance;
    public ELEMENTS elements;

    public float speed = 2;
    public bool useEncapsulation = true;

    private void Awake()
    {
        instance = this;
        speakerNameText.text = "";
        speechText.text = "";
    }

    public void Say(string speech, string speaker = "", bool additive = false)
    {
        StopSpeaking();
        speaking = StartCoroutine(Speaking(speech, speaker, additive));
    }

    private void StopSpeaking()
    {
        speechText.text = targetSpeech;

        if (isSpeaking)
        {
            StopCoroutine(speaking);
            speaking = null;
        }

        if (TextArchitect != null && TextArchitect.isConstructing)
        {
            TextArchitect.Stop();
        }
    }

    [HideInInspector] public bool isWaitingForUserInput = false;
    public bool isSpeaking { get { return speaking != null; } }
    Coroutine speaking = null;

    public string targetSpeech { get; set; }

    public TextArchitect TextArchitect { get; private set; }

    public IEnumerator Speaking(string speech, string speaker, bool additive)
    {
        speechPanel.SetActive(true);

        string preText = additive ? speechText.text : "";
        targetSpeech = preText + speech;

        if (TextArchitect == null)
        {
            TextArchitect = new TextArchitect(speechText, speech, preText, 1, speed);
        }
        else
        {
            TextArchitect.Renew(speech, preText);
        }

        speakerNameText.text = DetermineSpeaker(speaker);
        speakerPanel.SetActive(speakerNameText.text != "");

        isWaitingForUserInput = false;

        while (TextArchitect.isConstructing)
        {
            //if (Input.GetKeyDown(KeyCode.Space))
            //{
            //    TextArchitect.skip = true;
            //}

            yield return new WaitForEndOfFrame();
        }

        isWaitingForUserInput = true;
        while (isWaitingForUserInput)
        {
            yield return new WaitForEndOfFrame();
        }

        StopSpeaking();
    }

    private string DetermineSpeaker(string speaker)
    {
        string retVal = speakerNameText.text;
        if (speaker != speakerNameText.text && speaker != "")
        {
            retVal = (speaker.ToLower().Contains("narrator")) ? "" : speaker;
        }

        return "<b><i>" + retVal + "</b></i>";
    }

    public void ToggleVisibility()
    {
        StopSpeaking();
        speechPanel.SetActive(!speechPanel.activeSelf);
    }

    [System.Serializable]
    public class ELEMENTS
    {
        //The main panel containing all dialogue elements
        public GameObject speechPanel;
        public GameObject speakerPanel;
        public TextMeshProUGUI speakerNameText;
        public TextMeshProUGUI speechText;
    }
    public GameObject speechPanel { get { return elements.speechPanel; } }
    public GameObject speakerPanel { get { return elements.speakerPanel; } }
    public TextMeshProUGUI speakerNameText { get { return elements.speakerNameText; } }
    public TextMeshProUGUI speechText { get { return elements.speechText; } }
}
