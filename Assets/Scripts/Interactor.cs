using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

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
		if (prevIntable != null)
			foreach (Interactable item in prevIntable)
			{
				if (item.interacting)
					item.InteractEnd();
				else
					item.HoverEnd();
			}

		prevIntable = null;
	}

	private List<Interactable> prevIntable = null;
	protected virtual void Update()
    {
        Ray ray = head.ScreenPointToRay(Input.mousePosition);

		//ignore layer 2, which is the Ignore Raycast layer.
		bool rayhit = Physics.Raycast(ray, out hit, interactDistanceLimit, ~(1 << 2), QueryTriggerInteraction.Collide);
		Interactable[] intable = null;
		if (rayhit && hit.collider.TryGetComponents(out intable))
		{
			intable = intable.Where(x => x.enabled).ToArray();

			foreach (Interactable item in intable)
			{
				if (Input.GetKeyDown(item.key))
				{
					if (!item.interacting)
					{
						item.InteractBegin();
					}
					else
					{
						item.InteractEnd();
						prevIntable = null;
					}
				}
				else if (item.interacting)
					item.InteractContinue();

				if (!item.interacting)
				{
					if (prevIntable == null || !prevIntable.Contains(item))
						item.HoverBegin();
					else if (prevIntable != null && prevIntable.Contains(item))
						item.HoverContinue();
				}
			}
			

		}
		else if (prevIntable != null)
		{
			foreach (Interactable item in prevIntable)
			{
				if (!item.interacting)
					item.HoverEnd();

			}
		}
		prevIntable = intable?.ToList();


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
