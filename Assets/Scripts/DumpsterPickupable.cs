using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DumpsterPickupable : Pickupable
{
	protected override void SetGrabPointPosition(Transform grabPoint)
	{
		Vector3 mousePos = Input.mousePosition - new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
		mousePos *= 1.2f;
		mousePos += Input.mousePosition;
		Debug.LogError("you need to set the dumpster camera");
		grabPoint.position = Camera.main.ScreenToWorldPoint(mousePos + (Vector3.forward * holdDistance) + Vector3.forward) + (transform.forward * holdDistance);
	}
}
