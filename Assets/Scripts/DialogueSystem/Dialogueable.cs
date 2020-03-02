using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dialogueable : Interactable
{
	public DialogueController DialogueController;
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
		instructions.RemovePrompt(key);
		_interacting = true;
		DialogueController.gameObject.SetActive(true);
		DialogueController.HandleDialogueText(textChapter);
		interactAction?.Invoke();
	}

	public override void InteractContinue()
	{

	}

	public override void InteractEnd()
	{

	}
}
