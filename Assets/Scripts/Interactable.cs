using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
	[SerializeField] bool _pickUp = false;
	public bool pickUp { get { return _pickUp; } }

	public void OnPointerEnter(PointerEventData eventData)
	{
		throw new System.NotImplementedException();
	}

	public void OnPointerExit(PointerEventData eventData)
	{
		throw new System.NotImplementedException();
	}
}
