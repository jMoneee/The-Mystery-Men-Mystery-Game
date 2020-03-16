using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectDetailedOther : Dialogueable
{
	private void Start()
	{
		_key = KeyCode.E;
	}

	public override void InteractBegin()
	{
		base.InteractBegin();
		enabled = false;
	}

	private void OnValidate()
	{
		Start();
	}
}
