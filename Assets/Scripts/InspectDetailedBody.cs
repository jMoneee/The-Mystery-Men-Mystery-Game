using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InspectDetailedBody : Dialogueable
{
	private void Start()
	{
		_key = KeyCode.E;
	}

	public override void InteractBegin()
	{
		base.InteractBegin();
		gameObject.SetActive(false);
	}

	private void OnValidate()
	{
		Start();
	}
}
