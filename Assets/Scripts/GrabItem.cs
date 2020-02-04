﻿using System;
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
	[SerializeField] float grabDistanceLimit = 2.5f;
	[Tooltip("The distance the item is held in front of the player.")]
	[SerializeField] float holdDistance = 1.5f;
	private Transform grabPoint;
	public Transform grabbedItem;
	//private GameObject underWeight;
	[SerializeField] bool grab = false;
	private RaycastHit hit;
	private Camera head;
	private DisplayInstructions display;

    void Start()
    {
		head = Camera.main;
		grabPoint = new GameObject("Grab Point").transform;
		grabPoint.parent = head.transform;
		grabPoint.localPosition = new Vector3(0, 0, holdDistance);
		display = GetComponentInChildren<DisplayInstructions>();
	}

    void Update()
    {
        Ray ray = head.ScreenPointToRay(Input.mousePosition);

		bool rayhit = Physics.Raycast(ray, out hit, grabDistanceLimit, -1, QueryTriggerInteraction.Ignore);
		bool pickup = false;
		Debug.Log(Input.GetKeyDown(pickupKey));
		if (Input.GetKeyDown(pickupKey))
		{
			if (grabbedItem == null)
			{
				if (rayhit && hit.collider.TryGetComponent(out Interactable inter) && inter.pickUp)
				{
					grab = true;
					pickup = true;
					StartCoroutine(HoldItem());
				}
			}
			else
			{
				grab = false;
			}
		}

		if (!grab && rayhit && hit.collider.TryGetComponent(out Interactable interact) && interact.pickUp)
		{
			//show pickup text
			display.SetPrompt(pickupKey, "pick up", hit.collider.gameObject.name);
		}
		else if (grab)
		{
			//hide pickup text
			//show drop text
			display.SetPrompt(pickupKey, "drop ", grabbedItem.name);
		}
		else
		{
			//hide pickup and drop text
			display.RemovePrompt(pickupKey);
		}
		//if (rayhit)
		//{
		//	Interactable interact = hit.collider.GetComponent<Interactable>();
		//	if (interact != null && interact.pickUp)
		//	{
		//		//show text for press key to pick up
		//		grab = true;
		//		StartCoroutine(HoldItem());
		//	}
		//}
	}

	/// <summary>
	/// Sets initial states needed for picking up the item including assigning values, changing parent, and
	/// preventing collision with the player so that we don't end up with weird unwanted physics interactions.
	/// </summary>
	/// <returns>Coroutine</returns>
	private IEnumerator HoldItem()
	{
		grabbedItem = hit.collider.gameObject.transform;
		Transform ogParent = grabbedItem.transform.parent;
		Rigidbody grabbedrb = grabbedItem.GetComponent<Rigidbody>();

		grabbedrb.useGravity = false;
		grabbedItem.transform.parent = grabPoint.transform;

		Quaternion ogRotation = grabbedItem.rotation;
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
			grabbedItem.rotation = Quaternion.Lerp(ogRotation, Quaternion.identity, time * 2);
			yield return new WaitForFixedUpdate();
		}
		grabbedItem.transform.parent = ogParent;
		grabbedrb.useGravity = true;

		foreach (Collider coll in grabbedItem.GetComponents<Collider>())
		{
			Physics.IgnoreCollision(transform.GetComponent<Collider>(), coll, false);
		}
		grabbedItem = null;
	}

	private void OnValidate()
	{
#if UNITY_EDITOR
		//allows us to update hold distance during playtesting if we want to
		if (EditorApplication.isPlaying && grabPoint != null)
			grabPoint.localPosition = new Vector3(0, 0, holdDistance);
#endif
	}
}
