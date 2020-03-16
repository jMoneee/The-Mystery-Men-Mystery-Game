using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.Characters.FirstPerson;

public class DetachCamera : MonoBehaviour
{
    public static DetachCamera instance;
    private void Awake()
    {
        instance = this;
		firstPersonController = GetComponent<RigidbodyFirstPersonController>();
	}

	private RigidbodyFirstPersonController firstPersonController;

    private int cachedDetachments = 0;

    public static void Detach()
    {
        instance.cachedDetachments++;
        instance.firstPersonController.enabled = false;
        //mouseLook.ena = false;
    }

    public static void Reattach()
    {
        instance.cachedDetachments--;
        if (instance.cachedDetachments <= 0)
        {
            instance.firstPersonController.enabled = true;
        }
    }
}
