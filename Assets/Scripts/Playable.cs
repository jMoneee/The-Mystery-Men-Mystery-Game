using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Playable : Interactable
{
	public string playableName;

	public override void StartHover(KeyCode key, GameObject obj)
	{
		instructions.SetPrompt(key, "play " + playableName);
	}

	public override void ActiveHover(KeyCode key, GameObject obj)
	{
		instructions.RemovePrompt(key);
	}
}
