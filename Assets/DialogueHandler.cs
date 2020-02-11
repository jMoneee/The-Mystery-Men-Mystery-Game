using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DialogueHandler : MonoBehaviour
{
    public DialogueController DialogueController;
    public TextMeshProUGUI StartDialogueText;

    // Start is called before the first frame update
    void Start()
    {
        DialogueController.gameObject.SetActive(false);
        StartDialogueText.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, transform.forward, out raycastHit, 100))
        {
            Dialogueable dialogueable = raycastHit.collider.gameObject.GetComponent<Dialogueable>();
            if (dialogueable != null)
            {
                if (StartDialogueText.gameObject.activeSelf == false)
                {
                    StartDialogueText.gameObject.SetActive(true);
                    StartDialogueText.text = "Press <color=red>E</color> to <color=yellow>" + dialogueable.interactionText + "</color>";
                }

                if (Input.GetKeyDown(KeyCode.E))
                {

                }
            }
            else
            {
                StartDialogueText.gameObject.SetActive(false);
            }
        }
        else
        {
            StartDialogueText.gameObject.SetActive(false);
        }
    }
}
