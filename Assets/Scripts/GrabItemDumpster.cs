using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItemDumpster : GrabItem
{
	protected override void Update()
    {
		Vector3 mousePos = Input.mousePosition - new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
		mousePos *= 1.2f;
		mousePos += Input.mousePosition;
		grabPoint.position = head.ScreenToWorldPoint(mousePos + (Vector3.forward * holdDistance) + Vector3.forward) + (transform.forward * holdDistance);
		base.Update();
	}
}
