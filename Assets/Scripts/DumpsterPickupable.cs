using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DumpsterPickupable : Pickupable
{
	public Camera cam;
	[SerializeField] List<Collider> validPlacements;
	private DisplayWarning warning;

	protected override void Awake()
	{
		base.Awake();
		if (!cam)
			Debug.LogError("dumpster pickup items need their camera assigned");
		foreach (Collider validPlacement in validPlacements)
		{
			if (!validPlacement.isTrigger)
				Debug.LogError("dumpster pickup items require triggers and only triggers for a valid placement zone.");
		}
		if (validPlacements.Count == 0)
			Debug.LogError("dumpster pickups must have at least one valid placement zone");
		setColliderToTrigger = true;
		warning = FindObjectOfType<DisplayWarning>();
	}

	protected override void SetGrabPointPosition(Transform grabPoint)
	{
		Vector3 mousePos = Input.mousePosition;// - new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
		//mousePos *= 1.1f;
		//mousePos += Input.mousePosition;
		//Debug.LogError("you need to set the dumpster camera");
		grabPoint.position = cam.ScreenToWorldPoint(mousePos + (Vector3.forward * holdDistance) + Vector3.forward);// + (transform.forward * holdDistance);
	}

	public override void InteractEnd()
	{
		bool valid = false;
		foreach (Collider item in validPlacements)
		{
			if (item.bounds.Contains(transform.position))
				valid = true;
		}
		if (valid)
		{
			Debug.Log("detected valid placement for " + name);
			base.InteractEnd();
		}
		else
		{
			Debug.Log("detected not valid placement for " + name);
			warning.Warning("Can not place " + name + " there.");
		}
	}
}
