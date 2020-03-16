using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayableTarget : Interactable
{
	private void Start()
	{
		_key = KeyCode.Q;
	}

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
		interactAction?.Invoke();
	}

	public override void InteractContinue()
	{
		instructions.RemovePrompt(key);
	}

	public override void InteractEnd()
	{

	}

	private void OnValidate()
	{
		Start();
	}
}
