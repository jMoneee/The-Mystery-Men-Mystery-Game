using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrabItemDumpster : GrabItem
{
	protected override void Update()
    {
		base.Update();
		grabPoint.localPosition = head.ScreenToWorldPoint(Input.mousePosition) + new Vector3(0, 0, holdDistance);
	}
}
