using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class CourtroomController : MonoBehaviour
{
    private DialogueController DialogueController;
    public TextAsset courtText;
    public RigidbodyFirstPersonController fpsplayer;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(CourtScene());
    }

    private IEnumerator CourtScene()
    {
        //opening dialog
        yield return new WaitForSeconds(0.2f);

        DialogueController = FindObjectOfType<DialogueController>();
        yield return WaitForDialog(courtText);
    }

    private IEnumerator WaitForDialog(TextAsset asset)
    {
        DialogueController.HandleDialogueText(asset);
        yield return new WaitForSeconds(0.1f);
        yield return new WaitUntil(() => fpsplayer.enabled);
    }
}
