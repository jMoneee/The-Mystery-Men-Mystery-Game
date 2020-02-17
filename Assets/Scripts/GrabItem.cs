using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// Allows a player to pick up items and carry them to another place to be dropped.
/// </summary>
public class GrabItem : MonoBehaviour
{
	[SerializeField] KeyCode pickupKey = KeyCode.E;
	[Tooltip("The max raycast distance to pick up an item.")]
	[SerializeField] protected float grabDistanceLimit = 2.5f;
	[Tooltip("The distance the item is held in front of the player.")]
	[SerializeField] protected float holdDistance = 1.5f;
	[SerializeField] protected Transform grabPoint;
	public Interactable grabbedItem;
	//private GameObject underWeight;
	[SerializeField] protected bool grab = false;
	protected RaycastHit hit;
	protected Camera head;
	private Coroutine cor;

	protected virtual void Awake()
    {
		head = GetComponentInChildren<Camera>();
		grabPoint = new GameObject("Grab Point").transform;
		grabPoint.parent = head.transform;
		grabPoint.localPosition = (-head.transform.forward * holdDistance);
	}

	private void OnDisable()
	{
		if (cor != null)
			StopCoroutine(cor);
	}

	protected virtual void Update()
    {
        Ray ray = head.ScreenPointToRay(Input.mousePosition);

		bool rayhit = Physics.Raycast(ray, out hit, grabDistanceLimit, -1, QueryTriggerInteraction.Ignore);
		Interactable pickup = null;
		if (rayhit)
			pickup = hit.collider.GetComponent<Interactable>();

		if (Input.GetKeyDown(pickupKey))
		{
			if (grabbedItem == null)
			{
				if (rayhit && pickup != null)
				{
					if (pickup.enabled)
					{
						grab = true;
						if (pickup.interactAction.GetPersistentEventCount() != 0)
							pickup.interactAction?.Invoke();
						else
							cor = StartCoroutine(HoldItem());
						pickup.EndHover(pickupKey);
					}
				}
			}
			else
			{
				grab = false;
			}
		}

		if (!grab && rayhit && pickup != null)
		{
			//show pickup text
			pickup.StartHover(pickupKey, pickup.gameObject);
		}
		else if (grab && grabbedItem != null)
		{
			//hide pickup text
			//show drop text
			grabbedItem.ActiveHover(pickupKey, grabbedItem.gameObject);
		}
		else
		{
			//hide pickup and drop text
			//display.RemovePrompt(pickupKey);
		}
	}

	/// <summary>
	/// Sets initial states needed for picking up the item including assigning values, changing parent, and
	/// preventing collision with the player so that we don't end up with weird unwanted physics interactions.
	/// </summary>
	/// <returns>Coroutine</returns>
	protected virtual IEnumerator HoldItem()
	{
		grabbedItem = hit.collider.gameObject.GetComponent<Pickupable>();
		Transform ogParent = grabbedItem.transform.parent;
		Rigidbody grabbedrb = grabbedItem.GetComponent<Rigidbody>();

		grabbedrb.useGravity = false;
		grabbedItem.transform.parent = grabPoint.transform;

		Quaternion ogRotation = grabbedItem.transform.rotation;
		float time = 0f;
		foreach (Collider coll in grabbedItem.GetComponents<Collider>())
		{
			Physics.IgnoreCollision(transform.GetComponent<Collider>(), coll, true);
		}

		//hold at the center of the object itself by referencing the renderer, NOT transform.
		Renderer r = grabbedItem.GetComponent<Renderer>();
		if (r == null)
			r = grabbedItem.GetComponentInChildren<Renderer>();
		while (grab)
		{
			Vector3 grabbedCenter = r.bounds.center;
			time += Time.fixedDeltaTime;
			grabbedrb.velocity = (grabPoint.position - grabbedCenter) * Time.fixedDeltaTime * 1000;
			//grabbedItem.position = Vector3.MoveTowards(grabbedItem.position, grabPoint.position, Time.fixedDeltaTime * 10);
			grabbedItem.transform.rotation = Quaternion.Lerp(ogRotation, Quaternion.identity, time * 2);
			yield return new WaitForFixedUpdate();
		}
		grabbedItem.transform.parent = ogParent;
		grabbedrb.useGravity = true;

		foreach (Collider coll in grabbedItem.GetComponents<Collider>())
		{
			Physics.IgnoreCollision(transform.GetComponent<Collider>(), coll, false);
		}
		grabbedItem = null;
		cor = null;
	}
}
