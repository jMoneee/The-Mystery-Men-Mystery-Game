using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickupable : Interactable
{
	private Interactor interactor;
	[Space]
	[Header("Pickupable variables")]
	[Tooltip("The distance the item is held in front of the player.")]
	[SerializeField] protected float holdDistance = 1.5f;

	protected override void Start()
	{
		interactor = FindObjectOfType<Interactor>();
		base.Start();
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
		_interacting = true;
		StartCoroutine(Hold());
		instructions.SetPrompt(key, endVerb + " " + TextEffects.EmphasizeWord(name, Color.red));
	}

	public override void InteractContinue()
	{
	}

	public override void InteractEnd()
	{
		_interacting = false;
	}

	protected virtual IEnumerator Hold()
	{
		Transform grabPoint = new GameObject("Grab Point").transform;
		grabPoint.parent = interactor.GetComponentInChildren<Camera>().transform;
		Transform ogParent = transform.parent;
		Rigidbody grabbedrb = GetComponent<Rigidbody>();
		SetGrabPointPosition(grabPoint);

		grabbedrb.useGravity = false;
		transform.parent = grabPoint;

		Quaternion ogRotation = transform.rotation;
		float time = 0f;
		foreach (Collider coll in GetComponents<Collider>())
		{
			Physics.IgnoreCollision(interactor.GetComponent<Collider>(), coll, true);
		}

		//hold at the center of the object itself by referencing the renderer, NOT transform.
		Renderer r = GetComponent<Renderer>();
		if (r == null)
			r = GetComponentInChildren<Renderer>();
		while (interacting)
		{
			SetGrabPointPosition(grabPoint);
			Vector3 grabbedCenter = r.bounds.center;
			time += Time.fixedDeltaTime;
			grabbedrb.velocity = (grabPoint.position - grabbedCenter) * Time.fixedDeltaTime * 1000;
			//grabbedItem.position = Vector3.MoveTowards(grabbedItem.position, grabPoint.position, Time.fixedDeltaTime * 10);
			transform.rotation = Quaternion.Lerp(ogRotation, Quaternion.identity, time * 2);
			yield return new WaitForFixedUpdate();
		}
		transform.parent = ogParent;
		grabbedrb.useGravity = true;

		foreach (Collider coll in GetComponents<Collider>())
		{
			Physics.IgnoreCollision(transform.GetComponent<Collider>(), coll, false);
		}
	}

	protected virtual void SetGrabPointPosition(Transform grabPoint)
	{
		grabPoint.localPosition = new Vector3(0, 0, holdDistance);
	}
}
