using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Examineable : Interactable
{
	public override void HoverBegin()
	{
		
	}

	public override void HoverContinue()
	{
		
	}

	public override void HoverEnd()
	{
		
	}

	public override void InteractBegin()
	{
		instructions.SetPrompt(key, startVerb + " " + TextEffects.EmphasizeWord(name, Color.red));
	}

	public override void InteractContinue()
	{
		//show some dialogue if they input the key
	}

	public override void InteractEnd()
	{
		instructions.RemovePrompt(key);
	}
}
