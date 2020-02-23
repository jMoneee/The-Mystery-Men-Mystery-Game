using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

/// <summary>
/// The script through which the player can interact with objects in the environment.
/// Objects must be of type Interactable.
/// </summary>
public class Interactor : MonoBehaviour
{
	[Tooltip("The max raycast distance to pick up an item.")]
	[SerializeField] protected float interactDistanceLimit = 2.5f;
	//[SerializeField] protected Transform grabPoint;
	//public Interactable grabbedItem;
	//private GameObject underWeight;
	//[SerializeField] protected bool grab = false;
	protected RaycastHit hit;
	protected Camera head;
	//private Coroutine cor;

	protected virtual void Awake()
    {
		head = GetComponentInChildren<Camera>();
		//grabPoint = new GameObject("Grab Point").transform;
		//grabPoint.parent = head.transform;
		//grabPoint.localPosition = (-head.transform.forward * holdDistance);
	}

	private void OnDisable()
	{
		//if (cor != null)
		//	StopCoroutine(cor);
	}

	private Interactable prevIntable = null;
	protected virtual void Update()
    {
        Ray ray = head.ScreenPointToRay(Input.mousePosition);

		bool rayhit = Physics.Raycast(ray, out hit, interactDistanceLimit, -1, QueryTriggerInteraction.Ignore);
		Interactable intable = null;
		if (rayhit && hit.collider.TryGetComponent(out intable))
		{
			if (Input.GetKeyDown(intable.key))
			{
				if (!intable.interacting)
					intable.InteractBegin();
				else
				{
					Debug.Log("begin interact -- interactor");
					intable.InteractEnd();
				}
			}
			else if (intable.interacting)
				intable.InteractContinue();

			if (prevIntable == null)
				intable.HoverBegin();
			if (prevIntable == intable)
				intable.HoverContinue();
		}
		else if (prevIntable != null)
		{
			prevIntable.HoverEnd();
		}
		prevIntable = intable;


		//if (Input.GetKeyDown(pickupKey))
		//{
		//	if (grabbedItem == null)
		//	{
		//		if (rayhit && pickup != null)
		//		{
		//			if (pickup.enabled)
		//			{
		//				grab = true;
		//				if (pickup.interactAction.GetPersistentEventCount() != 0)
		//					pickup.interactAction?.Invoke();
		//				else
		//					cor = StartCoroutine(HoldItem());
		//				pickup.EndHover(pickupKey);
		//			}
		//		}
		//	}
		//	else
		//	{
		//		grab = false;
		//	}
		//}

		//if (!grab && rayhit && pickup != null)
		//{
		//	//show pickup text
		//	pickup.StartHover(pickupKey, pickup.gameObject);
		//}
		//else if (grab && grabbedItem != null)
		//{
		//	//hide pickup text
		//	//show drop text
		//	grabbedItem.DuringHover(pickupKey, grabbedItem.gameObject);
		//}
		//else
		//{
		//	//hide pickup and drop text
		//	//display.RemovePrompt(pickupKey);
		//}
	}
}
