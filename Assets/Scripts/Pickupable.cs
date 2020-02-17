using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickupable : Interactable
{
	public override void StartHover(KeyCode pickupKey, GameObject obj)
	{
		instructions.SetPrompt(pickupKey, "pick up " + obj.name);
	}

	public override void ActiveHover(KeyCode key, GameObject obj)
	{
		instructions.SetPrompt(key, "drop " + obj.name);
	}
}
