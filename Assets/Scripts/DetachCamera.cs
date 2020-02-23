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
    }

    private RigidbodyFirstPersonController firstPersonController;

    private int cachedDetachments = 0;

    // Start is called before the first frame update
    void Start()
    {
        firstPersonController = GetComponent<RigidbodyFirstPersonController>();
    }

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
