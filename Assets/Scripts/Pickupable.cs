using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Pickupable : Interactable
{
	public Interactor interactor;
	[Space]
	[Header("Pickupable variables")]
	[Tooltip("The distance the item is held in front of the player.")]
	[SerializeField] protected float holdDistance = 1.5f;
	protected bool setColliderToTrigger = false;

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
	}

	public override void InteractContinue()
	{
		instructions.SetPrompt(key, endVerb + " " + TextEffects.EmphasizeWord(name, Color.red));
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
		Collider[] colliders = GetComponents<Collider>();
		Bounds[] bounds = colliders.Select(x => x.bounds).ToArray();
		if (interactor.TryGetComponent(out Collider interactorCollider))
			foreach (Collider coll in colliders)
				{
					//coll.bounds = new Bounds(Vector3.one * 1000, Vector3.one * 0.1f);
					Physics.IgnoreCollision(interactorCollider, coll, true);
					coll.isTrigger = setColliderToTrigger;
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

		if (interactorCollider)
			foreach (Collider coll in GetComponents<Collider>())
			{
				Physics.IgnoreCollision(transform.GetComponent<Collider>(), coll, false);
				coll.isTrigger = false;
			}

		Destroy(grabPoint.gameObject);
	}

	protected virtual void SetGrabPointPosition(Transform grabPoint)
	{
		grabPoint.localPosition = new Vector3(0, 0, holdDistance);
	}
}
