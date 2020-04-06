using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowObjectAfterTimer : MonoBehaviour
{
	private Coroutine co;
	public int timer = 60;

	private void OnEnable()
	{
		co = StartCoroutine(Timer());
	}

	private IEnumerator Timer()
	{
		yield return new WaitForSeconds(timer);
		GetComponent<MeshRenderer>().enabled = true;
	}

	private void OnDisable()
	{
		if (co != null)
		{
			StopCoroutine(co);
			co = null;
		}
	}
}
