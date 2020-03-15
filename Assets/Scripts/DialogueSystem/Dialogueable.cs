using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogueable : Interactable
{
    public TextAsset textChapter;

	public override void HoverBegin()
	{
		instructions.SetPrompt(key, startVerb + " " + TextEffects.EmphasizeWord(name, Color.red));
	}

	public override void HoverContinue()
	{
	}

	public override void HoverEnd()
	{
		instructions.RemovePrompt(key);
	}

	public override void InteractBegin()
	{

        _interacting = true;

        instructions.RemovePrompt(key);

        DialogueController.instance.currentDialogueable = this;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        DialogueController.instance.gameObject.SetActive(true);
		DialogueController.instance.HandleDialogueText(textChapter);
		interactAction?.Invoke();
	}

	public override void InteractContinue()
	{

	}

	public override void InteractEnd()
	{
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }
}
