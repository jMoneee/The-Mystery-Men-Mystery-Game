using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Playable : Interactable
{
	public string gameName;

	public override void StartHover()
	{
		instructions.SetPrompt(key, startVerb + " " + TextEffects.EmphasizeWord(gameName, Color.red));
	}

	public override void DuringHover()
	{

	}
	public override void EndHover()
	{
		throw new System.NotImplementedException();
	}

	public override void StartInteract()
	{
		instructions.SetPrompt(key, endVerb + " " + TextEffects.EmphasizeWord(gameName, Color.red));
		_interacting = true;
		interactAction?.Invoke();
	}


	public override void DuringInteract()
	{
		throw new System.NotImplementedException();
	}

	public override void EndInteract()
	{
		_interacting = false;
	}
}
