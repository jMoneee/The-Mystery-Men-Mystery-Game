using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class PictureMinigame : MonoBehaviour
{
    public int step = 0;

    public GameObject EvidenceParent;

    [Header("Step 1")]
    public GameObject Step1Canvas;
    public GameObject Step1Objects;
    public TextMeshProUGUI Step1PicturesTakenText;
    public TextMeshProUGUI Step1EvidenceMarkedText;
    private int Step1PicturesTaken { get { return Step1Objects.GetComponentsInChildren<Playable_StartOnly>(true).Where(p => p.interacting).Count(); } }
    private int Step1PicturesTotal { get { return Step1Objects.GetComponentsInChildren<Playable_StartOnly>(true).Length; } }
    private int Step1EvidenceMarked { get { return EvidenceParent.GetComponentsInChildren<Playable_StartOnly>(true).Where(p => p.interacting).Count(); } }
    private int Step1EvidenceTotal { get { return EvidenceParent.GetComponentsInChildren<Playable_StartOnly>(true).Length; } }

    public void StartStep1 ()
    {
        Step1Canvas.SetActive(true);
        step = 1;
    }

    private void Update()
    {
        switch(step)
        {
            case 1:
                Step1PicturesTakenText.text = Step1PicturesTaken + " / " + Step1PicturesTotal;
                Step1EvidenceMarkedText.text = Step1EvidenceMarked + " / " + Step1EvidenceTotal;

                if (Step1PicturesTaken >= Step1PicturesTotal && Step1EvidenceMarked >= Step1EvidenceTotal)
                {
                    step = 2;
                    Step1Canvas.SetActive(false);
                }

                break;
            case 2:

                break;
        }
    }

    private void Start()
    {
        Step1Canvas.SetActive(false);
    }
}
