using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Playable : Interactable
{
	public string gameName;

	public override void HoverBegin()
	{
		instructions.SetPrompt(key, startVerb + " " + TextEffects.EmphasizeWord(gameName, Color.red));
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
		instructions.SetPrompt(key, endVerb + " " + TextEffects.EmphasizeWord(gameName, Color.red));
		_interacting = true;
		interactAction?.Invoke();
	}


	public override void InteractContinue()
	{
		
	}

	public override void InteractEnd()
	{
		_interacting = false;
		instructions.RemovePrompt(key);
	}
}
