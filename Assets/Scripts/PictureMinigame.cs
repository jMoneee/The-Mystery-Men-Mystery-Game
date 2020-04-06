using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class PictureMinigame : MonoBehaviour
{
    public int step = 0;

    public TextAsset TutorialText;

    public GameObject EvidenceParent;

    [Header("Step 1")]
    public GameObject Step1Canvas;
    public GameObject Step2Canvas;
    public GameObject Step2Objects;
    public TextMeshProUGUI Step2PicturesTakenText;
    public TextMeshProUGUI Step1EvidenceMarkedText;
    private int Step1PicturesTaken { get { return Step2Objects.GetComponentsInChildren<PlayableTarget>(true).Where(p => p.interacting).Count(); } }
    private int Step1PicturesTotal { get { return Step2Objects.GetComponentsInChildren<PlayableTarget>(true).Length; } }
    private int Step1EvidenceMarked { get { return EvidenceParent.GetComponentsInChildren<Taggable>(true).Where(p => p.interacting).Count(); } }
    private int Step1EvidenceTotal { get { return EvidenceParent.GetComponentsInChildren<Taggable>(true).Length; } }
	public bool win { get { return step == 3; } }

    public void StartStep1 ()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        DialogueController DialogueController = FindObjectOfType<DialogueController>();
        DialogueController.HandleDialogueText(TutorialText);


        Step1Canvas.SetActive(true);
        step = 1;
    }

    public void StartStep2()
    {
        Step1Canvas.SetActive(false);
        Step2Canvas.SetActive(true);
        Step2Objects.SetActive(true);
        StartCoroutine(TurnOnCameraPositions(120));
        step = 2;
    }

    private void Update()
    {
        switch (step)
        {
            case 1:
                Step1EvidenceMarkedText.text = Step1EvidenceMarked + " / " + Step1EvidenceTotal;

                if (Step1EvidenceMarked >= Step1EvidenceTotal)
                {
                    StartStep2();
                }

                break;
            case 2:
                Step2PicturesTakenText.text = Step1PicturesTaken + " / " + Step1PicturesTotal;
                if (Step1PicturesTaken >= Step1PicturesTotal)
                {
                    step = 3;
                    Step2Canvas.SetActive(false);
                }

                if (Input.GetKeyDown(KeyCode.L))
                {
                    TurnOnCameraPositions();
                }
                break;
        }
    }

    private void Start()
    {
        Step1Canvas.SetActive(false);
        Step2Canvas.SetActive(false);
        Step2Objects.SetActive(false);
    }

    IEnumerator TurnOnCameraPositions(float time)
    {
        yield return new WaitForSeconds(time);

        if (step == 2)
        {
            TurnOnCameraPositions();
        }
    }

    void TurnOnCameraPositions()
    {
        MeshRenderer[] renderers = Step2Objects.GetComponentsInChildren<MeshRenderer>(true);
        foreach(MeshRenderer rend in renderers)
        {
            //not a camera target
            if (rend.GetComponent<PlayableTarget>() == null)
                rend.enabled = true;
        }
    }
}
