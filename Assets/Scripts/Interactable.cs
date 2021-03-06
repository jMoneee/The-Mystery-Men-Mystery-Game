﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;
using System.Threading.Tasks;

[RequireComponent(typeof(InteractableDisplay))]
public abstract class Interactable : MonoBehaviour
{
	protected DisplayInstructions instructions;
	[SerializeField] public string startVerb = "interact with";
	[SerializeField] public string endVerb = "stop interacting with";
	[SerializeField] protected KeyCode _key;
    public float maxInteractDistance = 2.5f;
    public KeyCode key { get { return _key; } }
	[HideInInspector]public bool _interacting = false;
	public bool interacting { get { return _interacting; } }
	public UnityEvent interactAction;

	protected virtual void Awake()
	{
		instructions = FindObjectOfType<DisplayInstructions>();
	}

	public abstract void HoverBegin();

	public abstract void HoverContinue();

	public abstract void HoverEnd();

	public abstract void InteractBegin();

	public abstract void InteractContinue();

	public abstract void InteractEnd();

	private void OnDisable()
	{
		if (_interacting)
			InteractEnd();
		else
			HoverEnd();

		GetComponent<InteractableDisplay>().enabled = false;
	}

	private void OnEnable()
	{
		GetComponent<InteractableDisplay>().enabled = true;
	}
}